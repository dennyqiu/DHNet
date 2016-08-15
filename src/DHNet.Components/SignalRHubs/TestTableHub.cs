using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DHNet.Components.SignalRHubs
{
    [HubName("TestTableService")]
    public class TestTableHub: Hub
    {
        [HubMethodName("show")]
        public static void show()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TestTableHub>();
            //context.Clients.All.displaydatas();
        }
    }
}