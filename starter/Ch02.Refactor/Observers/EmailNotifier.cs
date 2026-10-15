using EduHub.Ch02.Abstractions;
using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Observers;

/// <summary>
/// Observer Pattern: khi sự kiện được tạo, gửi email đến từng thành viên trong CLB.
/// Dùng INotificationServiceFactory để chọn đúng kênh gửi (tích hợp Factory + Observer).
/// </summary>
public class EmailNotifier(INotificationServiceFactory factory) : IEventObserver
{
    public void OnEventCreated(Event ev, Club club, IEnumerable<Member> members)
    {
        foreach (var member in members)
        {
            var notifier = factory.Create(member.Email);
            notifier.Send(member.Email,
                $"[{club.Name}] Sự kiện mới: '{ev.Title}' vào {ev.Date:dd/MM/yyyy}");
        }
    }
}
