# 2026-04-18 - UI Refactor Priority Matrix (Effort/Risk)

## Muc tieu
- Giam code-built UI trong cac page nang, uu tien UCThanhToan va UCBanHang.
- Tach thanh custom control co Designer de de bao tri, giam loi wiring va giam rui ro break Designer.
- Giu nguyen hanh vi nghiep vu (khong doi ket qua thanh toan, in hoa don, luu don cho).

## Cach cham diem
- Effort:
  - S: 0.5-1.5 ngay cong
  - M: 2-3 ngay cong
  - L: 4-6 ngay cong
- Risk:
  - Low: Anh huong cuc bo, de rollback
  - Medium: Anh huong lien ket 1 module lon
  - High: Anh huong luong thanh toan/ban hang, nhieu state phu thuoc
- Priority:
  - P0: Lam truoc trong dot refactor dau tien
  - P1: Lam ngay sau P0 khi regression da on
  - P2: Hoan thien sau cung

## Bang uu tien refactor

| Priority | Hang muc | Hien trang | Effort | Risk | Gia tri | De xuat tach |
|---|---|---|---|---|---|---|
| P0 | UCThanhToan - Reprint panel | Tao UITextBox/UIButton bang code va add vao pnlTotals | S | Low | Cao | Tao `UCInvoiceReprintPanel` (Designer + event contract), UCThanhToan chi bind du lieu va xu ly callback |
| P0 | UCThanhToan - Payment history panel | Tao ListView + button + search runtime trong `pnlRight` | M | Medium | Cao | Tao `UCPaymentHistoryPanel` (Designer), expose `SearchRequested`, `InvoiceOpenRequested`, method `BindHistory(...)` |
| P0 | FrmXoaSP | Form dang code-built gan nhu 100% (khong co Designer file) | M | Low | Trung binh/Cao | Tao Designer cho FrmXoaSP, giu logic xoa o code-behind, tach setup control khoi constructor |
| P1 | UCBanHang - Courts cards | Tao `Panel/Label` dong trong loop, click wiring tren nhieu control | M | Medium | Cao | Tao `UCCourtPosCard` (Designer) + model bind, event `CourtSelected` |
| P1 | UCBanHang - Product cards | Tao card san pham runtime va add vao flow layout | L | Medium | Rat cao | Tao `UCProductCard` (Designer), cache/reuse control de giam create/dispose hang loat |
| P1 | UCThanhToan - Courts cards | Tao card san thanh toan runtime, co luong tiep nhan san va state thanh toan | L | High | Rat cao | Tao `UCCourtPaymentCard` + state model, UCThanhToan chi dieu phoi state |
| P1 | UCBanHang - Prompt quantity dialog | Dung `new Form()` inline de sua so luong | S | Low | Trung binh | Tao `FrmEditQuantity` co Designer de tai su dung va de test UI |
| P2 | FrmChinh - host wiring | Chu yeu event wiring/presenter-like logic trong Form | M | Medium | Trung binh | Dua dieu huong module vao class mediator/presenter, Form chi giu host rendering |
| P2 | UCDateRangeFilter - layout by code | Nhieu set `Location/Size` trong behaviors | M | Medium | Trung binh | Gom layout constants + helper, giam hard-code pixel, uu tien Anchor/Dock khi co the |

## Lo trinh de xuat (3 dot)

### Dot 1 (P0) - On dinh nhanh, rui ro thap
1. Tach `UCInvoiceReprintPanel`.
2. Tach `UCPaymentHistoryPanel`.
3. Chuyen `FrmXoaSP` sang Designer-first.

Dieu kien qua cong doan:
- Build Debug xanh.
- Chuc nang in lai theo ma, in lai hoa don vua xong, mo tu lich su van dung dung.
- Khong tang loi giao dien khi mo/dong form lap lai.

### Dot 2 (P1) - Xu ly diem nong code-built lon
1. Tach `UCCourtPosCard`.
2. Tach `UCProductCard`.
3. Tach `UCCourtPaymentCard`.
4. Tao `FrmEditQuantity` thay cho dialog inline.

Dieu kien qua cong doan:
- Luong chon san -> them hang -> luu don cho van giu nguyen.
- Luong chon san -> tinh tien -> checkout van giu nguyen.
- Khong leak control khi reload danh sach (dispose dung cach).

### Dot 3 (P2) - Hoan thien kien truc host
1. Tach mediator/presenter cho `FrmChinh`.
2. Chuan hoa layout logic cua `UCDateRangeFilter`.

Dieu kien qua cong doan:
- Giam do phuc tap code-behind o host pages.
- Code de doc hon, giam wiring lap.

## Task order chi tiet theo effort/risk
1. `UCInvoiceReprintPanel` (S/Low)
2. `FrmEditQuantity` (S/Low)
3. `FrmXoaSP` Designer-first (M/Low)
4. `UCPaymentHistoryPanel` (M/Medium)
5. `UCCourtPosCard` (M/Medium)
6. `UCDateRangeFilter` layout cleanup (M/Medium)
7. `FrmChinh` mediator extraction (M/Medium)
8. `UCProductCard` (L/Medium)
9. `UCCourtPaymentCard` (L/High)

## Ghi chu ky thuat can giu
- Khong dua business logic/DB query vao Form/UserControl.
- Bat buoc unsubscribe event khi dispose neu control song dai hon host.
- Uu tien Designer + custom control de han che parser break voi Sunny.UI.
- Kiem soat tao/huy control trong flow panels de tranh memory growth.