namespace EduHub.Ch02.Abstractions;

/// <summary>Factory tạo INotificationService phù hợp dựa theo địa chỉ email người nhận (Factory Pattern).</summary>
public interface INotificationServiceFactory
{
    INotificationService Create(string recipientEmail);
}
