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
using Microsoft.AspNetCore.SignalR.Client;

namespace PepeConfiguration
{
    public class DataBaseConfigurationProvider : ConfigurationProvider,IDisposable
    {
        private readonly IDbConnection _connection;
        private readonly PepeConfigurationOptions _configure;

        private static CancellationTokenSource _cancellationToken;

        public DataBaseConfigurationProvider(PepeConfigurationOptions configure)
        {
            this._configure = configure;
            this._connection = new SqlConnection(configure.DataSourceConnectionString);//manejar exepcion

            if (!(configure.EndpointHubListerner is null)) 
            {
                var _connection = new HubConnectionBuilder().WithUrl(configure.EndpointHubListerner)
                .Build();

                _connection.StartAsync().Wait();
                //EL TOPIC DEBE SER DINAMICO...
                _connection.On<string>("pepeTopic", async (x) =>
                {
                    await LoadAsync();
                });
            }
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
            }
            else 
            {
                //por lo menos 1 vez tiene que esperar a que se ejecute... despues como "subproceso"
                Task.WaitAll(LoadAsync());// por lo menos 1 vez se tiene que esperar 
                await Task.Delay(_configure.TimeReloadAt, _cancellationToken.Token);
                await Task.Run(PollForChangesAsync, _cancellationToken.Token);
            }
        }

        private async Task LoadAsync() 
        {
            string sqlQuery = "select Section, [key], Value from configurations";//deberia estar en otra clase "dbContext | repository"
            var configuraciones = await _connection.QueryAsync<Configuration>(sqlQuery);//deberia estar en otra clase "dbContext | repository"

            //if "lastModifed" != db.lastModifed <- ver factibilidad
            var dataFromDataBase = configuraciones.ToDictionary(x => $"{ x.Section }:{ x.Key }", x => x.Value);

            Data = dataFromDataBase;

            if (_configure.ReloadAnyTime)
                await Task.Delay(_configure.TimeReloadAt, _cancellationToken.Token);
        }

        private async Task PollForChangesAsync() 
        {
            if (_configure.ReloadAnyTime)
                while (!_cancellationToken.IsCancellationRequested) 
                        await LoadAsync();
           
        }


        public void Dispose()
        {
            _connection.Dispose();
            _cancellationToken.Cancel();
        }

    }
}
