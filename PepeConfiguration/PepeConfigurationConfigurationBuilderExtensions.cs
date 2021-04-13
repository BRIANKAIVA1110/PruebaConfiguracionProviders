using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace PepeConfiguration
{
    public static class PepeConfigurationConfigurationBuilderExtensions
    {

        public static IConfigurationBuilder AddPepeConfiguration(this IConfigurationBuilder builder, Action<PepeConfigurationOptions> configure)
        {
            var pepeConfigurationOpcions = new PepeConfigurationOptions();
            configure?.Invoke(pepeConfigurationOpcions);

            if (pepeConfigurationOpcions.DataSourceConnectionString == null)
                throw new ArgumentException(nameof(pepeConfigurationOpcions.DataSourceConnectionString));

            builder.Add(new DataBaseConfigurationSource(pepeConfigurationOpcions));

            return builder;
        }
    }
}
