# 06 - Phân tích API của dự án (API nội bộ Service/Controller)

## 1) Lưu ý quan trọng
Dự án DemoPick hiện tại là WinForms, **không có Web API REST public** kiểu `/api/...`.

Khái niệm “API” trong tài liệu này là:
- Các hàm public của `Controllers` và `Services` được các màn hình gọi để xử lý nghiệp vụ.
- Có thể xem đây là Internal Application API.

---

## 2) Danh mục API nội bộ theo module

## 2.1 Booking API (Controllers/BookingController.cs)

### a) `GetOrCreateMemberId(string fullName, string phone)`
- Mục đích: tìm member theo phone, nếu chưa có thì tạo mới.
- Input: tên + số điện thoại.
- Output: `int?` MemberID.
- Side-effect: có thể insert bản ghi mới vào `Members`.

### b) `GetCourts()`
- Mục đích: lấy danh sách sân còn active.
- Output: `List<CourtModel>`.
- Đặc điểm: có sắp xếp theo loại sân và số sân.

### c) `GetBookingsByDate(DateTime date)`
- Mục đích: lấy lịch booking theo ngày.
- Output: `List<BookingModel>`.

### d) `SubmitBooking(...)` (nhiều overload)
- Mục đích: tạo booking.
- Logic: gọi `sp_CreateBooking`, chống trùng lịch ở DB.

### e) `UpdateBookingTime(...)`, `UpdateBookingTimeAndNote(...)`
- Mục đích: đổi ca và cập nhật ghi chú.
- Validate: không cho đổi booking đã Paid/Cancelled, không cho trùng lịch.

### f) `DeactivateCourt(int courtId)`
- Mục đích: vô hiệu hóa sân.
- Rule: sân có booking hiện tại/tương lai thì không được deactivate.

### g) `CancelBooking(int bookingId)`
- Mục đích: hủy booking theo soft status.

Code mẫu thật:
```csharp
public void SubmitBooking(int courtId, int? memberId, string guestName, string note, DateTime startTime, DateTime endTime, string status)
{
    TryEnsureBookingNoteSchema();

    using (var conn = DatabaseHelper.GetConnection())
    using (var cmd = new SqlCommand("sp_CreateBooking", conn))
    {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@CourtID", courtId);
        // ...
        cmd.ExecuteNonQuery();
    }
}
```

---

## 2.2 POS/Checkout API (Services/PosService.cs)

### a) `SavePendingOrder(string courtName, List<CartLine> lines)`
- Lưu giỏ hàng tạm theo sân.

### b) `GetPendingOrder(string courtName)`
- Lấy giỏ hàng tạm theo sân.

### c) `ClearPendingOrder(string courtName)`
- Xóa giỏ hàng tạm theo sân.

### d) `Checkout(int memberId, IReadOnlyList<CartLine> lines, decimal totalAmount, decimal discountAmount, decimal finalAmount, string paymentMethod, string courtNameForLog)`
- API nghiệp vụ quan trọng nhất.
- Trả về: `invoiceId`.
- Tác động DB:
1. Đóng booking liên quan.
2. Insert `Invoices`.
3. Insert `InvoiceDetails`.
4. Cập nhật stock/member.
5. Ghi log.
- Có transaction và rollback khi lỗi.

Code mẫu thật:
```csharp
using (var tran = conn.BeginTransaction(IsolationLevel.ReadCommitted))
{
    int invoiceId = InsertInvoice(conn, tran, effectiveMemberId, totalAmount, discountAmount, finalAmount, paymentMethod);
    // insert details + update member + log
    tran.Commit();
    return invoiceId;
}
```

---

## 2.3 Invoice API (Services/InvoiceService.cs)

### a) `GetInvoiceHeader(int invoiceId)`
### b) `GetInvoiceHeader(int invoiceId, string courtName)`
- Lấy header hóa đơn kèm thông tin member và thời gian booking gần nhất.

### c) `GetInvoiceLines(int invoiceId)`
- Lấy danh sách dòng hóa đơn.

Code mẫu thật:
```csharp
var dt = DatabaseHelper.ExecuteQuery(
    SqlQueries.Invoice.InvoiceLines,
    new SqlParameter("@InvoiceID", invoiceId)
);
```

---

## 2.4 Pricing API (Services/PriceCalculator.cs)

### a) `GuessServiceUnit(string serviceName)`
- Đoán đơn vị dịch vụ (`Giờ`, `Cái`, `Rổ`).

### b) `GetCourtRateMultiplier(string courtType, string courtName)`
- Xác định hệ số giá theo loại sân.

