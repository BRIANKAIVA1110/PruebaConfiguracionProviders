using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PepeConfiguration.API.DTOs
{
    public class ConfiguracionDTO
    {
        public int? Id { get; set; }
        public string Section { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Cluster { get; set; }
        public string ApplicationName { get; set; }
    }
}
