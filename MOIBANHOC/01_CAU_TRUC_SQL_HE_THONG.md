# 01 - Cấu trúc SQL của hệ thống hiện tại (DemoPick)

## 1) Mục tiêu tài liệu
Tài liệu này giúp bạn hiểu cấu trúc cơ sở dữ liệu hiện tại của hệ thống DemoPick theo hướng học tập:
- Nắm được bảng nào lưu dữ liệu gì.
- Nắm quan hệ giữa các bảng.
- Hiểu luồng dữ liệu từ đặt sân -> hóa đơn -> báo cáo.
- Hiểu các thành phần SQL nâng cao đang được dùng: Stored Procedure, Trigger, Migration.

## 2) Tệp SQL nền tảng của dự án
Trong dự án hiện có các nhóm SQL chính:
- Schema chính: `Database/PickleProDB_Complete.sql`
- Migration tăng dần: `Database/Migrations/*.sql`
- Dữ liệu test: `Database/TesterData_Seed.sql`
- Legacy compatibility: `Database/Legacy/migration.sql`

Bạn đã có bản gộp đầy đủ ở:
- `MOIBANHOC/02_TONG_HOP_DATABASE_DU_AN.sql`

## 3) Sơ đồ thực thể cốt lõi
Các bảng chính:
- `Courts`: thông tin sân, loại sân, trạng thái, giá giờ.
- `Members`: thông tin hội viên/khách.
- `Bookings`: lịch đặt sân.
- `Products`: sản phẩm bán ở POS + dịch vụ.
- `Invoices`: đầu hóa đơn.
- `InvoiceDetails`: chi tiết từng dòng của hóa đơn.
- `StaffAccounts`: tài khoản đăng nhập hệ thống.
- `SystemLogs`: nhật ký vận hành.

Quan hệ chính:
- `Bookings.CourtID -> Courts.CourtID`
- `Bookings.MemberID -> Members.MemberID`
- `Invoices.MemberID -> Members.MemberID`
- `InvoiceDetails.InvoiceID -> Invoices.InvoiceID`
- `InvoiceDetails.ProductID -> Products.ProductID` (có thể null cho dòng tiền sân)
- `InvoiceDetails.BookingID -> Bookings.BookingID` (liên kết thanh toán tiền sân)

## 4) Thiết kế bảng và ý nghĩa nghiệp vụ

### 4.1 Bảng Courts
- Dùng để cấu hình số sân và giá theo loại.
- Có `Status` để bật/tắt sân thay vì xóa cứng.

Ví dụ code SQL thật trong dự án:
```sql
CREATE TABLE dbo.Courts (
    CourtID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    CourtType NVARCHAR(50) NOT NULL,
    Status NVARCHAR(50) NOT NULL CONSTRAINT DF_Courts_Status DEFAULT 'Active',
    HourlyRate DECIMAL(18,2) NOT NULL
);
```

### 4.2 Bảng Members
- Theo dõi khách hàng và trạng thái cố định/vãng lai.
- `TotalHoursPurchased` và `IsFixed` hỗ trợ rule tự nâng hạng khách cố định.

Ví dụ code SQL thật:
```sql
CREATE TABLE dbo.Members (
    MemberID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    Level NVARCHAR(50) NOT NULL CONSTRAINT DF_Members_Level DEFAULT 'NEWBIE',
    Tier NVARCHAR(50) NOT NULL CONSTRAINT DF_Members_Tier DEFAULT 'Bronze',
    TotalSpent DECIMAL(18,2) NOT NULL CONSTRAINT DF_Members_TotalSpent DEFAULT 0,
    TotalHoursPurchased DECIMAL(18,2) NOT NULL CONSTRAINT DF_Members_TotalHoursPurchased DEFAULT 0,
    IsFixed BIT NOT NULL CONSTRAINT DF_Members_IsFixed DEFAULT 0,
    CreatedAt DATETIME NOT NULL CONSTRAINT DF_Members_CreatedAt DEFAULT GETDATE()
);
```

### 4.3 Bảng Bookings
- Lưu lịch đặt sân.
- Có `Note` để ghi chú booking.
- `Status` phân biệt Confirmed / Paid / Cancelled / Maintenance.

