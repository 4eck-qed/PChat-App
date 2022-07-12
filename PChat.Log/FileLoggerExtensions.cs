using Microsoft.Extensions.Logging;

namespace PChat.Log;

public static class FileLoggerExtensions
{
    public static ILoggerFactory AddFile(this ILoggerFactory factory, string filePath, bool echo = false)
    {
        factory.AddProvider(new FileLoggerProvider(filePath, echo));
        return factory;
    }
}