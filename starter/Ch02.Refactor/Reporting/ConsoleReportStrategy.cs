using EduHub.Ch02.Abstractions;
using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Reporting;

/// <summary>Strategy Pattern: xuất báo cáo ra Console.</summary>
public class ConsoleReportStrategy : IReportStrategy
{
    public void Write(IEnumerable<Club> clubs, Func<Guid, int> getMemberCount)
    {
        Console.WriteLine("╔══════════════════════════════════╗");
        Console.WriteLine("║         CLUB REPORT              ║");
        Console.WriteLine("╚══════════════════════════════════╝");
        foreach (var club in clubs)
            Console.WriteLine($"  • {club.Name,-25} | Members: {getMemberCount(club.Id)}");
    }
}
