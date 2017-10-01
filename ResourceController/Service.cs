using Newtonsoft.Json;
using SharedHardware.NetworkTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceController
{
    class Service : ServiceBase
    {
        private async Task Interval(Func<Task> action, TimeSpan interval, CancellationToken token)
        {
            await action();
            await Task.Delay(interval, token);
            await Interval(action, interval, token);
        }
        private async Task UpdateStatus(string serviceUrl, Guid platformId)
        {
            try
            {
                using (var request = new System.Net.Http.HttpClient { Timeout = TimeSpan.FromSeconds(30) })
                {
                    var servializer = new JsonSerializer { };
                    using (var memoryStream = new MemoryStream())
                    {
                        using (TextWriter textWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                        {
                            servializer.Serialize(textWriter, new NodeStatusUpdate { PlatformId = platformId });
                        }
                        await memoryStream.FlushAsync();
                        var content = new StreamContent(memoryStream, (int)memoryStream.Length);
                        var response = await request.PostAsync(serviceUrl, content);
                        if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
                        {
                            //TODO: log that service returned not 204 to system log
                        }
                    }
                }
            }
            catch (Exception) //network exception - TODO: catch SocketException 
            {
                //TODO - log result to system log 
            }
        }
        private CancellationTokenSource updaterCancelationTokenSource; 
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            //TODO: from config
            var serviceUrl = "";
            var platformId = Guid.Empty;
            updaterCancelationTokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(() => 
                Interval(async () => await UpdateStatus(serviceUrl, platformId), 
                    TimeSpan.FromMinutes(2), updaterCancelationTokenSource.Token));
        }

        protected override void OnStop()
        {
            updaterCancelationTokenSource.Cancel();
            base.OnStop();
        }
    }
}
