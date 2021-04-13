using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace PepeConfiguration
{
    public class DataBaseConfigurationProvider : ConfigurationProvider,IDisposable
    {
        private readonly IDbConnection _connection;
        private readonly PepeConfigurationOptions _configure;
        private bool _isFirsExcution = true;

        private static CancellationTokenSource _cancellationToken;

        public DataBaseConfigurationProvider(PepeConfigurationOptions configure)
        {
            this._configure = configure;
            this._connection = new SqlConnection(configure.DataSourceConnectionString);//manejar exepcion
            
        }

        static DataBaseConfigurationProvider() 
        {
            _cancellationToken = new CancellationTokenSource();
        }

        public override async void Load()
        {
            if (!_configure.ReloadAnyTime) 
            {
                Task.WaitAll(LoadAsync());
                _isFirsExcution = false;
            }

            //por lo menos 1 vez tiene que esperar a que se ejecute..despues como sub
            if (_isFirsExcution) 
            {
                Task.WaitAll(LoadAsync());// por lo menos 1 vez se tiene que esperar 
                _isFirsExcution = false;
                await Task.Run(PollForChangesAsync, _cancellationToken.Token);
            }
                
        }

        private async Task LoadAsync() 
        {
            var dataFromDataBase = new Dictionary<string, string>();

            string sqlQuery = "select Section, [key], Value from configurations";
            var configuraciones = await _connection.QueryAsync<Configuration>(sqlQuery);

            foreach (var item in configuraciones)
            {
                dataFromDataBase.Add($"{item.Section}:{item.Key}", item.Value);
            }

            Data = dataFromDataBase;

            if (_configure.ReloadAnyTime)
                await Task.Delay(_configure.TimeReloadAt, _cancellationToken.Token);
        }

        private async Task PollForChangesAsync() 
        {
            if (_configure.ReloadAnyTime)
                while (!_cancellationToken.IsCancellationRequested) 
                    if(!_isFirsExcution)
                        await LoadAsync();
            else 
                await LoadAsync();
        }


        public void Dispose()
        {
            _connection.Dispose();
            _cancellationToken.Cancel();
        }

    }
}
