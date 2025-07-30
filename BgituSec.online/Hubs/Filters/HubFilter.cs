using Microsoft.AspNetCore.SignalR;

namespace BgituSec.Api.Hubs.Filters
{
    public class RoleBasedGroupHubFilter(IHubContext<DynamicHub> hubContext) : IHubFilter
    {
        private readonly IHubContext<DynamicHub> _hubContext = hubContext;

        public async ValueTask<object> InvokeMethodAsync(
            HubInvocationContext invocationContext,
            Func<HubInvocationContext, ValueTask<object>> next)
        {
            return await next(invocationContext);
        }

        public async Task OnConnectedAsync(HubConnectionContext connection, Func<HubConnectionContext, Task> next)
        {
            var user = connection.User;
            if (user != null)
            {
                if (user.IsInRole("ROLE_ADMIN"))
                {
                    await _hubContext.Groups.AddToGroupAsync(connection.ConnectionId, "Admins");
                }
                if (user.IsInRole("ROLE_USER"))
                {
                    await _hubContext.Groups.AddToGroupAsync(connection.ConnectionId, "Users");
                }
            }
            await next(connection);
        }

        public async Task OnDisconnectedAsync(HubConnectionContext connection, Exception exception, Func<HubConnectionContext, Exception, Task> next)
        {
            var user = connection.User;
            if (user != null)
            {
                if (user.IsInRole("ROLE_ADMIN"))
                {
                    await _hubContext.Groups.RemoveFromGroupAsync(connection.ConnectionId, "Admins");
                }
                if (user.IsInRole("ROLE_USER"))
                {
                    await _hubContext.Groups.RemoveFromGroupAsync(connection.ConnectionId, "Users");
                }
            }
            await next(connection, exception);
        }
    }
}
