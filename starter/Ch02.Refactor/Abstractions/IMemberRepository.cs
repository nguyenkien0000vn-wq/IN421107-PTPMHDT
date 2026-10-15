using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Abstractions;

/// <summary>Abstraction cho lưu trữ Member (DIP, ISP).</summary>
public interface IMemberRepository
{
    void Add(Member member);
    IReadOnlyList<Member> GetByClubId(Guid clubId);
    int CountByClubId(Guid clubId);
}
