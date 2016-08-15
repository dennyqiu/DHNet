using DHNet.Components.Security;
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;

namespace DHNet.Components.Logging
{
    public class Logger : ILogger
    {
        private Int32? AccountId { get; set; }
        private static Object LogWriting = new Object();

        public Logger()
        {
        }
        public Logger(Int32? accountId)
        {
            AccountId = accountId;
        }

        public void Log(String message)
        {
            Int32? accountId = AccountId ?? (HttpContext.Current.User != null ? HttpContext.Current.User.Id() : null);
            Int64 backupSize = Int64.Parse(WebConfigurationManager.AppSettings["LogBackupSize"]);
            String logDirectoryPath = WebConfigurationManager.AppSettings["LogPath"];
            String basePath = HostingEnvironment.ApplicationPhysicalPath ?? "";
            logDirectoryPath = Path.Combine(basePath, logDirectoryPath);
            String logPath = Path.Combine(logDirectoryPath, "Log.txt");

            StringBuilder log = new StringBuilder();
            log.AppendLine("Time   : " + DateTime.Now.ToString("yyyy-dd-MM HH:mm:ss"));
            log.AppendLine("Account: " + accountId);
            log.AppendLine("Message: " + message);
            log.AppendLine();

            lock (LogWriting)
            {
                Directory.CreateDirectory(logDirectoryPath);
                File.AppendAllText(logPath, log.ToString());

                if (new FileInfo(logPath).Length >= backupSize)
                {
                    String logBackupFile = String.Format("Log {0}.txt", DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
                    String backupPath = Path.Combine(logDirectoryPath, logBackupFile);
                    File.Move(logPath, backupPath);
                }
            }
        }
        public void Log(Exception exception)
        {
            while (exception.InnerException != null)
                exception = exception.InnerException;

            String message = String.Format("{0}: {1}{2}{3}",
                    exception.GetType(),
                    exception.Message,
                    Environment.NewLine,
                    exception.StackTrace);

            Log(message);
        }
    }
}
