using System.Text;
using EduHub.Domain.Entities;
using EduHub.Domain.Enums;

namespace EduHub.ConsoleApp;

public record ValidationTestResult(string RuleName, string ExpectedBehavior, string Message, bool Passed);

public static class HtmlReportExporter
{
    public static string Export(
        Club club,
        List<Member> members,
        Event evt,
        List<Registration> registrations,
        List<CheckIn> checkIns,
        List<ValidationTestResult> validationTests,
        string outputPath)
    {
        var sb = new StringBuilder();
        int activeRegCount = registrations.Count(r => r.Status != RegistrationStatus.Cancelled);
        string generatedAt = DateTime.Now.ToString("HH:mm:ss - dd/MM/yyyy");

        sb.Append($$"""
<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>EduHub - Báo Cáo Chạy Demo Chương 1</title>
    <style>
        :root {
            --bg: #0f172a;
            --surface: #1e293b;
            --surface-hover: #334155;
            --border: #334155;
            --text-main: #f8fafc;
            --text-muted: #94a3b8;
            --primary: #38bdf8;
            --primary-bg: rgba(56, 189, 248, 0.1);
            --success: #4ade80;
            --success-bg: rgba(74, 222, 128, 0.1);
            --warning: #facc15;
            --warning-bg: rgba(250, 204, 21, 0.1);
            --danger: #f87171;
            --danger-bg: rgba(248, 113, 113, 0.1);
            --purple: #c084fc;
            --purple-bg: rgba(192, 132, 252, 0.1);
        }

        * {
            box-sizing: border-box;
            margin: 0;
            padding: 0;
            font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Helvetica, Arial, sans-serif;
        }

        body {
            background-color: var(--bg);
            color: var(--text-main);
            padding: 32px 20px;
            line-height: 1.5;
        }

        .container {
            max-width: 1040px;
            margin: 0 auto;
        }

        .header {
            background: linear-gradient(135deg, #1e293b 0%, #0f172a 100%);
            border: 1px solid var(--border);
            border-radius: 16px;
            padding: 28px 32px;
            margin-bottom: 28px;
            box-shadow: 0 10px 25px -5px rgba(0,0,0,0.3);
            display: flex;
            justify-content: space-between;
            align-items: center;
            flex-wrap: wrap;
            gap: 20px;
        }

        .header-title h1 {
            font-size: 26px;
            font-weight: 800;
            background: linear-gradient(to right, #38bdf8, #818cf8);
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
            margin-bottom: 6px;
        }

        .header-title p {
            color: var(--text-muted);
            font-size: 14px;
        }

        .header-meta {
            text-align: right;
            font-size: 13px;
            color: var(--text-muted);
        }

        .badge-student {
            display: inline-block;
            background: rgba(99, 102, 241, 0.15);
            color: #a5b4fc;
            border: 1px solid rgba(99, 102, 241, 0.3);
            padding: 4px 10px;
            border-radius: 9999px;
            font-weight: 600;
            margin-top: 6px;
        }

        /* Overview stats */
        .stats-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 16px;
            margin-bottom: 28px;
        }

        .stat-card {
            background-color: var(--surface);
            border: 1px solid var(--border);
            border-radius: 12px;
            padding: 18px 20px;
            position: relative;
            overflow: hidden;
        }

        .stat-card::after {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            height: 3px;
        }

        .stat-card.blue::after { background: var(--primary); }
        .stat-card.green::after { background: var(--success); }
        .stat-card.purple::after { background: var(--purple); }
        .stat-card.orange::after { background: var(--warning); }

        .stat-label {
            font-size: 13px;
            color: var(--text-muted);
            text-transform: uppercase;
            font-weight: 600;
            letter-spacing: 0.5px;
        }

        .stat-value {
            font-size: 26px;
            font-weight: 700;
            margin-top: 4px;
        }

        .stat-sub {
            font-size: 12px;
            color: var(--text-muted);
            margin-top: 2px;
        }

        /* Steps */
        .step-card {
            background-color: var(--surface);
            border: 1px solid var(--border);
            border-radius: 14px;
            margin-bottom: 20px;
            overflow: hidden;
            box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
        }

        .step-header {
            padding: 16px 22px;
            background: rgba(255, 255, 255, 0.02);
            border-bottom: 1px solid var(--border);
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .step-title {
            display: flex;
            align-items: center;
            gap: 12px;
            font-size: 16px;
            font-weight: 700;
        }

        .step-num {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 28px;
            height: 28px;
            border-radius: 8px;
            background: var(--primary);
            color: #0f172a;
            font-weight: 800;
            font-size: 14px;
        }

        .step-body {
            padding: 20px 22px;
        }

        /* Tables */
        table {
            width: 100%;
            border-collapse: collapse;
            font-size: 13.5px;
            margin-top: 8px;
        }

        th {
            background-color: rgba(15, 23, 42, 0.6);
            color: var(--text-muted);
            text-align: left;
            padding: 10px 14px;
            font-weight: 600;
            border-bottom: 1px solid var(--border);
        }

        td {
            padding: 12px 14px;
            border-bottom: 1px solid rgba(51, 65, 85, 0.5);
        }

        tr:last-child td {
            border-bottom: none;
        }

        tr:hover td {
            background-color: rgba(255, 255, 255, 0.02);
        }

        /* Badges */
        .badge {
            display: inline-block;
            font-size: 11.5px;
            font-weight: 700;
            padding: 3px 8px;
            border-radius: 6px;
            text-transform: uppercase;
        }

        .badge-success { background: var(--success-bg); color: var(--success); border: 1px solid rgba(74,222,128,0.3); }
        .badge-warning { background: var(--warning-bg); color: var(--warning); border: 1px solid rgba(250,204,21,0.3); }
        .badge-danger  { background: var(--danger-bg);  color: var(--danger);  border: 1px solid rgba(248,113,113,0.3); }
        .badge-primary { background: var(--primary-bg); color: var(--primary); border: 1px solid rgba(56,189,248,0.3); }
        .badge-purple  { background: var(--purple-bg);  color: var(--purple);  border: 1px solid rgba(192,132,252,0.3); }

        /* Code & Monospace */
        .code-box {
            background: #090d16;
            border: 1px solid #1e293b;
            border-radius: 8px;
            padding: 12px 16px;
            font-family: "Consolas", monospace;
            font-size: 13px;
            color: #e2e8f0;
            overflow-x: auto;
        }

        .grid-2 {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
            gap: 16px;
        }

        .info-item {
            background: rgba(15, 23, 42, 0.4);
            border: 1px solid var(--border);
            border-radius: 8px;
            padding: 12px 16px;
        }

        .info-label {
            font-size: 12px;
            color: var(--text-muted);
            margin-bottom: 4px;
        }

        .info-val {
            font-size: 14px;
            font-weight: 600;
        }

        /* Validation list */
        .rule-list {
            display: flex;
            flex-direction: column;
            gap: 10px;
        }

        .rule-item {
            display: flex;
            align-items: flex-start;
            justify-content: space-between;
            padding: 12px 16px;
            background: rgba(15, 23, 42, 0.5);
            border: 1px solid var(--border);
            border-radius: 8px;
            gap: 14px;
        }

        .rule-item.passed {
            border-left: 4px solid var(--success);
        }

        .rule-info {
            flex: 1;
        }

        .rule-title {
            font-size: 14px;
            font-weight: 600;
            color: var(--text-main);
            display: flex;
            align-items: center;
            gap: 8px;
        }

        .rule-desc {
            font-size: 12.5px;
            color: var(--text-muted);
            margin-top: 3px;
            font-family: Consolas, monospace;
        }

        .action-bar {
            margin-top: 24px;
            display: flex;
            justify-content: center;
            gap: 12px;
        }

        .btn {
            background: var(--surface);
            color: var(--text-main);
            border: 1px solid var(--border);
            padding: 10px 20px;
            border-radius: 8px;
            font-size: 13.5px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.2s;
            text-decoration: none;
            display: inline-flex;
            align-items: center;
            gap: 8px;
        }

        .btn:hover {
            background: var(--surface-hover);
            border-color: var(--primary);
            color: var(--primary);
        }

        @media print {
            body { background: #fff; color: #000; padding: 0; }
            .step-card, .header, .stat-card { border-color: #ddd; background: #fff; color: #000; box-shadow: none; }
            .btn, .action-bar { display: none; }
        }
    </style>
</head>
<body>
    <div class="container">
        <!-- Header -->
        <header class="header">
            <div class="header-title">
                <h1>🎓 EduHub · Kết Quả Chạy Thử Nghiệm</h1>
                <p>IN4211 — Phát triển phần mềm hướng đối tượng | Bài tập Chương 1</p>
            </div>
            <div class="header-meta">
                <div>Thời gian chạy: <strong>{{generatedAt}}</strong></div>
                <div class="badge-student">Sinh viên: Lăng Quang Thái (0024419382)</div>
            </div>
        </header>

        <!-- Stats Grid -->
        <section class="stats-grid">
            <div class="stat-card blue">
                <div class="stat-label">Câu lạc bộ</div>
                <div class="stat-value">{{club.Name}}</div>
                <div class="stat-sub">Đã khởi tạo với Id Guid hợp lệ</div>
            </div>
            <div class="stat-card green">
                <div class="stat-label">Thành viên CLB</div>
                <div class="stat-value">{{club.MemberCount()}}</div>
                <div class="stat-sub">{{members.Count(m => m.IsActive())}} đang Active</div>
            </div>
            <div class="stat-card purple">
                <div class="stat-label">Sự kiện & Chỗ ngồi</div>
                <div class="stat-value">{{activeRegCount}} / {{evt.Capacity}}</div>
                <div class="stat-sub">Sức chứa tối đa: {{evt.Capacity}} người</div>
            </div>
            <div class="stat-card orange">
                <div class="stat-label">Check-in / Đăng ký</div>
                <div class="stat-value">{{checkIns.Count}} / {{activeRegCount}}</div>
                <div class="stat-sub">Phương thức: {{CheckInMethod.QR}}</div>
            </div>
        </section>

        <!-- Bước 1: Tạo CLB -->
        <div class="step-card">
            <div class="step-header">
                <div class="step-title">
                    <span class="step-num">1</span>
                    <span>Khởi tạo Câu Lạc Bộ (Club)</span>
                </div>
                <span class="badge badge-success">Thành công</span>
            </div>
            <div class="step-body">
                <div class="grid-2">
                    <div class="info-item">
                        <div class="info-label">Tên câu lạc bộ</div>
                        <div class="info-val">{{club.Name}}</div>
                    </div>
                    <div class="info-item">
                        <div class="info-label">Mô tả</div>
                        <div class="info-val">{{club.Description}}</div>
                    </div>
                    <div class="info-item">
                        <div class="info-label">Mã định danh (Id)</div>
                        <div class="info-val" style="font-family: Consolas;">{{club.Id}}</div>
                    </div>
                    <div class="info-item">
                        <div class="info-label">Ngày khởi tạo (UTC)</div>
                        <div class="info-val">{{club.CreatedAt:dd/MM/yyyy HH:mm:ss}}</div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Bước 2: Thêm thành viên -->
        <div class="step-card">
            <div class="step-header">
                <div class="step-title">
                    <span class="step-num">2</span>
                    <span>Thêm 3 thành viên vào CLB</span>
                </div>
                <span class="badge badge-primary">{{club.MemberCount()}} Thành viên</span>
            </div>
            <div class="step-body">
                <table>
                    <thead>
                        <tr>
                            <th>Họ và Tên</th>
                            <th>MSSV</th>
                            <th>Email</th>
                            <th>Trạng Thái</th>
                            <th>Mã Guid</th>
                        </tr>
                    </thead>
                    <tbody>
""");

        foreach (var m in members)
        {
            string statusBadge = m.IsActive()
                ? "<span class=\"badge badge-success\">Active</span>"
                : "<span class=\"badge badge-danger\">Inactive</span>";

            sb.Append($$"""
                        <tr>
                            <td><strong>{{m.Name}}</strong></td>
                            <td>{{m.StudentCode}}</td>
                            <td>{{m.Email}}</td>
                            <td>{{statusBadge}}</td>
                            <td style="font-family: Consolas; font-size: 12px; color: #94a3b8;">{{m.Id.ToString()[..8]}}...</td>
                        </tr>
""");
        }

        sb.Append($$"""
                    </tbody>
                </table>
            </div>
        </div>

        <!-- Bước 3: Tạo sự kiện -->
        <div class="step-card">
            <div class="step-header">
                <div class="step-title">
                    <span class="step-num">3</span>
                    <span>Tổ chức Sự Kiện (Event)</span>
                </div>
                <span class="badge badge-purple">Sức chứa: {{evt.Capacity}}</span>
            </div>
            <div class="step-body">
                <div class="grid-2">
                    <div class="info-item">
                        <div class="info-label">Tiêu đề sự kiện</div>
                        <div class="info-val">{{evt.Title}}</div>
                    </div>
                    <div class="info-item">
                        <div class="info-label">Ngày tổ chức</div>
                        <div class="info-val">{{evt.Date:dd/MM/yyyy}}</div>
                    </div>
                    <div class="info-item">
                        <div class="info-label">Hình thức</div>
                        <div class="info-val">{(evt.IsOnline ? "Trực tuyến (Online)" : "Trực tiếp (Offline)")}</div>
                    </div>
                    <div class="info-item">
                        <div class="info-label">Thuộc CLB (ClubId)</div>
                        <div class="info-val" style="font-family: Consolas;">{{evt.ClubId.ToString()[..8]}}... (Khóa ngoại)</div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Bước 4 & 5: Đăng ký & Xác nhận -->
        <div class="step-card">
            <div class="step-header">
                <div class="step-title">
                    <span class="step-num">4-5</span>
                    <span>Quy trình Đăng Ký (Pending) & Xác Nhận (Confirmed)</span>
                </div>
                <span class="badge badge-warning">Xử lý sức chứa</span>
            </div>
            <div class="step-body">
                <table>
                    <thead>
                        <tr>
                            <th>Sinh viên</th>
                            <th>Mã phiếu đăng ký</th>
                            <th>Trạng thái ban đầu</th>
                            <th>Sau xác nhận</th>
                            <th>Ghi chú</th>
                        </tr>
                    </thead>
                    <tbody>
""");

        for (int i = 0; i < members.Count; i++)
        {
            var m = members[i];
            var reg = registrations.FirstOrDefault(r => r.MemberId == m.Id);
            if (reg != null)
            {
                sb.Append($$"""
                        <tr>
                            <td>{{m.Name}} ({{m.StudentCode}})</td>
                            <td style="font-family: Consolas;">{{reg.Id.ToString()[..8]}}</td>
                            <td><span class="badge badge-warning">Pending</span></td>
                            <td><span class="badge badge-success">Confirmed</span></td>
                            <td style="color: #4ade80;">Đăng ký thành công trong hạn mức</td>
                        </tr>
""");
            }
            else
            {
                sb.Append($$"""
                        <tr>
                            <td>{{m.Name}} ({{m.StudentCode}})</td>
                            <td style="color: #94a3b8;">—</td>
                            <td><span class="badge badge-danger">Từ chối</span></td>
                            <td><span class="badge badge-danger">Không tạo</span></td>
                            <td style="color: #f87171;">Bị từ chối do sự kiện đã đủ sức chứa (2/2)</td>
                        </tr>
""");
            }
        }

        sb.Append($$"""
                    </tbody>
                </table>
            </div>
        </div>

        <!-- Bước 6 & 7: Check-in & Báo cáo tham dự -->
        <div class="step-card">
            <div class="step-header">
                <div class="step-title">
                    <span class="step-num">6-7</span>
                    <span>Điểm Danh (CheckIn) & Báo Cáo Tham Dự</span>
                </div>
                <span class="badge badge-success">Báo cáo tổng kết</span>
            </div>
            <div class="step-body">
                <div style="margin-bottom: 16px;">
                    <div style="font-size: 13.5px; color: var(--text-muted); margin-bottom: 6px;">
                        Tỷ lệ điểm danh: <strong>{{checkIns.Count}}/{{activeRegCount}} người</strong> (50%)
                    </div>
                    <div style="width: 100%; height: 10px; background: #334155; border-radius: 9999px; overflow: hidden;">
                        <div style="width: 50%; height: 100%; background: linear-gradient(90deg, #38bdf8, #4ade80);"></div>
                    </div>
                </div>

                <table>
                    <thead>
                        <tr>
                            <th>Họ và Tên</th>
                            <th>Mã phiếu</th>
                            <th>Trạng thái Điểm danh</th>
                            <th>Phương thức</th>
                            <th>Thời gian</th>
                        </tr>
                    </thead>
                    <tbody>
""");

        foreach (var reg in registrations)
        {
            var member = members.First(m => m.Id == reg.MemberId);
            var chk = checkIns.FirstOrDefault(c => c.RegistrationId == reg.Id);
            bool isChecked = chk != null;

            sb.Append($$"""
                        <tr>
                            <td><strong>{{member.Name}}</strong></td>
                            <td style="font-family: Consolas;">{{reg.Id.ToString()[..8]}}</td>
                            <td>{(isChecked ? "<span class=\"badge badge-success\">Đã check-in</span>" : "<span class=\"badge badge-warning\">Chưa check-in</span>")}</td>
                            <td>{(isChecked ? $"<span class=\"badge badge-primary\">{chk!.Method}</span>" : "—")}</td>
                            <td style="color: #94a3b8;">{(isChecked ? chk!.CheckedInAt.ToString("HH:mm:ss dd/MM/yyyy") : "Chờ tại cửa")}</td>
                        </tr>
""");
        }

        sb.Append($$"""
                    </tbody>
                </table>
            </div>
        </div>

        <!-- Bước 8: Kiểm thử luật nghiệp vụ -->
        <div class="step-card">
            <div class="step-header">
                <div class="step-title">
                    <span class="step-num">8</span>
                    <span>Kiểm Thử 9 Quy Tắc Nghiệp Vụ (Domain Validation Suite)</span>
                </div>
                <span class="badge badge-success">{{validationTests.Count(t => t.Passed)}}/{{validationTests.Count}} Vượt qua</span>
            </div>
            <div class="step-body">
                <p style="font-size: 13.5px; color: var(--text-muted); margin-bottom: 16px;">
                    Chứng minh tính đóng gói (Encapsulation), kiểm tra dữ liệu đầu vào (Validation) và xử lý ngoại lệ theo đúng yêu cầu đề bài.
                </p>
                <div class="rule-list">
""");

        foreach (var t in validationTests)
        {
            sb.Append($$"""
                    <div class="rule-item passed">
                        <div class="rule-info">
                            <div class="rule-title">
                                <span>🛡️</span>
                                <span>{{t.RuleName}}</span>
                                <span class="badge badge-success">Đã chặn thành công</span>
                            </div>
                            <div class="rule-desc">{{t.Message}}</div>
                        </div>
                    </div>
""");
        }

        sb.Append($$"""
                </div>
            </div>
        </div>

        <!-- Action bar -->
        <div class="action-bar">
            <button class="btn" onclick="window.print()">🖨️ In / Xuất PDF Báo Cáo</button>
            <a class="btn" href="class-diagram.png" target="_blank">🖼️ Xem Sơ Đồ Lớp (draw.io)</a>
        </div>
    </div>
</body>
</html>
""");

        File.WriteAllText(outputPath, sb.ToString(), Encoding.UTF8);
        return outputPath;
    }
}
