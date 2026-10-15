using EduHub.Ch02.Abstractions;
using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Observers;

/// <summary>Observer Pattern: khi sự kiện được tạo, ghi log thông tin sự kiện.</summary>
public class LogNotifier(IAppLogger logger) : IEventObserver
{
    public void OnEventCreated(Event ev, Club club, IEnumerable<Member> members)
        => logger.Log($"[Event] '{ev.Title}' created for club '{club.Name}' on {ev.Date:dd/MM/yyyy} — {members.Count()} member(s) notified");
}
