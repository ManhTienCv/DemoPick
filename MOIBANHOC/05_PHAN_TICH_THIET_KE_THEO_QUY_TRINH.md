# 05 - Phân tích thiết kế bài toán theo đúng quy trình

## 1) Mục tiêu tài liệu
Tài liệu này phân tích thiết kế hệ thống DemoPick theo quy trình phát triển phần mềm chuẩn, để bạn học cách đi từ bài toán -> thiết kế -> cài đặt -> kiểm thử.

Quy trình được trình bày theo 8 bước:
1. Khảo sát và xác định bài toán.
2. Phân tích yêu cầu.
3. Phân tích nghiệp vụ và use case.
4. Thiết kế dữ liệu (Database Design).
5. Thiết kế kiến trúc và module.
6. Thiết kế chi tiết (class, method, transaction, error handling).
7. Kiểm thử và xác nhận.
8. Vận hành và cải tiến.

---

## 2) Bước 1 - Khảo sát và xác định bài toán
### 2.1 Đầu vào khảo sát
- Quy trình đặt sân thực tế.
- Quy trình bán hàng/thanh toán tại quầy.
- Nhu cầu báo cáo doanh thu, top sân, khách hàng.

### 2.2 Kết quả khảo sát
Xác định 4 miền nghiệp vụ cốt lõi:
- Booking Management
- POS/Checkout
- Customer Management
- Reporting/Analytics

---

## 3) Bước 2 - Phân tích yêu cầu
### 3.1 Yêu cầu chức năng
- Đăng nhập/đăng ký/đổi mật khẩu.
- Tạo/sửa/hủy booking.
- Quản lý giỏ hàng theo sân.
- Checkout sinh hóa đơn + trừ kho + cập nhật khách hàng.
- Xem KPI/báo cáo theo thời gian.

### 3.2 Yêu cầu phi chức năng
- Dữ liệu nhất quán khi checkout (transaction).
- Chống trùng booking.
- Hỗ trợ migration database.
- Bảo mật đăng nhập cơ bản (hash/salt + lockout).

---

## 4) Bước 3 - Phân tích nghiệp vụ và Use Case
### 4.1 Use Case chính
1. UC-01: User Login
2. UC-02: Create Booking
3. UC-03: Manage POS Cart
4. UC-04: Checkout Invoice
5. UC-05: View Dashboard/Reports

### 4.2 Luồng nghiệp vụ Checkout (trọng tâm)
- Actor: Thu ngân/nhân viên.
- Pre-condition: Đăng nhập thành công, có giỏ hàng hoặc booking cần thanh toán.
- Main flow:
1. Hệ thống nhận danh sách cart lines.
2. Hệ thống xác định booking liên quan (theo sân).
3. Hệ thống tạo invoice + invoice details.
4. Hệ thống cập nhật stock và dữ liệu hội viên.
5. Hệ thống ghi log.
6. Commit giao dịch.

- Exception flow:
- Thiếu dữ liệu thanh toán -> rollback.
- Lỗi DB -> rollback và báo lỗi.

Code mẫu thật (transaction) trong `Services/PosService.cs`:
```csharp
using (var tran = conn.BeginTransaction(IsolationLevel.ReadCommitted))
{
    // nghiệp vụ nhiều bước
    tran.Commit();
}
```

---

## 5) Bước 4 - Thiết kế dữ liệu (Database Design)
### 5.1 Mô hình dữ liệu
Bảng lõi: Courts, Members, Bookings, Products, Invoices, InvoiceDetails, StaffAccounts, SystemLogs.

### 5.2 Ràng buộc dữ liệu
- FK đảm bảo toàn vẹn tham chiếu.
- Unique index cho SKU và Username.
- Trigger tự động hỗ trợ nghiệp vụ hậu checkout.

Code mẫu thật (FK) từ SQL:
```sql
ALTER TABLE dbo.InvoiceDetails
ADD CONSTRAINT FK_InvoiceDetails_Invoices
FOREIGN KEY (InvoiceID) REFERENCES dbo.Invoices(InvoiceID);
```

### 5.3 Tiến hóa schema
- Sử dụng migration script theo định danh tăng dần `NNNN__*.sql`.
- Lưu checksum migration đã chạy.

---

## 6) Bước 5 - Thiết kế kiến trúc và module
### 6.1 Kiến trúc logic
- Presentation: `Views/*`
- Application orchestration: `Controllers/*`
- Business + data access: `Services/*`
- Data model: `Models/*`
- SQL scripts: `Database/*`

### 6.2 Lý do chọn cấu trúc này
- Dễ tách logic khỏi UI WinForms.
- Dễ debug luồng nghiệp vụ.
- Dễ mở rộng thêm module theo service.

Code mẫu thật (dependency từ UI sang service):
```csharp
private readonly CustomerService _customerService;

public UCThanhToan()
{
    InitializeComponent();
    _customerService = new CustomerService();
}
```

---

## 7) Bước 6 - Thiết kế chi tiết
### 7.1 Thiết kế đặt lịch an toàn trùng giờ
- Xử lý bằng stored procedure tại DB, không chỉ kiểm tra ở UI.

### 7.2 Thiết kế logging an toàn
- Logging best-effort không làm crash app.

Code mẫu thật trong `Services/DatabaseHelper.cs`:
```csharp
public static void TryLog(string eventDesc, string subDesc)
{
    try
    {
        ExecuteNonQuery(
            "INSERT INTO SystemLogs (EventDesc, SubDesc) VALUES (@EventDesc, @SubDesc)",
            new SqlParameter("@EventDesc", eventDesc ?? ""),
            new SqlParameter("@SubDesc", (object)subDesc ?? DBNull.Value)
        );
    }
    catch
    {
        // logging must never break the app
    }
}
```

### 7.3 Thiết kế phân quyền giao diện
- Role-based module enable/disable trong `FrmChinh`.
- Tránh truy cập module ngoài quyền ở mức UI.

---

## 8) Bước 7 - Kiểm thử và xác nhận
### 8.1 Smoke test tự động
Dự án có `SmokeTestRunner` để chạy kiểm thử tích hợp mức cơ bản:
- DB init
- Login
- Create booking + cleanup
- Logic tests
- Performance micro-benchmark

Code mẫu thật:
```csharp
int code = SmokeTestRunner.Run(args ?? Array.Empty<string>());
Environment.ExitCode = code;
```

### 8.2 Tiêu chí pass
- Không lỗi DB schema/migration.
- Login hoạt động.
- Booking không trùng lịch.
- Checkout và báo cáo hoạt động.

---

## 9) Bước 8 - Vận hành và cải tiến
### 9.1 Vận hành
- Quản lý connection string qua config/env.
- Theo dõi log qua `SystemLogs`.

### 9.2 Cải tiến đề xuất
1. Bổ sung lớp repository rõ ràng hơn cho từng domain.
2. Chuẩn hóa exception type theo nghiệp vụ.
3. Bổ sung test tự động cho từng service quan trọng.
4. Chuẩn hóa naming tiếng Việt/Anh trong DB để dễ maintenance lâu dài.

---

## 10) Tóm tắt thiết kế theo quy trình
- Bài toán đã được phân tích đúng hướng nghiệp vụ thực tế.
- Thiết kế dữ liệu và module đủ rõ để mở rộng.
- Luồng giao dịch chính được bảo vệ bằng transaction và constraint.
- Có cơ chế migration + smoke test hỗ trợ vòng đời phát triển.

Đây là một case học tốt để nắm quy trình phân tích thiết kế từ đầu đến cuối trên một dự án thực tế.
