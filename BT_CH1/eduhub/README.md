# EduHub — Bài tập Chương 1

- Sinh viên: **<Nguyễn Trung Kiên>** — MSSV: **<00244199288>**
- Môn: IN4211 — Phát triển phần mềm hướng đối tượng

Domain model của hệ thống quản lý CLB và sự kiện sinh viên (chỉ class OOP, chưa có database/API).

## Cấu trúc

EduHub.slnx
├── EduHub.Domain/ # 5 entity: Member, Club, Event, Registration, CheckIn (+ enums)
├── EduHub.Console/ # Program.cs: demo 8 bước
└── docs/
├── user-stories.md
├── class-diagram.md
├── class-diagram.png
└── demo.html # Báo cáo trực quan chạy 8 bước

## Sơ đồ lớp (Class Diagram)

![Sơ đồ lớp EduHub](docs/class-diagram.png)

> Chi tiết các thuộc tính, phương thức và quan hệ xem tại [docs/class-diagram.md](docs/class-diagram.md).

## Chạy thử

```bash
dotnet run --project EduHub.Console
```

> **Ghi chú**: Khi chạy lệnh trên, chương trình sẽ tự động xuất ra trang báo cáo dashboard trực quan tại **`docs/demo.html`**. Bạn có thể mở file này bằng trình duyệt bất kỳ hoặc in ra PDF để nộp báo cáo.

## Kết quả chạy

> TODO: chèn ảnh chụp màn hình kết quả `dotnet run` tại đây
> `![demo](docs/demo.png)`

## Ghi chú thiết kế

- Thuộc tính dùng `private set`, constructor `private`, tạo đối tượng qua `Create()` có kiểm tra dữ liệu.
- `Club.Members` là `IReadOnlyList<Member>`; thêm thành viên bắt buộc qua `AddMember()` (chặn trùng).
- Trạng thái dùng `enum` / hằng số, không rải chuỗi `"Pending"`.
