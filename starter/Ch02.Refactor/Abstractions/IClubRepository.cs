using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Abstractions;

/// <summary>Abstraction cho lưu trữ Club (DIP, ISP).</summary>
public interface IClubRepository
{
    void Add(Club club);
    Club? FindById(Guid id);
    IReadOnlyList<Club> GetAll();
}
