# 03 - Trình bày logic của dự án DemoPick

## 1) Mục tiêu file
File này giải thích logic nghiệp vụ của dự án theo ngôn ngữ dễ học:
- App khởi động như thế nào.
- Người dùng thao tác ra sao.
- Dữ liệu đi từ UI -> Service/Controller -> Database như thế nào.
- Quan hệ giữa đặt sân, POS, thanh toán, báo cáo.

## 2) Tổng quan kiến trúc logic
DemoPick là ứng dụng WinForms có kiến trúc logic 4 lớp chính:
1. `Views` (Form/UserControl): giao diện người dùng.
2. `Controllers` + `Services`: xử lý nghiệp vụ.
3. `Models`: đối tượng dữ liệu.
4. SQL Server: lưu dữ liệu.

Luồng tổng quát:
- App mở -> đảm bảo DB/migration -> đăng nhập -> vào màn hình chính.
- Người dùng thao tác trên module.
- Module gọi service/controller.
- Service/controller truy vấn và cập nhật DB.
- UI refresh dữ liệu.

## 3) Logic khởi động hệ thống
Code mẫu thật trong `Program.cs`:
```csharp
SchemaInstaller.EnsureDatabaseAndSchema();
MigrationsRunner.ApplyPendingMigrations();
Application.Run(new AppFlowContext());
```

Ý nghĩa:
- Trước khi vào giao diện chính, hệ thống tự đảm bảo DB đủ cấu trúc.
- Tránh lỗi thiếu bảng/cột khi chạy trên máy mới.

## 4) Logic xác thực người dùng
### 4.1 Đăng nhập
`AuthService.TryLogin` thực hiện:
1. Validate dữ liệu đầu vào.
2. Query danh sách account theo identifier (username/email/phone/fullname).
3. Kiểm tra lockout.
4. So sánh hash mật khẩu theo salt.
5. Nếu sai: tăng failed attempts.
6. Nếu đúng: reset failed attempts và tạo session.

Snippet code thật:
```csharp
byte[] computed = HashPassword(password, storedSalt);
if (!FixedTimeEquals(storedHash, computed))
    continue;
```

## 5) Logic đặt sân (Booking)
### 5.1 Tạo booking
`BookingController.SubmitBooking` gọi stored procedure `sp_CreateBooking`.
Stored procedure kiểm tra trùng lịch trước khi insert.

Snippet code thật:
```csharp
using (var cmd = new SqlCommand("sp_CreateBooking", conn))
{
    cmd.CommandType = CommandType.StoredProcedure;
    cmd.Parameters.AddWithValue("@CourtID", courtId);
    cmd.Parameters.AddWithValue("@StartTime", startTime);
    cmd.Parameters.AddWithValue("@EndTime", endTime);
    cmd.ExecuteNonQuery();
}
```

### 5.2 Đổi ca / Hủy booking
`BookingController` xử lý:
- Không cho đổi/hủy booking đã `Paid`.
- Không cho đổi sang giờ bị trùng sân.
- Hủy booking bằng update status `Cancelled` (soft delete logic).

## 6) Logic POS và giỏ hàng
`PosService` có cơ chế giữ đơn tạm theo sân:
- `SavePendingOrder(courtName, lines)`
- `GetPendingOrder(courtName)`
- `ClearPendingOrder(courtName)`

Code mẫu thật:
```csharp
public static Dictionary<string, List<CartLine>> PendingOrders { get; }
    = new Dictionary<string, List<CartLine>>(StringComparer.OrdinalIgnoreCase);
```

Ý nghĩa:
- Mỗi sân có thể có giỏ hàng tạm riêng.
- Đổi qua lại giữa các sân vẫn giữ được dữ liệu chưa checkout.

## 7) Logic checkout/thanh toán
`PosService.Checkout(...)` là điểm nghiệp vụ trung tâm.

Các bước chính:
1. Mở transaction DB.
2. Đóng booking active theo sân (nếu có).
3. Resolve member thực tế để ghi nhận điểm/giờ.
4. Tạo hóa đơn `Invoices`.
5. Tạo dòng hóa đơn `InvoiceDetails`.
6. Trừ kho (trừ dịch vụ).
7. Cộng chi tiêu + giờ mua cho member.
8. Ghi log `SystemLogs`.
9. Commit transaction.

Code mẫu thật:
```csharp
using (var tran = conn.BeginTransaction(IsolationLevel.ReadCommitted))
{
    // insert invoice + insert invoice details + update member + log
    tran.Commit();
}
```

## 8) Logic tính giá
`PriceCalculator.CalculateTotal` chia tiền sân theo block giờ và ngày thường/cuối tuần.

Điểm chính:
- Chia khoảng thời gian chơi thành nhiều block giá.
- Nhân hệ số sân (`courtRateMultiplier`) cho sân tập/sân đặc thù.
- Cộng dịch vụ theo đơn vị (`giờ`, `cái`, `rổ`).
- Trừ giảm giá cho khách cố định.

Code mẫu thật:
```csharp
while (current < end)
{
    foreach (var block in Blocks)
    {
        DateTime overlapStart = current > blockStartDateTime ? current : blockStartDateTime;
        DateTime overlapEnd = blockEnd < blockEndDateTime ? blockEnd : blockEndDateTime;
        if (overlapStart < overlapEnd)
        {
            decimal hours = (decimal)(overlapEnd - overlapStart).TotalHours;
            decimal rate = (isWeekend ? block.RateWeekend : block.RateWeekday) * courtRateMultiplier;
            breakdown.SubtotalCourts += hours * rate;
        }
    }
    current = blockEnd;
}
```

## 9) Logic báo cáo
`ReportService` lấy dữ liệu theo khoảng thời gian:
- Top sân.
- KPI (doanh thu, occupancy, khách mới).
- Trend doanh thu theo ngày.

`UCBaoCao` gọi async nhiều hàm service và render biểu đồ.

Code mẫu thật:
```csharp
await LoadTopCourtsAsync(fromStart, toExclusive);
await LoadKpisAsync(fromStart, toExclusive, days);
await LoadChartsAsync(fromStart, toExclusive, fromStart.Date, toDateInclusive.Date);
```

## 10) Logic phân quyền
Trong `FrmChinh.ApplyRolePermissions()`:
- Admin: full quyền.
- Staff: được vào các module tác nghiệp cho phép.
- Người không đủ quyền sẽ bị điều hướng về Dashboard.

## 11) Logic an toàn và ổn định
- Logging best-effort: `DatabaseHelper.TryLog`, `TryLogThrottled`.
- Không để lỗi log làm crash app.
- Nhiều thao tác DB có `try/catch` để giảm gãy luồng người dùng.
- Smoke mode (`--smoke`) để kiểm thử tự động.

## 12) Kết luận logic dự án
Logic DemoPick được tổ chức theo hướng:
- UI mỏng, nghiệp vụ tập trung ở Service/Controller.
- Dữ liệu giao dịch quan trọng chạy trong transaction.
- Trạng thái booking/invoice/member liên kết chặt để đảm bảo nhất quán.

Bạn nên đọc tiếp file API và file “hàm/cấu trúc code cơ bản” để hiểu sâu cách gọi hàm và cách code được triển khai thực tế.
