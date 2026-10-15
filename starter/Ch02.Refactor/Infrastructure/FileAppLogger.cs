using EduHub.Ch02.Abstractions;

namespace EduHub.Ch02.Infrastructure;

/// <summary>Ghi log ra file văn bản — implement IAppLogger (DIP fulfilled).</summary>
public class FileAppLogger(string logFilePath) : IAppLogger
{
    public void Log(string message)
        => File.AppendAllText(logFilePath,
            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}");
}
