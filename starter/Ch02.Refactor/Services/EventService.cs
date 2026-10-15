using EduHub.Ch02.Abstractions;
using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Services;

/// <summary>
/// EventService: chỉ chịu trách nhiệm tạo sự kiện (SRP).
/// Dùng Observer Pattern: sau khi tạo sự kiện, publish cho các IEventObserver đã đăng ký.
/// Không tự gửi email hay ghi log — các observer chịu trách nhiệm đó (OCP).
/// </summary>
public class EventService(
    IClubRepository clubRepo,
    IMemberRepository memberRepo,
    IAppLogger logger)
{
    private readonly List<IEventObserver> _observers = new();

    /// <summary>Đăng ký observer — có thể gọi nhiều lần để thêm nhiều observer.</summary>
    public EventService Subscribe(IEventObserver observer)
    {
        _observers.Add(observer);
        return this; // fluent API
    }

    public Event CreateEvent(Guid clubId, string title, DateTime date)
    {
        var club = clubRepo.FindById(clubId)
            ?? throw new InvalidOperationException("Club not found");

        var ev = new Event { Id = Guid.NewGuid(), ClubId = clubId, Title = title, Date = date };
        logger.Log($"Event '{title}' created for club '{club.Name}'");

        // Observer Pattern: notify tất cả observer đã đăng ký
        var members = memberRepo.GetByClubId(clubId);
        foreach (var observer in _observers)
            observer.OnEventCreated(ev, club, members);

        return ev;
    }
}
