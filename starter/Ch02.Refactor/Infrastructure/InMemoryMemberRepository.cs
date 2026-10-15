using EduHub.Ch02.Abstractions;
using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Infrastructure;

/// <summary>Lưu Member trong bộ nhớ — implement IMemberRepository.</summary>
public class InMemoryMemberRepository : IMemberRepository
{
    private readonly List<Member> _members = new();

    public void Add(Member member) => _members.Add(member);

    public IReadOnlyList<Member> GetByClubId(Guid clubId)
        => _members.Where(m => m.ClubId == clubId).ToList().AsReadOnly();

    public int CountByClubId(Guid clubId)
        => _members.Count(m => m.ClubId == clubId);
}
