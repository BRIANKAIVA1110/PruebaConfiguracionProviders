using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PepeConfiguration.API.Infraestructura.Entities
{
    public class Configuracion
    {
        public int Id { get; set; }
        public string Section { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
