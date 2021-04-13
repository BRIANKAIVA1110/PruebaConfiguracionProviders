using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public static class ServiceLayerServiceCollectionExtensions
    {

        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {

            services.AddTransient<IService, Service>();

            return services;
        }
    }
}
