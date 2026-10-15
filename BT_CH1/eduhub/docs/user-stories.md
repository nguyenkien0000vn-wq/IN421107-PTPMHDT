# User Stories — EduHub (Ch01)

Định dạng: *Là [vai trò], tôi muốn [việc cần làm], để [lợi ích].*
Tiêu chí nghiệm thu (AC) viết theo **Given – When – Then**.

---

## US-01 · Sinh viên tham gia một CLB
**Là** sinh viên, **tôi muốn** tham gia một câu lạc bộ, **để** nhận thông tin và đăng ký sự kiện của CLB đó.

- **AC1** — *Given* tôi đang hoạt động và chưa là thành viên, *When* tôi tham gia CLB, *Then* tên tôi xuất hiện trong `club.Members`.
- **AC2** — *Given* tôi đã là thành viên, *When* tôi tham gia lại, *Then* hệ thống từ chối và danh sách không bị trùng.

## US-02 · Sinh viên huỷ đăng ký sự kiện
**Là** sinh viên, **tôi muốn** huỷ phiếu đăng ký khi bận việc đột xuất, **để** nhường chỗ cho bạn khác.

- **AC1** — *Given* phiếu của tôi đang `Pending` hoặc `Confirmed`, *When* tôi huỷ, *Then* phiếu chuyển sang `Cancelled`.
- **AC2** — *Given* phiếu đã `Cancelled`, *When* tôi huỷ lần nữa, *Then* hệ thống báo lỗi.
- **AC3** — *Given* phiếu đã `Cancelled`, *When* tôi muốn xác nhận lại, *Then* hệ thống từ chối (trạng thái cuối).

## US-03 · Sinh viên check-in bằng QR
**Là** sinh viên, **tôi muốn** check-in bằng mã QR tại cửa sự kiện, **để** vào nhanh mà không phải ký tên.

- **AC1** — *Given* phiếu của tôi là `Confirmed`, *When* tôi quét QR, *Then* hệ thống tạo `CheckIn` với `Method = "QR"`.
- **AC2** — *Given* phiếu của tôi là `Pending` hoặc `Cancelled`, *When* tôi quét QR, *Then* hệ thống từ chối check-in.

## US-04 · Ban CLB tạo sự kiện trực tuyến
**Là** thành viên ban CLB, **tôi muốn** tạo sự kiện online với sức chứa giới hạn, **để** kiểm soát số người tham dự.

- **AC1** — *Given* tiêu đề không trống và `Capacity > 0`, *When* tôi tạo sự kiện, *Then* sự kiện có `IsOnline = true` và đúng `ClubId`.
- **AC2** — *Given* tiêu đề trống hoặc `Capacity ≤ 0`, *When* tôi tạo sự kiện, *Then* hệ thống báo lỗi và không tạo gì.

## US-05 · Ban CLB xác nhận phiếu đăng ký
**Là** thành viên ban CLB, **tôi muốn** xác nhận phiếu đăng ký đang chờ, **để** chốt danh sách người tham dự.

- **AC1** — *Given* phiếu `Pending`, *When* tôi xác nhận, *Then* phiếu thành `Confirmed`.
- **AC2** — *Given* phiếu không phải `Pending`, *When* tôi xác nhận, *Then* hệ thống báo lỗi nêu rõ trạng thái hiện tại.

## US-06 · Ban CLB xem người chưa check-in
**Là** thành viên ban CLB, **tôi muốn** xem ai đã đăng ký nhưng chưa check-in, **để** nhắc họ hoặc nhường chỗ.

- **AC1** — *Given* sự kiện có phiếu đăng ký, *When* tôi mở báo cáo, *Then* thấy "số đăng ký / sức chứa" và "số đã check-in".
- **AC2** — *Given* báo cáo đang mở, *When* tôi xem danh sách, *Then* mỗi người được đánh dấu "đã" hoặc "chưa" check-in.

## US-07 · Admin ngừng hoạt động một thành viên
**Là** admin, **tôi muốn** ngừng một thành viên vi phạm nội quy, **để** họ không thể đăng ký sự kiện mới nhưng lịch sử vẫn được giữ.

- **AC1** — *Given* thành viên đang `Active`, *When* tôi ngừng, *Then* `IsActive()` trả về `false`.
- **AC2** — *Given* thành viên đã ngừng, *When* họ đăng ký sự kiện, *Then* hệ thống từ chối.
- **AC3** — *Given* thành viên đã ngừng, *When* tôi xem lịch sử, *Then* các phiếu đăng ký cũ vẫn còn.

---

| Vai trò | Stories |
|---|---|
| Sinh viên | US-01, US-02, US-03 |
| Ban CLB | US-04, US-05, US-06 |
| Admin | US-07 |
