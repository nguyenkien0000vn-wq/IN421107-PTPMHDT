using EduHub.Ch02.Abstractions;
using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Infrastructure;

/// <summary>
/// Decorator Pattern: bọc IClubRepository để tự động ghi log mỗi thao tác
/// mà không sửa InMemoryClubRepository hay bất kỳ implementation nào khác.
/// </summary>
public class LoggingClubRepository(IClubRepository inner, IAppLogger logger) : IClubRepository
{
    public void Add(Club club)
    {
        inner.Add(club);
        logger.Log($"[Repo] Club added: '{club.Name}' (Id={club.Id})");
    }

    public Club? FindById(Guid id)
    {
        var club = inner.FindById(id);
        logger.Log(club is null
            ? $"[Repo] FindById({id}): NOT FOUND"
            : $"[Repo] FindById({id}): found '{club.Name}'");
        return club;
    }

    public IReadOnlyList<Club> GetAll()
    {
        var clubs = inner.GetAll();
        logger.Log($"[Repo] GetAll: {clubs.Count} club(s)");
        return clubs;
    }
}
