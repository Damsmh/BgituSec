using BgituSec.Application.DTOs;
using Microsoft.AspNetCore.Http;
using System.Threading.Channels;

namespace BgituSec.Application.Services.SSE
{
    public interface ISSEService
    {
        public void AddClient(HttpResponse response);
        public void RemoveClient(HttpResponse response);
        public Task NotifyClientsAsync(string message);

    }
}
