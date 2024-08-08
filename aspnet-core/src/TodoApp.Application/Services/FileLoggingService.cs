using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Services
{
    public class FileLoggingService : ILoggingService
    {
        public async Task Log(string userId, string serviceName)
        {
            var logUser = new LogUser { UserId = userId, ServiceName = serviceName };
            System.Diagnostics.Debug.WriteLine(logUser.Id.ToString());


            await WriteLog(logUser);
        }

        private async Task WriteLog(LogUser data)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("Logs/logUsers.txt", true))
                {
                    var dataId = data.Id.ToString();
                    await writer.WriteLineAsync($"({dataId}) ({data.ExecutionTime}) " +
                        $"({data.UserId}) ({data.ServiceName})");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to write log entry to file: {ex.Message}");
            }

        }
    }
}