### c) `CalculateTotal(DateTime start, DateTime end, bool isFixedCustomer, List<ServiceCharge> services = null, decimal courtRateMultiplier = 1m)`
- Trả về `PriceBreakdown` gồm:
  - SubtotalCourts
  - SubtotalServices
  - DiscountAmount
  - GrandTotal

---

## 2.5 Customer API (Services/CustomerService.cs)

### a) `GetAllCustomersAsync()`
- Trả danh sách khách hàng map sang `CustomerModel`.

### b) `GetTierCountsAsync()`
- Trả số lượng khách cố định/vãng lai.

### c) `GetRevenueSummaryAsync()`
- Tổng số khách + doanh thu.

### d) `GetTodayOccupancyPctAsync()`
- Tỷ lệ lấp đầy sân trong ngày.

### e) `FindCheckoutCustomerAsync(string search)`
- Tìm khách dùng cho checkout theo phone hoặc mã.

---

## 2.6 Report API (Services/ReportService.cs)

### a) `GetTopCourtsAsync()`
### b) `GetTopCourtsAsync(DateTime? fromDateInclusive, DateTime? toDateExclusive)`
- Trả top sân theo doanh thu.

### c) `GetKpisAsync(DateTime fromStart, DateTime toExclusive, int days)`
- KPI kỳ hiện tại và kỳ so sánh.

### d) `GetTrendAsync(DateTime fromStart, DateTime toExclusive, DateTime fromDateInclusive, DateTime toDateInclusive)`
- Trend doanh thu theo ngày.

### e) `GetTopCourtsRevenueAsync(DateTime fromStart, DateTime toExclusive)`
- Dữ liệu pie chart top sân.

---

## 2.7 Data Access API nền tảng (Services/DatabaseHelper.cs)

### a) `GetConnection()`
### b) `ExecuteQuery(...)`
### c) `ExecuteNonQuery(...)`
### d) `ExecuteScalar(...)`
### e) `TryLog(...)`, `TryLogThrottled(...)`

Đây là API hạ tầng dùng lại ở hầu hết service/controller.

Code mẫu thật:
```csharp
public static object ExecuteScalar(string query, params SqlParameter[] parameters)
{
    using (var conn = GetConnection())
    using (var cmd = new SqlCommand(query, conn))
    {
        if (parameters != null && parameters.Length > 0)
            cmd.Parameters.AddRange(parameters);

        conn.Open();
        return cmd.ExecuteScalar();
    }
}
```

---

## 3) API xác thực và session (nội bộ)
Mặc dù nhiều hàm là `internal`, về bản chất vẫn là API nội bộ của app.

- `AuthService.TryLogin(...)`
- `AuthService.TryRegister(...)`
- `AppSession.SignIn(...)`
- `AppSession.SignOut()`
- `AppSession.IsInRole(...)`

Code mẫu thật:
```csharp
internal static bool IsInRole(string role)
{
    if (CurrentUser == null) return false;
    if (string.IsNullOrWhiteSpace(role)) return false;
    return string.Equals(CurrentUser.Role ?? "", role, System.StringComparison.OrdinalIgnoreCase);
}
```

---

## 4) Mẫu gọi API trong luồng thực tế
Ví dụ từ `UCBaoCao`:
```csharp
var topCourts = await _reportService.GetTopCourtsAsync(fromStart, toExclusive);
var kpis = await _reportService.GetKpisAsync(fromStart, toExclusive, days);
```

Ví dụ từ `UCThanhToan` (logic phía service):
```csharp
int invoiceId = _posService.Checkout(
    memberId,
    lines,
    totalAmount,
    discountAmount,
    finalAmount,
    paymentMethod,
    courtName);
```

---

## 5) Đánh giá chất lượng API nội bộ hiện tại
Điểm tốt:
- Tách module rõ: Booking / POS / Report / Customer / Invoice.
- Có transaction cho luồng quan trọng.
- SQL truy vấn được gom vào `SqlQueries` dễ theo dõi.

Điểm cần cải thiện:
1. Chưa có interface chuẩn cho service (khó mock unit test).
2. Chưa có chuẩn response/error object thống nhất giữa các API nội bộ.
3. Một số method có nhiều responsibility (ví dụ Checkout) cần tách nhỏ hơn để dễ test.

---

## 6) Kết luận
API của DemoPick là API nội bộ theo mô hình desktop app, không phải HTTP API.
Tuy vậy, nó đã đủ rõ để:
- học cách thiết kế service layer,
- học cách map UI -> nghiệp vụ -> DB,
- học cách tổ chức transaction và query cho hệ thống vận hành thực tế.
