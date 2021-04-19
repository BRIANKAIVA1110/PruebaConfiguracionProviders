using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace PepeConfiguration
{
    interface IBus 
    {
        void StartReceiving();
    }

    class Bus : IBus
    {
        public void StartReceiving()
        {
            throw new NotImplementedException();
        }
    }
    public class DataBaseConfigurationProvider : ConfigurationProvider, IDisposable
    {
        private readonly IDbConnection _connection;
        private readonly PepeConfigurationOptions _configure;
        private HubConnection _hubConnection;

        private HttpClient _httpClient;

        public DataBaseConfigurationProvider(PepeConfigurationOptions configure)
        {
            this._configure = configure;

            this._httpClient = new HttpClient();
            //this._connection = new SqlConnection(configure.DataSourceConnectionString);//manejar exepcion, DI??
            //InicializarClientListenerCambiosConfiguracion(configure.EndpointHubListerner);
        }

        public override void Load()
        {

            /*
             * HTTP GET DE CONFIG
             */
            //string sqlQuery = "select Section, [key], Value from configurations";//deberia estar en otra clase "dbContext | repository"
            //var configuraciones =  _connection.Query<Configuration>(sqlQuery);//deberia estar en otra clase "dbContext | repository"

            //if "lastModifed" != db.lastModifed <- ver factibilidad
            //var dataFromDataBase = configuraciones.ToDictionary(x => $"{ x.Section }:{ x.Key }", x => x.Value);

             //Data= null;


        }

        private IDictionary<string, string> GetConfigurations() //Async
        {
            throw new NotImplementedException();
        }

        private void InicializarClientListenerCambiosConfiguracion(string endpointHubListerner)
        {
            _hubConnection = new HubConnectionBuilder().WithUrl(endpointHubListerner)
            .Build();

            _hubConnection.StartAsync().Wait();//MANEJAR Exception CUANDO EL SERVER NO ESTA INICIADO
            //EL TOPIC DEBE SER DINAMICO...
            //SE SUPONE QUE ESTO GENERA POR REFLECTION UN METODO "pepeTopic" CON FUNCIONALIDAD SEGUN EXPRE LAMBDA.
            //AL CREAR EL METODO POR REFLECTION EL SERVIDOR ES EL QUE EJECUTARIA ESTE METODO.
            //LOS CLIENTES TAMBIEN PUEDE EJECUTAR METODOS DEL SERVIDOR -> "_connection.InvokeAsync(SendMessage, ....)"
            _hubConnection.On<string>("pepeTopic", (x) => Load());
        }

        public async void Dispose() //si se genera una exeption en un metodo async con retorno void, si no me equivoco, hace que falle la ejecucion del sistema. leer sobre el tema.
        {
            _connection.Dispose();
            await _hubConnection.DisposeAsync();
        }

    }
}
