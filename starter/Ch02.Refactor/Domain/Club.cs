namespace EduHub.Ch02.Domain;

/// <summary>Câu lạc bộ. Quy tắc nghiệp vụ nằm ở đây, không nằm trong ClubManager hay Service.</summary>
public class Club
{
    public const int MaxMembers = 50;
    public const int MaxNameLength = 100;

    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    /// <summary>Factory method: validate và tạo Club mới. Nghiệp vụ nằm ở Domain, không ở Service.</summary>
    public static Club Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Club name is required.", nameof(name));
        if (name.Length > MaxNameLength)
            throw new ArgumentException($"Club name must not exceed {MaxNameLength} characters.", nameof(name));

        return new Club { Id = Guid.NewGuid(), Name = name, Description = description };
    }

    /// <summary>Trả về true khi CLB còn chỗ (số thành viên hiện tại nhỏ hơn tối đa).</summary>
    public bool HasRoom(int currentMemberCount)
        => currentMemberCount < MaxMembers;
}
