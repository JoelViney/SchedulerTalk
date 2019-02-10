using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulerTalk.Hubs
{
    public class WidgetHub : Hub
    {
        public async Task SendAsync(string message)
        {
            await Clients.All.SendAsync("Send", message);
        }
    }
}
