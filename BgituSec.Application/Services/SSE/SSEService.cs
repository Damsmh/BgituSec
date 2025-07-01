using BgituSec.Application.Features.Breakdowns.Commands;
using MediatR;
using System.Collections.Concurrent;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Channels;

namespace BgituSec.Application.Services.SSE
{
    public class SSEService : ISSEService, INotificationHandler<BreakdownResponseNotification>
    {
        private readonly ConcurrentDictionary<string, ChannelWriter<string>> _clients = new();

        public async Task RegisterClientAsync(string clientId, ChannelWriter<string> writer, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Клиент {clientId} зарегистрирован");
            _clients.TryAdd(clientId, writer);

            try
            {
                Console.WriteLine($"Отправка тестового сообщения клиенту {clientId}");
                await writer.WriteAsync("data: {\"message\": \"connected\"}\n\n", cancellationToken);

                while (!cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine($"Отправка пинга клиенту {clientId}");
                    await writer.WriteAsync(": ping\n\n", cancellationToken);
                    await Task.Delay(TimeSpan.FromSeconds(15), cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"Клиент {clientId} отключился (OperationCanceledException)");
                _clients.TryRemove(clientId, out _);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка для клиента {clientId}: {ex.Message}");
                _clients.TryRemove(clientId, out _);
            }
        }

        public async Task Handle(BreakdownResponseNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Отправка уведомления. Количество клиентов: {_clients.Count}");
            var data = JsonSerializer.Serialize(notification.Breakdowns, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            var message = $"data: {data}\n\n";
            var failedClients = new List<string>();

            foreach (var client in _clients)
            {
                try
                {
                    Console.WriteLine($"Отправка клиенту {client.Key}");
                    if (!client.Value.TryWrite(message))
                    {
                        Console.WriteLine($"Не удалось записать клиенту {client.Key}");
                        failedClients.Add(client.Key);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка отправки клиенту {client.Key}: {ex.Message}");
                    failedClients.Add(client.Key);
                }
            }

            foreach (var clientId in failedClients)
            {
                Console.WriteLine($"Удаление клиента {clientId}");
                _clients.TryRemove(clientId, out _);
            }
        }
    }
}