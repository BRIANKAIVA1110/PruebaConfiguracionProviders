using System;
using System.Collections.Generic;
using System.Text;

namespace PepeConfiguration
{
    public class PepeConfigurationOptions
    {
        public string DataSourceConnectionString { get; set; }
        public string Credentials { get; set; } // seguridad para la dll???
        public string Environment { get; set; }
        public string ApplicationName { get; set; }
        public bool ReloadAnyTime { get; set; }
        public TimeSpan TimeReloadAt { get; set; }
    }
}
