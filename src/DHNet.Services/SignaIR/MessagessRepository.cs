
using DHNet.Components.SignalRHubs;
using DHNet.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHNet.Services
{
    public class MessagessRepository
    {
        readonly string _connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString; 
 
 
         public IEnumerable<Messagess> GetAllMessages()
         { 
             var messagess = new List<Messagess>(); 
             using (var connection = new SqlConnection(_connString)) 
             { 
                 connection.Open(); 
                 using (var command = new SqlCommand(@"SELECT [MessageID], [Message], [EmptyMessage], [Date] FROM [dbo].[Messagess]", connection)) 
                 { 
                    command.Notification = null; 
 
 
                     var dependency = new SqlDependency(command); 
                     dependency.OnChange += new OnChangeEventHandler(dependency_OnChange); 
 
 
                     if (connection.State == ConnectionState.Closed) 
                         connection.Open(); 

 
                     var reader = command.ExecuteReader(); 
 
 
                     while (reader.Read()) 
                     { 
                         messagess.Add(item: new Messagess { MessageID = (int)reader["MessageID"], Message = (string)reader["Message"], EmptyMessage =  reader["EmptyMessage"] != DBNull.Value? (string) reader["EmptyMessage"] : "", MessageDate = Convert.ToDateTime(reader["Date"]) }); 
                     } 
                 } 
                
             } 
             return messagess; 
             
              
         } 
 
 
        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        { 
             if (e.Type == SqlNotificationType.Change) 
            { 
                 MessagessHub.SendMessages(); 
            } 
         } 
     } 

  }

