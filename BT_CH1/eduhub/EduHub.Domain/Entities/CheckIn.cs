using EduHub.Domain.Enums;

namespace EduHub.Domain.Entities;

/// <summary>Bản ghi điểm danh tại sự kiện (mỗi phiếu tối đa một CheckIn).</summary>
public class CheckIn
{
    public Guid Id { get; private set; }
    public Guid RegistrationId { get; private set; }  // khoá ngoại -> Registration
    public DateTime CheckedInAt { get; private set; }
    public string Method { get; private set; }

    private CheckIn(Guid registrationId, string method)
    {
        Id = Guid.NewGuid();
        RegistrationId = registrationId;
        CheckedInAt = DateTime.UtcNow;
        Method = method;
    }

    public static CheckIn Create(Registration registration, string method = CheckInMethod.Manual)
    {
        ArgumentNullException.ThrowIfNull(registration);
        if (registration.Status != RegistrationStatus.Confirmed)
            throw new InvalidOperationException("Chỉ phiếu Confirmed mới được check-in");
        if (string.IsNullOrWhiteSpace(method))
            throw new ArgumentException("Phương thức check-in không được trống", nameof(method));
        return new CheckIn(registration.Id, method);
    }

    public string Summary() => $"Check-in lúc {CheckedInAt:HH:mm:ss dd/MM/yyyy} bằng {Method}";
}
