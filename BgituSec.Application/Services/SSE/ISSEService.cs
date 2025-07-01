using System.Threading.Channels;

namespace BgituSec.Application.Services.SSE
{
    public interface ISSEService
    {
        Task RegisterClientAsync(string clientId, ChannelWriter<string> writer, CancellationToken cancellationToken);
    }
}
