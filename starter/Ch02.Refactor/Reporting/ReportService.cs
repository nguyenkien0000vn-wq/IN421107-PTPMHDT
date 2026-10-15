using EduHub.Ch02.Abstractions;

namespace EduHub.Ch02.Reporting;

/// <summary>
/// ReportService nhận IReportStrategy qua constructor (Strategy Pattern + DIP).
/// Có thể đổi strategy lúc runtime mà không sửa class này.
/// </summary>
public class ReportService(IClubRepository clubRepo, IMemberRepository memberRepo, IAppLogger logger)
{
    public void Generate(IReportStrategy strategy)
    {
        var clubs = clubRepo.GetAll();
        strategy.Write(clubs, id => memberRepo.CountByClubId(id));
        logger.Log($"Report generated — {clubs.Count} club(s)");
    }
}
