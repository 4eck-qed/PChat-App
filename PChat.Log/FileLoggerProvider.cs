using Microsoft.Extensions.Logging;

namespace PChat.Log;

public class FileLoggerProvider : ILoggerProvider
{
    private readonly string _path;
    private readonly bool _echo;

    public FileLoggerProvider(string path, bool echo)
    {
        _path = path;
        _echo = echo;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new FileLogger(_path, _echo);
    }

    public void Dispose()
    {
    }
}