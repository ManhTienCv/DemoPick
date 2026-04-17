# 09 - Bảo mật của dự án hiện tại

## 1) Mục tiêu
Tài liệu này phân tích bảo mật hiện trạng của DemoPick theo hướng dễ học:
- Hệ thống đang có biện pháp bảo mật gì.
- Điểm mạnh, điểm yếu.
- Đề xuất cải tiến theo mức ưu tiên.

Nguồn tham chiếu chính:
- `Docs/SECURITY-AUDIT.md`
- `Services/AuthService.cs`
- `Services/Db.cs`
- `Services/MigrationsRunner.cs`

---

## 2) Mô hình rủi ro cơ bản
Giả định các rủi ro thực tế:
1. Người dùng nhập sai/sập app do lỗi kết nối DB.
2. Đăng nhập bị thử mật khẩu nhiều lần (brute-force online).
3. Rò rỉ thông tin config/connection string.
4. Drift database do chỉnh sửa migration cũ.
5. Dữ liệu giao dịch bị lỗi giữa chừng khi checkout.

---

## 3) Cơ chế bảo mật đã có trong dự án

## 3.1 Hash + Salt cho mật khẩu
Trong `StaffAccounts`, mật khẩu không lưu plaintext mà lưu:
- `PasswordHash` (VARBINARY)
- `PasswordSalt` (VARBINARY)

Code mẫu thật trong `AuthService`:
```csharp
byte[] computed = HashPassword(password, storedSalt);
if (!FixedTimeEquals(storedHash, computed))
    continue;
```

Ý nghĩa:
- Giảm rủi ro lộ mật khẩu thật khi DB bị đọc trái phép.

## 3.2 Lockout tạm thời khi đăng nhập sai nhiều lần
Hệ thống có các cột:
- `FailedLoginCount`
- `LockoutUntil`
- `LastFailedLoginAt`

Code mẫu thật:
```csharp
private const int MaxFailedLoginAttempts = 5;
private const int LockoutMinutes = 5;
```

Ý nghĩa:
- Giảm brute-force online ở mức cơ bản.

## 3.3 Parameterized SQL
Phần lớn truy vấn dùng `SqlParameter`, giảm SQL injection.

Code mẫu thật:
```csharp
DatabaseHelper.ExecuteQuery(
    SqlQueries.Auth.LoginCandidatesByIdentifier,
    new SqlParameter("@Id", id));
```

## 3.4 Kiểm soát migration bằng checksum
`MigrationsRunner` lưu checksum migration đã chạy.
Nếu migration cũ bị sửa, hệ thống báo lỗi thay vì chạy mù.

Code mẫu thật:
```csharp
if (appliedChecksum != null && appliedChecksum.Length == 32 && !checksum.SequenceEqual(appliedChecksum))
{
    throw new MigrationChecksumMismatchException(migrationId);
}
```

## 3.5 Bảo vệ connection strings (best effort)
`Db.cs` hỗ trợ mã hóa section connectionStrings (DPAPI) khi bật cờ.

Code mẫu thật:
```csharp
section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
section.SectionInformation.ForceSave = true;
```

---

## 4) Các điểm yếu/rủi ro còn tồn tại

## 4.1 Chưa có cơ chế phân quyền dữ liệu sâu
- Hiện chủ yếu phân quyền ở mức module UI.
- Chưa có policy authorization chi tiết theo hành động/data row.

## 4.2 Có thể lộ chi tiết kỹ thuật khi lỗi DB
- Thông báo lỗi có thể chứa nhiều thông tin kỹ thuật.
- Nên tách user message và technical log rõ hơn cho production.

## 4.3 Cần siết quy trình bootstrap tài khoản admin
- Audit gợi ý chú ý rủi ro seed tài khoản mặc định.
- Nên chuẩn hóa quy trình bootstrap an toàn giữa DEV và RELEASE.

## 4.4 Chưa có Web security layer
- Vì là desktop app nên chưa có bảo vệ kiểu API gateway, JWT, CSRF.
- Nếu sau này chuyển web, cần bổ sung toàn bộ tầng bảo vệ web.

---

## 5) Mức độ bảo mật theo thành phần

### Xác thực
- Mức hiện tại: Khá.
- Lý do: Hash+salt, lockout cơ bản, reset failed login.

### Truy cập dữ liệu
- Mức hiện tại: Khá.
- Lý do: Query có parameter, transaction rõ cho nghiệp vụ quan trọng.

### Quản trị cấu hình
- Mức hiện tại: Trung bình.
- Lý do: Có option bảo vệ connection string nhưng cần vận hành chuẩn.

### Quan sát và kiểm toán
- Mức hiện tại: Trung bình.
- Lý do: Có SystemLogs nhưng cần chuẩn hóa log level, retention, dashboard bảo mật.

---

## 6) Đề xuất cải tiến ưu tiên

## P1 - Nên làm sớm
1. Chuẩn hóa policy mật khẩu mạnh hơn (độ dài, ký tự đặc biệt).
2. Bổ sung bắt buộc đổi mật khẩu ở lần đăng nhập đầu.
3. Tách thông báo lỗi người dùng và log kỹ thuật ở chế độ production.

## P2 - Nên làm tiếp
1. Chuẩn hóa phân quyền theo hành động (action-based authorization).
2. Ký và kiểm tra integrity cho binary/script khi release.
3. Bổ sung cảnh báo đăng nhập bất thường (nếu có điều kiện).

## P3 - Nâng cao
1. Mã hóa thêm dữ liệu nhạy cảm theo cột.
2. Tích hợp SIEM/log aggregation nếu triển khai quy mô lớn.
3. Nếu chuyển web: thêm JWT/OAuth2, CSRF protection, rate limit API.

---

## 7) Checklist tự đánh giá bảo mật nhanh
- [ ] Mật khẩu không lưu plaintext
- [ ] Có lockout đăng nhập sai
- [ ] Query có parameter
- [ ] Checkout có transaction
- [ ] Migration có checksum
- [ ] Connection string được bảo vệ (khi cần)
- [ ] Seed admin được kiểm soát an toàn

---

## 8) Kết luận
Bảo mật của DemoPick hiện tại ở mức tốt cho một ứng dụng nội bộ desktop:
- đã có nền tảng xác thực và bảo vệ dữ liệu cơ bản,
- đã có nhiều kỹ thuật đúng hướng.

Để đạt mức production mạnh hơn, cần ưu tiên nâng chuẩn vận hành bảo mật, phân quyền sâu, và quy trình phát hành an toàn.
