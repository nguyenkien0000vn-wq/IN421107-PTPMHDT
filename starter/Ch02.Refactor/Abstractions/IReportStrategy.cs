using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Abstractions;

/// <summary>Strategy cho xuất báo cáo — Console, CSV, HTML… đều implement interface này (Strategy Pattern + OCP).</summary>
public interface IReportStrategy
{
    void Write(IEnumerable<Club> clubs, Func<Guid, int> getMemberCount);
}
