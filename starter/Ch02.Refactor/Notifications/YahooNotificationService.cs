using EduHub.Ch02.Abstractions;

namespace EduHub.Ch02.Notifications;

/// <summary>Gửi thông báo qua Yahoo Mail.</summary>
public class YahooNotificationService : INotificationService
{
    public void Send(string recipient, string message)
        => Console.WriteLine($"[Yahoo] → {recipient}: {message}");
}
