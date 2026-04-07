# Smoke Test – Maintenance Booking + Checkout + Metrics

Mục tiêu: đảm bảo **bảo trì (Status = Maintenance)** chỉ **block lịch**, **không được tính doanh thu/occupancy**, và **không bị đưa vào luồng thanh toán**.

## Chuẩn bị
- Dùng tài khoản có quyền thao tác đặt lịch và xem Dashboard/Báo cáo (Admin/Staff).
- Đảm bảo DB kết nối OK (app chạy được và thấy dữ liệu sân).

## 1) Đặt bảo trì (fixed/recurring) và kiểm tra hiển thị
1. Vào **Đặt lịch** → mở form **Đặt sân cố định / Bảo trì**.
2. Chọn sân, chọn khoảng ngày, chọn thứ, chọn giờ bắt đầu + thời lượng.
3. Chọn mode **Bảo trì** và bấm tạo.

Kỳ vọng:
- Lịch hiển thị block **màu đỏ** (Maintenance).
- Nếu mở lại **Đặt lịch** thì các lần lặp đã được lưu (tồn tại sau khi đóng/mở app).

## 2) Thanh toán: bảo trì không được xuất hiện như “đang chơi/cần thanh toán”
1. Vào **Thanh toán**.
2. Quan sát danh sách sân cần thanh toán.

Kỳ vọng:
- Sân đang ở block **Maintenance** không xuất hiện như “Đang chơi”.
- Không thể “đóng sân”/chuyển Maintenance sang Paid do thao tác thanh toán.

## 3) Thanh toán “chỉ tiền sân” (không sản phẩm) vẫn tạo hóa đơn OK
1. Tạo 1 booking bình thường (Confirmed) trong khung giờ hiện tại (đang chơi).
2. Vào **Thanh toán**, chọn sân đó.
3. Không thêm sản phẩm, chỉ thanh toán tiền sân.

Kỳ vọng:
- Checkout thành công.
- Hóa đơn hiển thị 1 dòng **“Tiền sân”** (line có `ProductID = NULL` và có `BookingID`).

## 4) Thanh toán có sản phẩm + tiền sân: trừ kho và hóa đơn đúng
1. Tạo/đảm bảo có booking bình thường đang chơi.
2. Vào **Bán hàng** thêm 1–2 sản phẩm vào giỏ của sân.
3. Qua **Thanh toán** checkout.

Kỳ vọng:
- Sản phẩm bị trừ kho (nếu hệ thống đang bật trừ kho/trigger).
- Hóa đơn có các dòng sản phẩm + dòng **“Tiền sân”**.

## 5) Dashboard/Báo cáo: bảo trì không làm tăng doanh thu/occupancy
1. Ghi lại 2 số trước khi tạo bảo trì:
   - Dashboard: Tổng doanh thu, Occupancy.
   - Báo cáo: KPI doanh thu/occupancy trong range bạn chọn.
2. Tạo thêm 1–2 block **Maintenance** trong range đó.
3. Refresh Dashboard/Báo cáo.

Kỳ vọng:
- Doanh thu/occupancy **không tăng** chỉ vì Maintenance.
- Top sân theo doanh thu/giờ đặt không bị đội lên do Maintenance.

## Ghi chú khi test
- Nếu cần test nhanh tác động số liệu: tạo Maintenance 2–3 giờ trong hôm nay, sau đó refresh Dashboard/Báo cáo.
- Nếu có dữ liệu booking bình thường cùng ngày, doanh thu vẫn có thể thay đổi theo booking bình thường; chỉ kiểm tra rằng Maintenance **không góp phần** vào số đó.
