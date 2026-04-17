# 04 - Phát biểu bài toán

## 1) Bối cảnh
Một cơ sở thể thao pickleball cần một hệ thống phần mềm để quản lý trọn vòng đời vận hành hằng ngày:
- Quản lý sân theo loại và trạng thái.
- Quản lý lịch đặt sân theo khung giờ.
- Bán hàng tại quầy (POS) và ghi nhận dịch vụ.
- Thanh toán và in hóa đơn.
- Theo dõi khách hàng, doanh thu, hiệu suất sử dụng sân.

Hệ thống hiện tại được xây dựng dưới dạng ứng dụng WinForms và dùng SQL Server.

## 2) Bài toán cần giải
Thiết kế và triển khai một hệ thống quản lý sân pickleball có khả năng:
1. Đảm bảo đặt sân không bị trùng lịch.
2. Quản lý khách hàng (cố định/vãng lai) và chi tiêu.
3. Kết hợp thanh toán tiền sân + sản phẩm + dịch vụ trong một hóa đơn.
4. Tự động cập nhật dữ liệu kho và dữ liệu khách hàng sau thanh toán.
5. Cung cấp báo cáo tổng hợp, trend doanh thu, top sân.
6. Đảm bảo cơ bản về bảo mật đăng nhập và an toàn dữ liệu.

## 3) Mục tiêu nghiệp vụ
- Tối ưu vận hành quầy lễ tân và thu ngân.
- Giảm sai sót đặt lịch/thu tiền thủ công.
- Chuẩn hóa dữ liệu báo cáo để ra quyết định.
- Tăng trải nghiệm khách hàng và hiệu suất khai thác sân.

## 4) Đối tượng sử dụng
- Quản trị viên (Admin): toàn quyền quản trị vận hành.
- Nhân viên (Staff): thao tác đặt lịch, bán hàng, thanh toán, xem dữ liệu theo phân quyền.
- Khách hàng: được phục vụ qua nghiệp vụ nội bộ (không tương tác trực tiếp vào app backend).

## 5) Phạm vi chức năng chính
1. Xác thực người dùng: đăng nhập/đăng ký/đổi mật khẩu.
2. Quản lý lịch: tạo booking, đổi ca, hủy booking, đặt cố định.
3. POS/Checkout: tạo giỏ, chọn sản phẩm, áp dụng giảm giá, tạo hóa đơn.
4. Quản lý khách hàng: tra cứu và tổng hợp dữ liệu.
5. Quản lý kho hàng: tồn kho, cảnh báo mức thấp, lịch sử giao dịch.
6. Báo cáo: KPI, top sân, doanh thu theo ngày.

## 6) Ràng buộc và giả thiết
- Hệ thống chạy trên Windows và SQL Server.
- Connection string được cấu hình sẵn trong `App.config` hoặc biến môi trường.
- Môi trường thực tế có thể có dữ liệu cũ, nên cần migration idempotent.
- Không yêu cầu kiến trúc microservices; ưu tiên vận hành ổn định một ứng dụng desktop.

## 7) Yêu cầu phi chức năng
- Tính nhất quán dữ liệu: thao tác thanh toán quan trọng phải dùng transaction.
- Tính khả dụng: app có thể tự bootstrap DB để giảm lỗi setup.
- Bảo trì: migration có checksum chống sửa lệch lịch sử.
- Bảo mật cơ bản: mật khẩu hash+salt, lockout tạm thời.

## 8) Tiêu chí thành công
- Người dùng đăng nhập được và vào module đúng quyền.
- Đặt sân không trùng lịch.
- Checkout thành công tạo đầy đủ invoice + detail.
- Báo cáo phản ánh đúng dữ liệu giao dịch.
- Có cơ chế migration/seed để môi trường mới chạy được nhanh.

## 9) Code mẫu liên hệ trực tiếp với bài toán
Code thật trong `Program.cs` cho thấy bài toán có yêu cầu tự khởi tạo DB trước khi vận hành:
```csharp
try
{
    SchemaInstaller.EnsureDatabaseAndSchema();
    MigrationsRunner.ApplyPendingMigrations();
}
catch (Exception ex)
{
    MessageBox.Show(DbDiagnostics.BuildDbInitErrorMessage(ex), "Lỗi CSDL");
    return;
}
```

Code thật trong `BookingController` thể hiện yêu cầu không trùng lịch:
```csharp
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

## 10) Kết luận
Bài toán của DemoPick là bài toán quản lý vận hành sân thể thao theo hướng tích hợp:
- lịch sân + POS + thanh toán + báo cáo
trên cùng một nền tảng dữ liệu nhất quán.

Đây là bài toán thực tế, có độ phức tạp vừa phải, phù hợp để học cả:
- thiết kế CSDL,
- luồng nghiệp vụ,
- transaction,
- bảo mật cơ bản trong ứng dụng doanh nghiệp nội bộ.
