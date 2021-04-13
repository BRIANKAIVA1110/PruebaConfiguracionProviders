using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PepeConfiguration
{
    public class DataBaseConfigurationSource : IConfigurationSource
    {
        
        private readonly PepeConfigurationOptions _configure;

        public DataBaseConfigurationSource(PepeConfigurationOptions configure)
        {
            this._configure = configure;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DataBaseConfigurationProvider(_configure);
        }
    }
}
