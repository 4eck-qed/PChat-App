using Grpc.Core;
using PChat.Config;
using PChat.Log;

namespace PChat.Notify;

using Services;

public class NotifyServer
{
    private readonly ILogger _logger;
    private readonly Server _server;

    public NotifyServer()
    {
        _logger = LoggerFactory.Create(o =>
        {
            o.AddConsole();
            o.AddFile(Global.DataDir);
        }).CreateLogger<NotifyServer>();

        _server = new Server()
        {
            Services = {Pchat.Notify.BindService(new NotifyService(_logger))},
            Ports = {new ServerPort("localhost", Global.NotifyPort, ServerCredentials.Insecure)}
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