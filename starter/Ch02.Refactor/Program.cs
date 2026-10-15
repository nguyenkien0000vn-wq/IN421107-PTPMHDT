using EduHub.Ch02.Infrastructure;
using EduHub.Ch02.Notifications;
using EduHub.Ch02.Observers;
using EduHub.Ch02.Reporting;
using EduHub.Ch02.Services;

// ═══════════════════════════════════════════════════════════════════════════════
//  EduHub Ch02 — Composition Root (wiring tất cả dependencies lại với nhau)
//  Tất cả phụ thuộc được inject qua constructor — không có "new" bên trong service.
// ═══════════════════════════════════════════════════════════════════════════════

// ── Infrastructure ──────────────────────────────────────────────────────────────
var logger       = new FileAppLogger("club-log.txt");       // IAppLogger
var rawClubRepo  = new InMemoryClubRepository();            // IClubRepository (raw)
var clubRepo     = new LoggingClubRepository(rawClubRepo, logger); // Decorator: tự ghi log
var memberRepo   = new InMemoryMemberRepository();          // IMemberRepository

// ── Factory (Notifications) ─────────────────────────────────────────────────────
var notifFactory = new NotificationServiceFactory();        // INotificationServiceFactory

// ── Services ────────────────────────────────────────────────────────────────────
var clubService = new ClubService(clubRepo, memberRepo, notifFactory, logger);

var eventService = new EventService(clubRepo, memberRepo, logger)
    .Subscribe(new EmailNotifier(notifFactory))             // Observer: gửi email
    .Subscribe(new LogNotifier(logger));                    // Observer: ghi log

var reportService = new ReportService(clubRepo, memberRepo, logger);

// ═══════════════════════════════════════════════════════════════════════════════
//  Demo chạy toàn bộ flow
// ═══════════════════════════════════════════════════════════════════════════════
Console.WriteLine("=== TẠO CLB ===");
var club = clubService.CreateClub("Chess Club", "CLB cờ vua trường ĐHKTKT", "president@gmail.com");

Console.WriteLine("\n=== THÊM THÀNH VIÊN ===");
clubService.AddMemberToClub(club.Id, "Nguyễn Văn A", "a@gmail.com");
clubService.AddMemberToClub(club.Id, "Trần Thị B",   "b@yahoo.com");
clubService.AddMemberToClub(club.Id, "Lê Văn C",     "c@student.edu.vn");

Console.WriteLine("\n=== TẠO SỰ KIỆN (Observer notify) ===");
eventService.CreateEvent(club.Id, "Workshop SOLID", new DateTime(2026, 11, 20));

Console.WriteLine("\n=== BÁO CÁO CONSOLE (Strategy) ===");
reportService.Generate(new ConsoleReportStrategy());

Console.WriteLine("\n=== BÁO CÁO CSV (Strategy) ===");
reportService.Generate(new CsvReportStrategy("clubs-report.csv"));
