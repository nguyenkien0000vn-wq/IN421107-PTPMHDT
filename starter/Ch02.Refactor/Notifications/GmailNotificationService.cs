using EduHub.Ch02.Abstractions;

namespace EduHub.Ch02.Notifications;

/// <summary>Gửi thông báo qua Gmail.</summary>
public class GmailNotificationService : INotificationService
{
    public void Send(string recipient, string message)
        => Console.WriteLine($"[Gmail] → {recipient}: {message}");
}
