namespace EduHub.Domain.Entities;

/// <summary>Sự kiện do một CLB tổ chức (workshop, seminar, hackathon...).</summary>
public class Event
{
    public Guid Id { get; private set; }
    public Guid ClubId { get; private set; }          // khoá ngoại -> Club
    public string Title { get; private set; }
    public DateTime Date { get; private set; }
    public int Capacity { get; private set; }         // sức chứa
    public bool IsOnline { get; private set; }

    private Event(Guid clubId, string title, DateTime date, int capacity, bool isOnline)
    {
        Id = Guid.NewGuid();
        ClubId = clubId;
        Title = title;
        Date = date;
        Capacity = capacity;
        IsOnline = isOnline;
    }

    public static Event Create(Guid clubId, string title, DateTime date, int capacity, bool isOnline = false)
    {
        if (clubId == Guid.Empty)
            throw new ArgumentException("Sự kiện phải thuộc về một CLB", nameof(clubId));
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Tiêu đề không được để trống", nameof(title));
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Sức chứa phải lớn hơn 0");
        return new Event(clubId, title.Trim(), date, capacity, isOnline);
    }

    /// <summary>Còn chỗ nếu số người đã đăng ký chưa đạt sức chứa.</summary>
    public bool HasCapacity(int registeredCount) => registeredCount < Capacity;

    public override string ToString() => Title;
}
