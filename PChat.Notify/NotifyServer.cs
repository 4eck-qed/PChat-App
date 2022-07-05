using Grpc.Core;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.Extensions.Options;
using PChat.Log;

namespace PChat.Notify;

using Services;

public class NotifyServer
{
    private readonly Server _server;

    public NotifyServer()
    {
        var logger = new LoggerFactory()
            .AddFile("./data/notify.log")
            .CreateLogger<NotifyService>();
        _server = new Server()
        {
            Services = {Pchat.Notify.BindService(new NotifyService(logger))},
            Ports = {new ServerPort("localhost", 50052, ServerCredentials.Insecure)}
        };
    }

    public void Start(string[] args)
    {
        _server.Start();

        // var builder = WebApplication.CreateBuilder(args);
        // // Additional configuration is required to successfully run gRPC on macOS.
        // // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        //
        //
        // // Add services to the container.
        // builder.Services.AddGrpc();
        // var app = builder.Build();
        //
        // // Configure the HTTP request pipeline.
        // app.MapGrpcService<NotifyService>();
        // app.MapGet("/", () => "");
        // app.Run();
    }

    public async Task ShutdownAsync()
    {
        await _server.ShutdownAsync();
    }
}