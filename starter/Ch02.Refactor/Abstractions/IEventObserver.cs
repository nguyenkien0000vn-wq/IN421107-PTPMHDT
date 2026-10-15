using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Abstractions;

/// <summary>Observer nhận thông báo khi một sự kiện CLB được tạo (Observer Pattern).</summary>
public interface IEventObserver
{
    void OnEventCreated(Event ev, Club club, IEnumerable<Member> members);
}
