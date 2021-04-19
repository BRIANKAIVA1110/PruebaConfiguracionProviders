using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PepeConfiguration;
using System;

namespace PruebaConfiguracion
{
    public class Program
    {
        private const string DataSourceConnectionString = @"Server = DESKTOP-J4947VK\SQLEXPRESS; Database = PruebaRestful; Integrated Security = True;";
        private const string EndPointHubListener = @"https://localhost:44325/pepeConfiguracion";
        public static void Main(string[] args)
        {
            ServiceCollection sCollection = new ServiceCollection();
            sCollection.AddTransient<Ibus>();

            ServiceProvider sp = sCollection.BuildServiceProvider();

            var a = Environment.GetEnvironmentVariables();


            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((contextBuilder, config) =>
                {
                    var env = contextBuilder.HostingEnvironment;

                    config.AddPepeConfiguration(config =>
                    {
                        config.DataSourceConnectionString = DataSourceConnectionString;
                        config.Credentials = "";
                        config.Environment = env.EnvironmentName;
                        config.ApplicationName = env.ApplicationName;
                        config.EndpointHubListerner = EndPointHubListener;


                        //config.ReloadAnyTime = true;
                        //config.TimeReloadAt = TimeSpan.FromSeconds(4);
                    });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
