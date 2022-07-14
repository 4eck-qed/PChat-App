using Microsoft.Extensions.Logging;

namespace PChat.Log;

public static class FileLoggerExtensions
{
    public static ILoggerFactory AddFile(this ILoggerFactory factory, string filePath, bool echo = false)
    {
        factory.AddProvider(new FileLoggerProvider(filePath, echo));
        return factory;
    }
    public static ILoggingBuilder AddFile(this ILoggingBuilder builder, string filePath, bool echo = false)
    {
        builder.AddProvider(new FileLoggerProvider(filePath, echo));
        return builder;
    }
}