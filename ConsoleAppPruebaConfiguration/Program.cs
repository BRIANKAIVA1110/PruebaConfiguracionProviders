using Microsoft.Extensions.Configuration;
using PepeConfiguration;
using System;

namespace ConsoleAppPruebaConfiguration
{
    class Program
    {
        private const string DataSourceConnectionString = @"Server = DESKTOP-J4947VK\SQLEXPRESS; Database = PruebaRestful; Integrated Security = True;";
        static void Main(string[] args)
        {
            IConfiguration _configuration = new ConfigurationBuilder().AddPepeConfiguration(config =>
            {
                config.DataSourceConnectionString = DataSourceConnectionString;
                config.Environment = Environment.GetEnvironmentVariable("DOTNET_CONSOLEAPP");
                config.ApplicationName = "PEPEAPP";
                config.Credentials = "";
                config.ReloadAnyTime = true;
                config.TimeReloadAt = TimeSpan.FromSeconds(5);
            }).Build();
            
            

            var section = _configuration["PEPE:NOMBRE"];

            Console.WriteLine($"Informacion configuracion: {section}");
            Console.WriteLine("Hello World!");


            Console.ReadKey();
        }
    }
}