Ví dụ code SQL thật:
```sql
CREATE TABLE dbo.Bookings (
    BookingID INT IDENTITY(1,1) PRIMARY KEY,
    CourtID INT NOT NULL,
    MemberID INT NULL,
    GuestName NVARCHAR(100) NULL,
    Note NVARCHAR(200) NULL,
    StartTime DATETIME NOT NULL,
    EndTime DATETIME NOT NULL,
    Status NVARCHAR(50) NOT NULL CONSTRAINT DF_Bookings_Status DEFAULT 'Confirmed'
);
```

### 4.4 Bảng Invoices + InvoiceDetails
- `Invoices` là header hóa đơn (tổng tiền, giảm giá, phương thức thanh toán).
- `InvoiceDetails` là từng dòng tiền sân/sản phẩm.
- Thiết kế cho phép một hóa đơn chứa nhiều dòng sản phẩm và/hoặc tiền sân.

### 4.5 Bảng StaffAccounts
- Dùng cho xác thực người dùng app.
- Mật khẩu lưu dạng hash + salt (`PasswordHash`, `PasswordSalt`).
- Có lockout fields để chống brute-force cơ bản:
  - `FailedLoginCount`
  - `LockoutUntil`
  - `LastFailedLoginAt`

## 5) Thành phần SQL nghiệp vụ nâng cao

### 5.1 Stored Procedure: sp_CreateBooking
Mục tiêu:
- Chặn trùng lịch cùng sân theo khoảng thời gian.
- Chèn booking chuẩn.

Snippet SQL thật:
```sql
IF EXISTS (
    SELECT 1 FROM dbo.Bookings
    WHERE CourtID = @CourtID
      AND Status != 'Cancelled'
      AND (StartTime < @EndTime AND EndTime > @StartTime)
)
BEGIN
    RAISERROR('Court is already booked for this time slot.', 16, 1);
    RETURN;
END
```

### 5.2 Trigger: trg_ReduceStock
Mục tiêu:
- Tự động trừ tồn kho khi phát sinh dòng `InvoiceDetails` cho sản phẩm hàng hóa.
- Bỏ qua sản phẩm category `Dịch vụ`.

Snippet SQL thật:
```sql
UPDATE p
SET p.StockQuantity = p.StockQuantity - i.Quantity
FROM dbo.Products p
INNER JOIN inserted i ON p.ProductID = i.ProductID
WHERE i.ProductID IS NOT NULL
  AND ISNULL(p.Category, N'') <> N'Dịch vụ';
```

### 5.3 Trigger: trg_UpdateMemberTier
- Tự cộng `TotalSpent` cho member khi thêm invoice.
- Tự cập nhật tier Bronze/Silver/Gold theo ngưỡng.

## 6) Chỉ mục và ràng buộc quan trọng
- `UX_Products_SKU`: SKU không trùng.
- `UX_StaffAccounts_Username`: username không trùng.
- Hệ thống foreign key giúp đảm bảo toàn vẹn tham chiếu giữa booking, invoice, member, product.

## 7) Migration và tính an toàn khi nâng cấp DB
Luồng chạy app:
1. `SchemaInstaller.EnsureDatabaseAndSchema()`
2. `MigrationsRunner.ApplyPendingMigrations()`

Điểm hay của migration hiện tại:
- Lưu checksum migration đã chạy.
- Nếu sửa nội dung migration cũ đã apply, hệ thống báo mismatch để tránh drift.

Code mẫu trong dự án (`Services/MigrationsRunner.cs`):
```csharp
if (applied.TryGetValue(migrationId, out var appliedChecksum))
{
    if (appliedChecksum != null && appliedChecksum.Length == 32 && !checksum.SequenceEqual(appliedChecksum))
    {
        throw new MigrationChecksumMismatchException(migrationId);
    }
    continue;
}
```

## 8) Checklist tự học SQL theo dự án này
1. Đọc cấu trúc bảng trong file tổng hợp.
2. Vẽ lại ERD bằng tay từ các FK.
3. Chạy seed test để có dữ liệu thật.
4. Viết truy vấn tổng hợp:
   - Doanh thu theo ngày.
   - Tỷ lệ lấp đầy sân.
   - Top sân doanh thu cao.
5. Thử thêm migration mới theo chuẩn `NNNN__Description.sql`.

## 9) Kết luận
CSDL DemoPick được thiết kế theo hướng:
- Đủ thực dụng cho WinForms app.
- Có cơ chế migration/checksum rõ ràng.
- Có trigger/procedure để giảm logic lặp ở tầng UI.

Bạn có thể dùng file này làm tài liệu gốc trước khi đọc tiếp các file về logic, API và bảo mật trong thư mục MOIBANHOC.
