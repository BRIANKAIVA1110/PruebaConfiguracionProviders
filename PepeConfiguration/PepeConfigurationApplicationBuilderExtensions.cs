using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PepeConfiguration
{
    public static class PepeConfigurationApplicationBuilderExtensions
    {

        public static IApplicationBuilder UsePepeConfiguration(this IApplicationBuilder builder) 
        {
            builder.Use(async(context, next) =>
            {
                var configurationRoot = context.RequestServices.GetService(typeof(IConfiguration)) as ConfigurationRoot;

                var dataBaseConfigurationProvider = configurationRoot.Providers.First(x => x is DataBaseConfigurationProvider);
                dataBaseConfigurationProvider.Load();

                await next();
            });

            return builder;
        }
    }
}
