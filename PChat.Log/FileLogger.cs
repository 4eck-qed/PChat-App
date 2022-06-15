using Microsoft.Extensions.Logging;

namespace PChat.Log;

public class FileLogger : ILogger
{
    private readonly string _path;
    private static readonly object _lock = new object();

    public FileLogger(string path)
    {
        _path = path;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == LogLevel.None;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
        Func<TState, Exception, string>? formatter)
    {
        if (!IsEnabled(logLevel) || formatter == null) return;

        var fullFilePath = Path.Combine(_path, DateTime.Now.ToString("yyyy-MM-dd") + "_log.txt");
        var exc = "";
        lock (_lock)
        {
            var n = Environment.NewLine;
            if (exception != null)
                exc = n + exception.GetType() + ": " + exception.Message + n + exception.StackTrace + n;
            File.AppendAllText(fullFilePath,
                logLevel + ": " + DateTime.Now + " " + formatter(state, exception!) + n + exc);
        }
    }
}