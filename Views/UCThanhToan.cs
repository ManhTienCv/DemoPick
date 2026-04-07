using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DemoPick.Services;
using Sunny.UI;

namespace DemoPick
{
    public partial class UCThanhToan : UserControl
    {
        private decimal _cartTotal = 0;
        private string _selectedCourtName = "";
        private decimal _currentDiscountPct = 0;
        private int _currentCustomerId = 0;
        private bool _isFixedCustomer = false;
        private DemoPick.Models.BookingModel _currentBooking;
        private DemoPick.Models.CourtModel _selectedCourt;
        private decimal _lastDiscountAmount = 0m;
        private decimal _lastFinalTotal = 0m;

        public UCThanhToan()
        {
            InitializeComponent();
            if (DesignModeUtil.IsDesignMode(this)) return;

            SetupListView();
            btnSearchCustomer.Click += BtnSearchCustomer_Click;
            btnCheckout.Click += BtnCheckout_Click;
            btnCancel.Click += BtnCancel_Click;
        }

        private void SetupListView()
        {
            lstCart.Columns.Add("Sản phẩm", 140);
            lstCart.Columns.Add("SL", 40);
            lstCart.Columns.Add("Thành tiền", 110);
        }

        public void RefreshOnActivated()
        {
            LoadCourts();
            ResetCheckoutPane();
        }

        private void ResetCheckoutPane()
        {
            _cartTotal = 0;
            _currentDiscountPct = 0;
            _currentCustomerId = 0;
            txtCustomerPhone.Text = "";
            lblCustomerInfo.Text = "Khách lẻ (Không áp dụng thẻ)";
            lblCustomerInfo.ForeColor = Color.Gray;
            lstCart.Items.Clear();
            lblRightTitle.Text = "Hóa đơn thanh toán";
            _selectedCourtName = "";
            _lastDiscountAmount = 0m;
            _lastFinalTotal = 0m;
            UpdateTotals();
        }

        private void LoadCourts()
        {
            try
            {
                flpCourts.Controls.Clear();
                var bookCtrl = new DemoPick.Controllers.BookingController();
                var courts = bookCtrl.GetCourts();

                foreach (var c in courts)
                {
                    // Check if court has pending order or is actively playing.
                    var lines = PosService.GetPendingOrder(c.Name);
                    bool hasOrder = lines.Count > 0;
                    
                    var bookings = bookCtrl.GetBookingsByDate(DateTime.Now);
                        var currentBooking = bookings.Find(b =>
                            b.CourtID == c.CourtID &&
                            !string.Equals(b.Status, "Maintenance", StringComparison.OrdinalIgnoreCase) &&
                            DateTime.Now >= b.StartTime && DateTime.Now <= b.EndTime);
                    bool active = currentBooking != null;

                    if (!hasOrder && !active) continue; // Only show courts that need to be checked out

                    string statusTxt = hasOrder ? "Có order" : (active ? "Đang chơi" : "");
                    Color lineCol = hasOrder ? Color.FromArgb(231, 76, 60) : Color.FromArgb(76, 175, 80);

                    Panel pnlCtx = new Panel { Size = new Size(240, 80), BackColor = Color.White, Margin = new Padding(0, 0, 0, 10), Cursor = Cursors.Hand };
                    pnlCtx.Paint += (s, e) =>
                    {
                        e.Graphics.DrawRectangle(new Pen(Color.FromArgb(229, 231, 235), 1), 0, 0, pnlCtx.Width - 1, pnlCtx.Height - 1);
                        e.Graphics.FillRectangle(new SolidBrush(lineCol), 0, 10, 4, pnlCtx.Height - 20);
                    };

                    Label cName = new Label { Text = c.Name, Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = Color.FromArgb(26, 35, 50), Location = new Point(15, 15), AutoSize = true };
                    Label badge = new Label { Text = statusTxt, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.White, BackColor = lineCol, Location = new Point(150, 17), AutoSize = true, Padding = new Padding(2) };
                    Label cOrderInfo = new Label { Text = $"Đỏ: {lines.Count} món đang chờ.", Font = new Font("Segoe UI", 9F, FontStyle.Italic), ForeColor = Color.Gray, Location = new Point(15, 45), AutoSize = true };

                    pnlCtx.Controls.AddRange(new Control[] { cName, badge, cOrderInfo });

                    EventHandler selectCourt = (s, e) =>
                    {
                        foreach (Control p in flpCourts.Controls) { if (p is Panel panel) panel.BackColor = Color.White; }
                        pnlCtx.BackColor = Color.FromArgb(235, 248, 235);
                        SelectCourtToCheckout(c, currentBooking);
                    };

                    pnlCtx.Click += selectCourt;
                    cName.Click += selectCourt;
                    badge.Click += selectCourt;
                    cOrderInfo.Click += selectCourt;

                    flpCourts.Controls.Add(pnlCtx);
                }
                
                if (flpCourts.Controls.Count == 0)
                {
                    flpCourts.Controls.Add(new Label { Text = "Không có sân nào đang cần thanh toán", Font = new Font("Segoe UI", 11F, FontStyle.Italic), AutoSize = true, Margin = new Padding(20), ForeColor = Color.Gray });
                }
            }
            catch (Exception ex)
            {
                DatabaseHelper.TryLog("ThanhToan Load Courts Error", ex, "UCThanhToan.LoadCourts");
            }
        }

