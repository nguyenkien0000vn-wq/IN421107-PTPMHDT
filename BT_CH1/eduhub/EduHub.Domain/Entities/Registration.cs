using EduHub.Domain.Enums;

namespace EduHub.Domain.Entities;

/// <summary>Phiếu đăng ký tham dự sự kiện của một thành viên.</summary>
public class Registration
{
    public Guid Id { get; private set; }
    public Guid MemberId { get; private set; }        // khoá ngoại -> Member
    public Guid EventId { get; private set; }         // khoá ngoại -> Event
    public DateTime RegisteredAt { get; private set; }
    public RegistrationStatus Status { get; private set; }

    private Registration(Guid memberId, Guid eventId)
    {
        Id = Guid.NewGuid();
        MemberId = memberId;
        EventId = eventId;
        RegisteredAt = DateTime.UtcNow;
        Status = RegistrationStatus.Pending;
    }

    /// <param name="currentRegisteredCount">Số phiếu hiện có (chưa huỷ) của sự kiện.</param>
    public static Registration Create(Member member, Event evt, int currentRegisteredCount)
    {
        ArgumentNullException.ThrowIfNull(member);
        ArgumentNullException.ThrowIfNull(evt);
        if (!member.IsActive())
            throw new InvalidOperationException($"{member} đã ngừng hoạt động, không thể đăng ký");
        if (!evt.HasCapacity(currentRegisteredCount))
            throw new InvalidOperationException($"Sự kiện \"{evt}\" đã hết chỗ");
        return new Registration(member.Id, evt.Id);
    }

    public void Confirm()
    {
        if (Status != RegistrationStatus.Pending)
            throw new InvalidOperationException($"Không thể xác nhận phiếu đang ở trạng thái {Status}");
        Status = RegistrationStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status == RegistrationStatus.Cancelled)
            throw new InvalidOperationException("Phiếu đã bị huỷ trước đó");
        Status = RegistrationStatus.Cancelled;
    }
}
