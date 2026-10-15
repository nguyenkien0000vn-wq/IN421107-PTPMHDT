using EduHub.Ch02.Abstractions;
using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Reporting;

/// <summary>Strategy Pattern: xuất báo cáo ra file CSV — không cần sửa ReportService (OCP).</summary>
public class CsvReportStrategy(string filePath) : IReportStrategy
{
    public void Write(IEnumerable<Club> clubs, Func<Guid, int> getMemberCount)
    {
        var lines = new List<string> { "Id,Name,Description,MemberCount" };
        lines.AddRange(clubs.Select(c =>
            $"{c.Id},\"{c.Name}\",\"{c.Description}\",{getMemberCount(c.Id)}"));

        File.WriteAllLines(filePath, lines);
        Console.WriteLine($"[CSV] Report written → {filePath}");
    }
}
