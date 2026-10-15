# Sơ đồ lớp EduHub

## 1. Sơ đồ lớp thiết kế (draw.io)

![Sơ đồ lớp EduHub](class-diagram.png)

## 2. Sơ đồ lớp (Mermaid)

```mermaid
classDiagram
    direction LR

    class Club {
        +Guid Id
        +string Name
        +string Description
        +DateTime CreatedAt
        +IReadOnlyList~Member~ Members
        +Create(name, description)$ Club
        +AddMember(member) void
        +MemberCount() int
    }
    class Member {
        +Guid Id
        +string Name
        +string Email
        +string StudentCode
        +DateTime JoinedAt
        +MemberStatus Status
        +Create(name, email, studentCode)$ Member
        +IsActive() bool
        +Deactivate() void
    }
    class Event {
        +Guid Id
        +Guid ClubId  «FK»
        +string Title
        +DateTime Date
        +int Capacity
        +bool IsOnline
        +Create(clubId, title, date, capacity, isOnline)$ Event
        +HasCapacity(registeredCount) bool
    }
    class Registration {
        +Guid Id
        +Guid MemberId  «FK»
        +Guid EventId  «FK»
        +DateTime RegisteredAt
        +RegistrationStatus Status
        +Create(member, evt, count)$ Registration
        +Confirm() void
        +Cancel() void
    }
    class CheckIn {
        +Guid Id
        +Guid RegistrationId  «FK»
        +DateTime CheckedInAt
        +string Method
        +Create(registration, method)$ CheckIn
        +Summary() string
    }

    Club "*" -- "*" Member : tham gia (bảng ClubMember ở Ch04)
    Club "1" --> "*" Event : tổ chức
    Member "1" --> "*" Registration : đăng ký
    Event "1" --> "*" Registration : nhận
    Registration "1" --> "0..1" CheckIn : điểm danh
```

## Quan hệ và khoá ngoại

| Quan hệ | Loại | Khoá ngoại |
|---|---|---|
| Member — Club | * — * | bảng trung gian `ClubMember` (Ch04) |
| Club — Event | 1 — * | `Event.ClubId` |
| Member — Registration | 1 — * | `Registration.MemberId` |
| Event — Registration | 1 — * | `Registration.EventId` |
| Registration — CheckIn | 1 — 0..1 | `CheckIn.RegistrationId` |

Quy tắc: phía "nhiều" giữ khoá ngoại. Khoá chính của mọi entity là `Id` kiểu `Guid`.

## Vòng đời Registration

```mermaid
stateDiagram-v2
    [*] --> Pending
    Pending --> Confirmed : Confirm()
    Pending --> Cancelled : Cancel()
    Confirmed --> Cancelled : Cancel()
    Confirmed --> [*] : CheckIn.Create()
```
