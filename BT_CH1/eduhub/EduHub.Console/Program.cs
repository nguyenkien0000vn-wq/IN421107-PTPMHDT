using EduHub.ConsoleApp;
using EduHub.Domain.Entities;
using EduHub.Domain.Enums;

Console.OutputEncoding = System.Text.Encoding.UTF8;

void Step(string text)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"\n=== {text} ===");
    Console.ResetColor();
}

// ---------- Bước 1: Tạo CLB ----------
Step("1. Tạo CLB");
var club = Club.Create("CLB Lập trình", "Sinh hoạt về lập trình và công nghệ");
Console.WriteLine($"Đã tạo: {club}");

// ---------- Bước 2: Thêm 3 thành viên ----------
Step("2. Thêm 3 thành viên");
var members = new List<Member>
{
    Member.Create("Nguyễn Văn An", "an@dthu.edu.vn", "DH24001"),
    Member.Create("Trần Thị Bình", "binh@dthu.edu.vn", "DH24002"),
    Member.Create("Lê Minh Cường", "cuong@dthu.edu.vn", "DH24003"),
};
foreach (var m in members) club.AddMember(m);
Console.WriteLine($"CLB hiện có {club.MemberCount()} thành viên");

// ---------- Bước 3: Tạo sự kiện (sức chứa 2 để thử luật hết chỗ) ----------
Step("3. Tạo sự kiện");
var evt = Event.Create(club.Id, "Workshop C# & .NET 10", DateTime.Today.AddDays(7), capacity: 2);
Console.WriteLine($"Sự kiện: {evt} | sức chứa {evt.Capacity} | online: {evt.IsOnline}");

// ---------- Bước 4: Đăng ký (Pending) ----------
Step("4. Đăng ký (Pending)");
var registrations = new List<Registration>();
int Active() => registrations.Count(r => r.Status != RegistrationStatus.Cancelled);

foreach (var m in members)
{
    try
    {
        var reg = Registration.Create(m, evt, Active());
        registrations.Add(reg);
        Console.WriteLine($"{m.Name}: {reg.Status}");
    }
    catch (InvalidOperationException ex)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{m.Name}: BỊ TỪ CHỐI - {ex.Message}");
        Console.ResetColor();
    }
}

// ---------- Bước 5: Xác nhận (Confirmed) ----------
Step("5. Xác nhận (Confirmed)");
registrations.ForEach(r => r.Confirm());
registrations.ForEach(r => Console.WriteLine($"Phiếu {r.Id.ToString()[..8]}: {r.Status}"));

// ---------- Bước 6: Check-in qua QR (chỉ người thứ nhất) ----------
Step("6. Check-in qua QR");
var checkIns = new List<CheckIn>();
var checkIn = CheckIn.Create(registrations[0], CheckInMethod.QR);
checkIns.Add(checkIn);
Console.WriteLine(checkIn.Summary());

// ---------- Bước 7: Báo cáo tham dự ----------
Step("7. Báo cáo tham dự");
Console.WriteLine($"Đăng ký: {Active()}/{evt.Capacity} | Đã check-in: {checkIns.Count}");
foreach (var r in registrations)
{
    var name = members.First(m => m.Id == r.MemberId).Name;
    var done = checkIns.Any(c => c.RegistrationId == r.Id);
    Console.WriteLine($" - {name,-18} {(done ? "đã check-in" : "chưa check-in")}");
}

// ---------- Bước 8: Thử vi phạm luật nghiệp vụ ----------
Step("8. Thử vi phạm luật nghiệp vụ");
var validationTests = new List<ValidationTestResult>();

void Try(string title, Action action)
{
    try 
    { 
        action(); 
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[{title}] KHÔNG báo lỗi (sai!)"); 
        Console.ResetColor();
        validationTests.Add(new ValidationTestResult(title, "Ném ngoại lệ", "LỖI: Không chặn được vi phạm", false));
    }
    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"[{title}] bị chặn: {ex.Message}");
        Console.ResetColor();
        validationTests.Add(new ValidationTestResult(title, "Ném ngoại lệ", $"bị chặn: {ex.Message}", true));
    }
}

Try("Email sai", () => Member.Create("Abc", "abc", "DH24999"));
Try("Thêm trùng thành viên", () => club.AddMember(members[0]));
Try("Capacity = 0", () => Event.Create(club.Id, "Sự kiện lỗi", DateTime.Today, 0));
Try("Tiêu đề trống", () => Event.Create(club.Id, "  ", DateTime.Today, 10));
Try("Hết chỗ", () => Registration.Create(members[2], evt, Active()));
registrations[1].Cancel();
Try("Xác nhận phiếu đã huỷ", () => registrations[1].Confirm());
Try("Check-in phiếu đã huỷ", () => CheckIn.Create(registrations[1]));
members[1].Deactivate();
Console.WriteLine($"[Ngừng thành viên] IsActive() = {members[1].IsActive()}; lịch sử đăng ký vẫn còn: {registrations.Any(r => r.MemberId == members[1].Id)}");
validationTests.Add(new ValidationTestResult("Ngừng thành viên & bảo lưu lịch sử", "Deactivate", $"IsActive() = {members[1].IsActive()}; lịch sử vẫn còn: {registrations.Any(r => r.MemberId == members[1].Id)}", true));
Try("Đăng ký khi đã ngừng", () => Registration.Create(members[1], evt, 0));

// ---------- Tự động Xuất Báo Cáo HTML ----------
string FindDocsDirectory()
{
    var current = new DirectoryInfo(Directory.GetCurrentDirectory());
    while (current != null)
    {
        var docs = Path.Combine(current.FullName, "docs");
        if (Directory.Exists(docs)) return docs;
        current = current.Parent;
    }
    var fallback = Path.Combine(Directory.GetCurrentDirectory(), "docs");
    Directory.CreateDirectory(fallback);
    return fallback;
}

try
{
    string docsDir = FindDocsDirectory();
    string htmlPath = Path.Combine(docsDir, "demo.html");
    HtmlReportExporter.Export(club, members, evt, registrations, checkIns, validationTests, htmlPath);

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("\n" + new string('=', 64));
    Console.WriteLine($"✨ ĐÃ XUẤT BÁO CÁO HTML THÀNH CÔNG!");
    Console.WriteLine($"📁 Đường dẫn: {Path.GetFullPath(htmlPath)}");
    Console.WriteLine("🌐 Bạn có thể nhấp đúp hoặc mở file trên trình duyệt để xem!");
    Console.WriteLine(new string('=', 64));
    Console.ResetColor();
}
catch (Exception ex)
{
    Console.WriteLine($"[Cảnh báo xuất HTML]: {ex.Message}");
}
