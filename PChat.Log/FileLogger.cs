using Microsoft.Extensions.Logging;

namespace PChat.Log;

public class FileLogger : ILogger
{
    private readonly string _path;
    private readonly bool _echo;
    private readonly DateTime _startDate;
    private static readonly object Lock = new();

    public FileLogger(string path, bool echo)
    {
        _path = path;
        _echo = echo;
        _startDate = DateTime.Now;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? e,
        Func<TState, Exception?, string>? formatter)
    {
        if (!IsEnabled(logLevel)) return;

        var fMessage = "";
        if (formatter != null)
            fMessage = formatter(state, e);

        var fullFilePath = Path.Combine(_path, $"{_startDate:yyyy-MM-dd}.log");
        var eMessage = "";
        lock (Lock)
        {
            var n = Environment.NewLine;
            if (e != null)
                eMessage = $"{n}{e.GetType()}: {e.Message}{n}{e.StackTrace}{n}";

            var logMessage = $"{DateTime.Now} [{logLevel}] {fMessage}{n}{eMessage}";
            
            if (_echo)
                Console.WriteLine(logMessage);
            
            File.AppendAllText(fullFilePath, logMessage);
        }
    }
}