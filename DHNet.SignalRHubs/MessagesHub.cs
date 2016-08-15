using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR; 
using Microsoft.AspNet.SignalR.Hubs;


namespace DHNet.SignalRHubs
{
    public class MessagesHub : Hub 
     { 
        //private static string conString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString(); 
         public void Hello()
         { 
            Clients.All.hello(); 
        } 
  
         [HubMethodName("sendMessages")] 
        public static void SendMessages()
        { 
             IHubContext context = GlobalHost.ConnectionManager.GetHubContext<MessagesHub>(); 
            context.Clients.All.updateMessages(); 
         } 
 
         
     } 

}
