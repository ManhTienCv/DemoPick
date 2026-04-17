using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DemoPick.Models;
using DemoPick.Services;
using Sunny.UI;

namespace DemoPick
{
    public partial class UCThanhToan
    {
        private void SetupListView()
        {
            lstCart.Columns.Add("Sản phẩm", 140);
            lstCart.Columns.Add("SL", 40);
            lstCart.Columns.Add("Thành tiền", 110);
        }

        private void UpdateTotals()
        {
            lstCart.Items.Clear();
            _cartTotal = 0;

            decimal fixedDiscountAmtAmount = 0;

            if (_currentBooking != null)
            {
                // Run PriceCalculator
                var services = new System.Collections.Generic.List<DemoPick.Services.ServiceCharge>();
                var pendingLines = PosService.GetPendingOrder(_selectedCourtName);

                foreach (var pl in pendingLines)
                {
                    // By default: quantity-based services. Some add-ons are per-hour.
                    string name = pl.ProductName ?? "";
                    string unit = PriceCalculator.GuessServiceUnit(name);

                    services.Add(new DemoPick.Services.ServiceCharge
                    {
                        ProductID = pl.ProductId,
                        ServiceName = name,
                        Quantity = pl.Quantity,
                        UnitPrice = pl.UnitPrice,
                        Unit = unit
                    });
                }

                decimal courtMultiplier = PriceCalculator.GetCourtRateMultiplier(_selectedCourt?.CourtType, _selectedCourt?.Name);

                var breakdown = DemoPick.Services.PriceCalculator.CalculateTotal(_currentBooking.StartTime, _currentBooking.EndTime, _isFixedCustomer, services, courtMultiplier);

                foreach (var ts in breakdown.TimeSlots)
                {
                    var lviCourt = new ListViewItem(new[] { "Giờ sân " + ts.Description, $"{ts.Hours:0.##}h", ts.Total.ToString("N0") + "đ" });
                    lviCourt.Tag = new CartLine(-1, "Giờ sân " + ts.Description, 1, ts.Total);
                    lviCourt.ForeColor = Color.DarkBlue;
                    lstCart.Items.Add(lviCourt);
                }

                foreach (var svc in breakdown.Services)
                {
                    var lvi = new ListViewItem(new[] { svc.ServiceName, svc.Quantity.ToString(), svc.Total.ToString("N0") + "đ" });
                    lvi.Tag = new CartLine(svc.ProductID, svc.ServiceName, svc.Quantity, svc.UnitPrice);
                    lstCart.Items.Add(lvi);
                }

                _cartTotal = breakdown.SubtotalCourts + breakdown.SubtotalServices;
                fixedDiscountAmtAmount = breakdown.DiscountAmount;
            }
            else
            {
                var pendingLines = PosService.GetPendingOrder(_selectedCourtName);
                foreach (var line in pendingLines)
                {
                    var lvi = new ListViewItem(new[] { line.ProductName, line.Quantity.ToString(), (line.UnitPrice * line.Quantity).ToString("N0") + "đ" });
                    lvi.Tag = line;
                    lstCart.Items.Add(lvi);
                    _cartTotal += (line.UnitPrice * line.Quantity);
                }
            }

            lblSubTotalV.Text = _cartTotal.ToString("N0") + "đ";

            // Normal discount % + fixed amount discount
            decimal discountAmt = (_cartTotal * _currentDiscountPct) + fixedDiscountAmtAmount;
            lblDiscountV.Text = "-" + discountAmt.ToString("N0") + "đ";

            decimal finalTotal = _cartTotal - discountAmt;
            if (finalTotal < 0) finalTotal = 0;

            _lastDiscountAmount = discountAmt;
            _lastFinalTotal = finalTotal;

            lblTotalV.Text = finalTotal.ToString("N0") + "đ";
            UpdateMockInvoicePreview(_cartTotal, discountAmt, finalTotal);
        }

