namespace EduHub.Ch02.Abstractions;

/// <summary>Abstraction cho gửi thông báo — mỗi kênh (Gmail, Yahoo, SMS…) implement riêng (OCP).</summary>
public interface INotificationService
{
    void Send(string recipient, string message);
}
