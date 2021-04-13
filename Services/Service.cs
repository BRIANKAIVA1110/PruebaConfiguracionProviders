using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class Service : IService
    {
        private readonly IConfiguration _configuration;

        public Service(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public int GetNumber()
        {
            var a = _configuration["pepe"];
            return 1;
        }
    }
}