        private void UpdateMockInvoicePreview(decimal subtotal, decimal discountAmount, decimal finalTotal)
        {
            if (lblPreviewDesc == null || lblPreviewTotal == null || pnlMockInvoice == null)
            {
                return;
            }

            lblPreviewDesc.AutoSize = false;
            lblPreviewDesc.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblPreviewTotal.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;

            lblPreviewDesc.Width = Math.Max(120, pnlMockInvoice.Width - 30);
            int usableHeight = Math.Max(120, lblPreviewTotal.Top - lblPreviewDesc.Top - 10);
            lblPreviewDesc.Height = usableHeight;

            var sb = new StringBuilder();
            sb.AppendLine("   GREEN COURT");
            sb.AppendLine("----------------------------");
            sb.AppendLine("HÓA ĐƠN THANH TOÁN");
            sb.AppendLine(" ");

            string courtName = string.IsNullOrWhiteSpace(_selectedCourtName) ? "-" : _selectedCourtName;
            sb.AppendLine("Sân  : " + TrimPreview(courtName, 18));

            if (_currentBooking != null)
            {
                sb.AppendLine($"Ca   : {_currentBooking.StartTime:HH:mm}-{_currentBooking.EndTime:HH:mm}");
            }

            sb.AppendLine("Khách: " + TrimPreview(GetPreviewCustomerName(), 18));
            sb.AppendLine("----------------------------");

            int maxRows = 7;
            if (lstCart.Items.Count == 0)
            {
                sb.AppendLine("(Chưa có dòng tính tiền)");
            }
            else
            {
                int visibleRows = Math.Min(maxRows, lstCart.Items.Count);
                for (int i = 0; i < visibleRows; i++)
                {
                    ListViewItem item = lstCart.Items[i];
                    string name = TrimPreview(item.SubItems.Count > 0 ? item.SubItems[0].Text : "-", 12);
                    string qty = TrimPreview(item.SubItems.Count > 1 ? item.SubItems[1].Text : "1", 4);
                    string amount = TrimPreview(item.SubItems.Count > 2 ? item.SubItems[2].Text : "0đ", 9);
                    sb.AppendLine($"{name,-12} x{qty,-4} {amount}");
                }

                int hiddenRows = lstCart.Items.Count - visibleRows;
                if (hiddenRows > 0)
                {
                    sb.AppendLine($"+ {hiddenRows} dòng khác...");
                }
            }

            sb.AppendLine("----------------------------");
            sb.AppendLine($"Tạm tính: {subtotal,10:N0}đ");
            sb.AppendLine($"Giảm giá: -{discountAmount,9:N0}đ");

            lblPreviewDesc.Text = sb.ToString();
            lblPreviewTotal.Text = finalTotal.ToString("N0") + "đ";
        }

        private string GetPreviewCustomerName()
        {
            if (_currentCustomerId <= 0)
            {
                return "Khách lẻ";
            }

            string infoText = (lblCustomerInfo == null ? string.Empty : lblCustomerInfo.Text) ?? string.Empty;
            infoText = infoText.Trim();
            if (string.IsNullOrEmpty(infoText))
            {
                return "Khách thành viên";
            }

            if (infoText.StartsWith("✓ "))
            {
                infoText = infoText.Substring(2).Trim();
            }

            int tierStart = infoText.IndexOf(" (", StringComparison.Ordinal);
            if (tierStart > 0)
            {
                infoText = infoText.Substring(0, tierStart).Trim();
            }

            return string.IsNullOrWhiteSpace(infoText) ? "Khách thành viên" : infoText;
        }

        private static string TrimPreview(string input, int maxLen)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "-";
            }

            string value = input.Trim().Replace("\r", " ").Replace("\n", " ");
            if (value.Length <= maxLen)
            {
                return value;
            }

            if (maxLen <= 1)
            {
                return value.Substring(0, 1);
            }

            return value.Substring(0, maxLen - 1) + "~";
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ResetCheckoutPane();
        }

        private void BtnCheckout_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedCourtName))
            {
                new UIPage().ShowWarningTip("Vui lòng chọn sân cần thanh toán.");
                return;
            }

            if (_currentBooking != null && DateTime.Now < _currentBooking.StartTime)
            {
                MessageBox.Show(
                    "Booking chưa tới giờ chơi. Hãy bấm 'Nhận sân' để check-in, rồi thanh toán khi ca đã bắt đầu.",
                    "Chưa tới giờ thanh toán",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            // Prompt user with yes/no dialog equivalent.
            // In a pro UI, we'd use Sunny UI Form.
            var diagRet = MessageBox.Show($"Xác nhận Thu tiền và In Hóa Đơn cho {_selectedCourtName}?", "Thanh toán", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (diagRet == DialogResult.No) return;

            decimal discountAmt = _lastDiscountAmount;
            decimal finalTotal = _lastFinalTotal;

            try
            {
                var lines = new System.Collections.Generic.List<CartLine>();
                foreach (ListViewItem item in lstCart.Items)
                {
                    if (item.Tag is CartLine line)
                    {
                        lines.Add(line);
                    }
                }

                var pos = new PosService();
                int? preferredBookingId = (_currentBooking != null && _currentBooking.BookingID > 0)
                    ? (int?)_currentBooking.BookingID
                    : null;

                int invoiceId = pos.Checkout(
                    _currentCustomerId,
                    lines,
                    _cartTotal,
                    discountAmt,
                    finalTotal,
                    "Cash",
                    _selectedCourtName,
                    preferredBookingId
                );

                _lastCompletedInvoiceId = invoiceId;
                _lastCompletedCourtName = _selectedCourtName ?? string.Empty;
                if (_txtReprintInvoiceId != null)
                {
                    _txtReprintInvoiceId.Text = invoiceId.ToString();
                }
                UpdateReprintButtonState();

                ShowInvoicePreview(invoiceId, _lastCompletedCourtName);

                PosService.ClearPendingOrder(_selectedCourtName);
                new UIPage().ShowSuccessTip($"Thanh toán hoàn tất! HĐ #{invoiceId}");
                RefreshOnActivated();

            }
            catch (Exception ex)
            {
                new UIPage().ShowErrorTip("Lỗi: " + ex.Message);
            }
        }
    }
}
