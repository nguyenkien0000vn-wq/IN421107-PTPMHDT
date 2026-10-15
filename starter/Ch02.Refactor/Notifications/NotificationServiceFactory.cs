using EduHub.Ch02.Abstractions;

namespace EduHub.Ch02.Notifications;

/// <summary>
/// Factory Pattern: tạo INotificationService phù hợp dựa trên domain của email người nhận.
/// Muốn thêm kênh mới (SMS, Zalo…) chỉ cần thêm class mới + một nhánh ở đây — không sửa service nào khác (OCP).
/// </summary>
public class NotificationServiceFactory : INotificationServiceFactory
{
    public INotificationService Create(string recipientEmail)
    {
        if (recipientEmail.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase))
            return new GmailNotificationService();

        if (recipientEmail.EndsWith("@yahoo.com", StringComparison.OrdinalIgnoreCase))
            return new YahooNotificationService();

        return new GenericEmailNotificationService();
    }
}
