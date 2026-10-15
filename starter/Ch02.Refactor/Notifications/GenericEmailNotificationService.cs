using EduHub.Ch02.Abstractions;

namespace EduHub.Ch02.Notifications;

/// <summary>Fallback: gửi email thông thường cho các domain khác.</summary>
public class GenericEmailNotificationService : INotificationService
{
    public void Send(string recipient, string message)
        => Console.WriteLine($"[Email] → {recipient}: {message}");
}
