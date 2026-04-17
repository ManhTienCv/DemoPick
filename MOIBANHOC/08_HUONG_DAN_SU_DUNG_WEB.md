# 08 - Hướng dẫn sử dụng web/hệ thống

## 1) Ghi chú trước khi học
Dự án DemoPick hiện tại triển khai bằng WinForms (desktop), chưa phải website chạy trình duyệt.

Tuy nhiên, luồng sử dụng module được tổ chức tương tự một web admin portal, nên tài liệu này trình bày theo góc nhìn "sử dụng web/hệ thống" để bạn dễ học quy trình nghiệp vụ.

---

## 2) Luồng sử dụng tổng quan
1. Mở ứng dụng.
2. Đăng nhập tài khoản.
3. Vào trang chính (Dashboard).
4. Chọn module theo menu trái:
- Đặt lịch
- Bán hàng POS
- Khách hàng
- Kho hàng
- Báo cáo
- Thanh toán
5. Thực hiện nghiệp vụ.
6. Đăng xuất.

Code mẫu thật (điều hướng module) trong `Views/FrmChinh.cs`:
```csharp
public void SwitchModule(UserControl uc, Sunny.UI.UIPanel activeBtn, string title, string subtitle)
{
    pnlContent.Controls.Clear();
    pnlContent.Controls.Add(uc);
    lblPageTitle.Text = title;
    lblPageSubtitle.Text = subtitle;
}
```

---

## 3) Chuẩn bị trước khi sử dụng
### 3.1 Cấu hình DB
- Kiểm tra `App.config` -> connection string `DefaultConnection`.
- Đảm bảo SQL Server đang chạy.

### 3.2 Lần chạy đầu tiên
Ứng dụng tự tạo schema/migration khi khởi động.

Code mẫu thật trong `Program.cs`:
```csharp
SchemaInstaller.EnsureDatabaseAndSchema();
MigrationsRunner.ApplyPendingMigrations();
```

---

## 4) Hướng dẫn thao tác theo chức năng

## 4.1 Đăng nhập
Bước thực hiện:
1. Nhập tài khoản (username/email/phone/fullname).
2. Nhập mật khẩu.
3. Bấm đăng nhập.

Nếu sai nhiều lần, hệ thống có lockout tạm thời.

## 4.2 Dashboard (Tổng quan)
Chức năng:
- Xem KPI doanh thu.
- Xem tỷ lệ lấp đầy sân.
- Xem top sân.

Mục đích:
- Nắm trạng thái vận hành nhanh trong ngày/tuần.

## 4.3 Đặt lịch sân
Bước thực hiện:
1. Vào module Đặt lịch.
2. Chọn ngày.
3. Chọn sân và khung giờ.
4. Nhập thông tin khách/ghi chú.
5. Lưu booking.

Lưu ý:
- Hệ thống tự chặn trùng lịch theo sân và thời gian.

## 4.4 POS/Bán hàng
Bước thực hiện:
1. Vào module Bán hàng.
2. Chọn sân/booking liên quan.
3. Thêm sản phẩm/dịch vụ vào giỏ.
4. Chỉnh số lượng.
5. Chuyển sang thanh toán.

Lưu ý:
- Mỗi sân có thể giữ giỏ hàng tạm riêng.

## 4.5 Thanh toán
Bước thực hiện:
1. Kiểm tra lại thông tin khách hàng.
2. Kiểm tra tổng tiền và giảm giá.
3. Chọn phương thức thanh toán.
4. Bấm checkout để tạo hóa đơn.

Sau khi checkout:
- Sinh invoice + invoice detail.
- Trừ kho với hàng hóa.
- Cập nhật dữ liệu hội viên.

## 4.6 Khách hàng
Chức năng:
- Tra cứu khách.
- Xem phân loại cố định/vãng lai.
- Theo dõi tổng chi tiêu và giờ đã mua.

## 4.7 Kho hàng
Chức năng:
- Quản lý danh mục sản phẩm.
- Theo dõi tồn kho và mức cảnh báo.
- Xem một phần log giao dịch liên quan POS.

## 4.8 Báo cáo
Bước thực hiện:
1. Chọn khoảng thời gian.
2. Bấm Apply.
3. Xem KPI, trend, top sân.

Lưu ý:
- Có thể double click top sân để điều hướng sang module đặt lịch.

---

## 5) Quy tắc sử dụng theo vai trò
- Admin: toàn quyền.
- Staff: được cấp các module tác nghiệp.

Nếu tài khoản không có quyền, hệ thống sẽ vô hiệu một số menu.

---

## 6) Các lỗi thường gặp và cách xử lý

### Lỗi 1: Không kết nối được DB
Triệu chứng:
- App báo lỗi CSDL khi khởi động.

Cách xử lý:
1. Kiểm tra SQL Server service.
2. Kiểm tra server/database trong connection string.
3. Kiểm tra quyền kết nối.

### Lỗi 2: Không đặt được sân
Triệu chứng:
- Báo sân đã được đặt.

Cách xử lý:
1. Chọn khung giờ khác.
2. Kiểm tra booking hiện có trong ngày.

### Lỗi 3: Checkout thất bại
Triệu chứng:
- Báo lỗi khi tạo hóa đơn.

Cách xử lý:
1. Kiểm tra dữ liệu giỏ hàng có hợp lệ.
2. Kiểm tra dữ liệu sản phẩm và tồn kho.
3. Kiểm tra trạng thái booking liên quan.

---

## 7) Quy trình thao tác mẫu (end-to-end)
Scenario mẫu:
1. Đăng nhập -> Dashboard.
2. Vào Đặt lịch tạo booking sân 1 lúc 18:00-19:00.
3. Vào Bán hàng thêm 2 chai nước.
4. Vào Thanh toán checkout đơn.
5. Vào Báo cáo kiểm tra doanh thu đã tăng.

Đây là luồng học tốt nhất để hiểu toàn bộ hệ thống.

---

## 8) Mẹo học nhanh cho người mới
1. Học từng module, không học dàn trải.
2. Luôn đối chiếu thao tác UI với dữ liệu trong DB.
3. Đọc thêm các file trong `MOIBANHOC` theo thứ tự:
- cấu trúc SQL
- logic dự án
- API nội bộ
- bảo mật

---

## 9) Kết luận
Dù là desktop app, DemoPick có luồng sử dụng rất gần một web quản trị nội bộ:
- đăng nhập,
- điều hướng module,
- thao tác dữ liệu,
- xem dashboard/báo cáo.

Bạn hoàn toàn có thể dùng tài liệu này để học quy trình vận hành một hệ thống quản lý nghiệp vụ thực tế.