        private void SelectCourtToCheckout(DemoPick.Models.CourtModel court, DemoPick.Models.BookingModel booking)
        {
            string courtName = court.Name;
            _selectedCourtName = courtName;
            lblRightTitle.Text = "Hóa đơn - " + courtName;
            
            _currentBooking = booking;
            _selectedCourt = court;
            _cartTotal = 0;

            UpdateTotals();
        }

        private void BtnSearchCustomer_Click(object sender, EventArgs e)
        {
            string search = txtCustomerPhone.Text.Trim();
            if (string.IsNullOrEmpty(search))
            {
                _currentDiscountPct = 0;
                _currentCustomerId = 0;
                _isFixedCustomer = false;
                lblCustomerInfo.Text = "Khách lẻ (Không áp dụng thẻ)";
                lblCustomerInfo.ForeColor = Color.Gray;
                UpdateTotals();
                return;
            }

            string qid = search.Replace("#PB", "").Trim();
            try
            {
                var dt = DatabaseHelper.ExecuteQuery(
                    "SELECT MemberID, FullName, Tier, IsFixed FROM Members WHERE Phone = @Phone OR CAST(MemberID as VARCHAR(20)) = @Qid",
                    new SqlParameter("@Phone", search),
                    new SqlParameter("@Qid", qid)
                );

                if (dt.Rows.Count > 0)
                {
                    _currentCustomerId = Convert.ToInt32(dt.Rows[0]["MemberID"]);
                    string name = dt.Rows[0]["FullName"].ToString();
                    string tier = (dt.Rows[0]["Tier"] == DBNull.Value ? "" : dt.Rows[0]["Tier"].ToString()).ToLowerInvariant();
                    
                    _isFixedCustomer = false;
                    if (dt.Columns.Contains("IsFixed") && dt.Rows[0]["IsFixed"] != DBNull.Value)
                    {
                        _isFixedCustomer = Convert.ToBoolean(dt.Rows[0]["IsFixed"]);
                    }

                    if (tier.Contains("vip") || tier.Contains("vàng") || tier == "gold")
                    {
                        _currentDiscountPct = 0.10m;
                        lblCustomerInfo.Text = $"✓ {name} (VIP). Giảm 10%.";
                        lblCustomerInfo.ForeColor = Color.FromArgb(255, 160, 0);
                    }
                    else if (tier.Contains("bạc") || tier == "silver")
                    {
                        _currentDiscountPct = 0.05m;
                        lblCustomerInfo.Text = $"✓ {name} (Bạc). Giảm 5%.";
                        lblCustomerInfo.ForeColor = Color.FromArgb(76, 175, 80);
                    }
                    else
                    {
                        _currentDiscountPct = 0.02m;
                        lblCustomerInfo.Text = $"✓ {name} (Đồng). Giảm 2%.";
                        lblCustomerInfo.ForeColor = Color.FromArgb(31, 41, 55);
                    }
                    if (_isFixedCustomer)
                    {
                        lblCustomerInfo.Text += " | CỐ ĐỊNH";
                    }
                }
                else
                {
                    _currentDiscountPct = 0;
                    _currentCustomerId = 0;
                    _isFixedCustomer = false;
                    lblCustomerInfo.Text = "⚠ Không tìm thấy khách hàng này!";
                    lblCustomerInfo.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                DatabaseHelper.TryLog("ThanhToan Customer Error", ex, "UCThanhToan.BtnSearchCustomer_Click");
            }

            UpdateTotals();
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
                
                foreach(var pl in pendingLines)
                {
                    // By default: quantity-based services. Some add-ons are per-hour.
                    string name = pl.ProductName ?? "";
                    string unit = "Cái";
                    if (name.IndexOf("máy bắn bóng", StringComparison.OrdinalIgnoreCase) >= 0) unit = "Giờ";
                    else if (name.IndexOf("nhặt bóng", StringComparison.OrdinalIgnoreCase) >= 0) unit = "Giờ";
                    else if (name.IndexOf("bóng", StringComparison.OrdinalIgnoreCase) >= 0 && name.IndexOf("rổ", StringComparison.OrdinalIgnoreCase) >= 0) unit = "Rổ";

                    services.Add(new DemoPick.Services.ServiceCharge {
                        ProductID = pl.ProductId,
                        ServiceName = name,
                        Quantity = pl.Quantity,
                        UnitPrice = pl.UnitPrice,
                        Unit = unit
                    });
                }

                decimal courtMultiplier = 1m;
                try
                {
                    string t = _selectedCourt?.CourtType ?? "";
                    string n = _selectedCourt?.Name ?? "";
                    if (t.IndexOf("tập", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        t.IndexOf("practice", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        n.IndexOf("tập", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        courtMultiplier = 0.5m;
                    }
                }
                catch { }

                var breakdown = DemoPick.Services.PriceCalculator.CalculateTotal(_currentBooking.StartTime, _currentBooking.EndTime, _isFixedCustomer, services, courtMultiplier);

                foreach(var ts in breakdown.TimeSlots)
                {
                    var lviCourt = new ListViewItem(new[] { "Giờ sân " + ts.Description, $"{ts.Hours:0.##}h", ts.Total.ToString("N0") + "đ" });
                    lviCourt.Tag = new PosService.CartLine(-1, "Giờ sân " + ts.Description, 1, ts.Total);
                    lviCourt.ForeColor = Color.DarkBlue; 
                    lstCart.Items.Add(lviCourt);
                }

                foreach(var svc in breakdown.Services)
                {
                    var lvi = new ListViewItem(new[] { svc.ServiceName, svc.Quantity.ToString(), svc.Total.ToString("N0") + "đ" });
                    lvi.Tag = new PosService.CartLine(svc.ProductID, svc.ServiceName, svc.Quantity, svc.UnitPrice);
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
            lblPreviewTotal.Text = lblTotalV.Text; // update fake bill
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ResetCheckoutPane();
        }

        private void BtnCheckout_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedCourtName)) return;

            // Prompt user with yes/no dialog equivalent. 
            // In a pro UI, we'd use Sunny UI Form.
            var diagRet = MessageBox.Show($"Xác nhận Thu tiền và In Hóa Đơn cho {_selectedCourtName}?", "Thanh toán", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (diagRet == DialogResult.No) return;

            decimal discountAmt = _lastDiscountAmount;
            decimal finalTotal = _lastFinalTotal;

            try
            {
                var lines = new System.Collections.Generic.List<PosService.CartLine>();
                foreach (ListViewItem item in lstCart.Items)
                {
                    if (item.Tag is PosService.CartLine line)
                    {
                        lines.Add(line);
                    }
                }

                var pos = new PosService();
                int invoiceId = pos.Checkout(
                    _currentCustomerId,
                    lines,
                    _cartTotal,
                    discountAmt,
                    finalTotal,
                    "Cash",
                    _selectedCourtName
                );

                try
                {
                    using (var frm = new FrmInvoicePreview(invoiceId, _selectedCourtName))
                    {
                        frm.ShowDialog(FindForm());
                    }
                }
                catch (Exception ex) { DatabaseHelper.TryLog("Print error", ex, ""); }

                PosService.ClearPendingOrder(_selectedCourtName);
                new UIPage().ShowSuccessTip("Thanh toán hoàn tất!");
                RefreshOnActivated();

            }
            catch (Exception ex)
            {
                new UIPage().ShowErrorTip("Lỗi: " + ex.Message);
            }
        }
    }
}
