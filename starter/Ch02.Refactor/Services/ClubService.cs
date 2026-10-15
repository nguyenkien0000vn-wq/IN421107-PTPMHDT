using EduHub.Ch02.Abstractions;
using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Services;

/// <summary>
/// ClubService: chỉ chịu trách nhiệm nghiệp vụ tạo CLB và thêm thành viên (SRP).
/// Mọi phụ thuộc đi qua interface, nhận bằng constructor (DIP).
/// </summary>
public class ClubService(
    IClubRepository clubRepo,
    IMemberRepository memberRepo,
    INotificationServiceFactory notifFactory,
    IAppLogger logger)
{
    public Club CreateClub(string name, string description, string presidentEmail)
    {
        // Domain validation nằm trong Club.Create (không phải ở đây)
        var club = Club.Create(name, description);
        clubRepo.Add(club);
        logger.Log($"Club created: '{name}'");

        // Factory chọn đúng kênh thông báo — không cần if/else ở đây (OCP)
        notifFactory.Create(presidentEmail)
                    .Send(presidentEmail, $"CLB '{name}' đã được tạo thành công!");

        return club;
    }

    public Member AddMemberToClub(Guid clubId, string memberName, string email)
    {
        var club = clubRepo.FindById(clubId)
            ?? throw new InvalidOperationException("Club not found");

        // Business rule nằm trong Domain (Club.HasRoom) — không hardcode 50 ở đây
        if (!club.HasRoom(memberRepo.CountByClubId(clubId)))
            throw new InvalidOperationException($"Club is full (max {Club.MaxMembers} members)");

        var member = new Member { Id = Guid.NewGuid(), Name = memberName, Email = email, ClubId = clubId };
        memberRepo.Add(member);
        logger.Log($"Member '{memberName}' joined '{club.Name}'");

        notifFactory.Create(email)
                    .Send(email, $"Chào mừng bạn gia nhập CLB '{club.Name}'!");

        return member;
    }
}
