using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR; 
using Microsoft.AspNet.SignalR.Hubs;


namespace DHNet.Components.SignalRHubs
{
    public class MessagessHub : Hub 
     { 
        //private static string conString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString(); 
        // public void Hello()
        // { 
        //    Clients.All.hello(); 
        //} 
  
         [HubMethodName("sendMessages")] 
        public static void SendMessages()
        { 
             IHubContext context = GlobalHost.ConnectionManager.GetHubContext<MessagessHub>();
             //context.Clients.All.updateMessages();
        } 
 
         
     } 

}
