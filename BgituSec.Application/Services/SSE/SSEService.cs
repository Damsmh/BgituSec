using Microsoft.AspNetCore.Http;
using System.Text;

namespace BgituSec.Application.Services.SSE
{
    public class SSEService : ISSEService
    {
        private readonly List<HttpResponse> _clients = [];

        public void AddClient(HttpResponse response)
        {
            lock (_clients)
            {
                _clients.Add(response);
            }
        }

        public void RemoveClient(HttpResponse response)
        {
            lock (_clients)
            {
                _clients.Remove(response);
            }
        }

        public async Task NotifyClientsAsync(string message)
        {
            var tasks = new List<Task>();
            lock (_clients)
            {
                foreach (var client in _clients.ToList())
                {
                    if (client.HttpContext.RequestAborted.IsCancellationRequested)
                    {
                        continue;
                    }

                    var data = $"data: {message}\n\n";
                    tasks.Add(Task.Run(async () =>
                    {
                        await client.WriteAsync(data, Encoding.UTF8);
                        await client.Body.FlushAsync();
                    }));
                }
            }

            await Task.WhenAll(tasks);
        }
    }
}