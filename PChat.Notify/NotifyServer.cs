using Grpc.Core;
using PChat.Log;

namespace PChat.Notify;

using Services;

public class NotifyServer
{
    private readonly ILogger _logger;
    private readonly Server _server;

    public NotifyServer()
    {
        var logger = new LoggerFactory()
            .AddFile($"./data/{nameof(NotifyService)}", true)
            .CreateLogger<NotifyService>();
        _logger = logger;
        _server = new Server()
        {
            Services = {Pchat.Notify.BindService(new NotifyService(logger))},
            Ports = {new ServerPort("localhost", 50052, ServerCredentials.Insecure)}
        };
    }

    public void Start(string[]? args = null)
    {
        _logger.LogInformation("NotifyServer started");
        _server.Start();
    }

    public async Task ShutdownAsync()
    {
        await _server.ShutdownAsync();
    }
}