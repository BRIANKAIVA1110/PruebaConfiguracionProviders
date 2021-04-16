using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PepeConfiguration.API.Hubs
{
    public class PepeConfigurationHub:Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("pepeTopic", message);
        }
    }
}
