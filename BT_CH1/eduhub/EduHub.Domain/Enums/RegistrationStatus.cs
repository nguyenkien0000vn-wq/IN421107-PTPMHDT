namespace EduHub.Domain.Enums;

/// <summary>Vòng đời phiếu đăng ký: Pending -> Confirmed -> (CheckIn) | Cancelled.</summary>
public enum RegistrationStatus
{
    Pending,
    Confirmed,
    Cancelled
}
