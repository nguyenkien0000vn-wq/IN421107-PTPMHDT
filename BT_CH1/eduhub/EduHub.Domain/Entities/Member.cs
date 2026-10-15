using EduHub.Domain.Enums;

namespace EduHub.Domain.Entities;

/// <summary>Thành viên (sinh viên) của hệ thống EduHub.</summary>
public class Member
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string StudentCode { get; private set; }   // mã số sinh viên
    public DateTime JoinedAt { get; private set; }
    public MemberStatus Status { get; private set; }

    // Constructor private: bắt buộc tạo qua Create()
    private Member(string name, string email, string studentCode)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        StudentCode = studentCode;
        JoinedAt = DateTime.UtcNow;
        Status = MemberStatus.Active;
    }

    public static Member Create(string name, string email, string studentCode)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên không được để trống", nameof(name));
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException("Email không hợp lệ", nameof(email));
        if (string.IsNullOrWhiteSpace(studentCode))
            throw new ArgumentException("Mã số sinh viên không được để trống", nameof(studentCode));

        return new Member(name.Trim(), email.Trim().ToLowerInvariant(), studentCode.Trim());
    }

    public bool IsActive() => Status == MemberStatus.Active;

    public void Deactivate()
    {
        if (Status == MemberStatus.Inactive)
            throw new InvalidOperationException("Thành viên đã ngừng hoạt động");
        Status = MemberStatus.Inactive;
    }

    public override string ToString() => $"{Name} ({StudentCode})";
}
