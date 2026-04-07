using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Panel = System.Windows.Forms.Panel;
using Sunny.UI;
using DemoPick.Services;

namespace DemoPick
{
    public partial class UCBanHang : UserControl
    {
        private string _selectedCourtName = "";
        private bool _shownSelectCourtHint;

        private sealed class CartItemTag
        {
            public int ProductId { get; }
            public decimal UnitPrice { get; }

            public CartItemTag(int productId, decimal unitPrice)
            {
                ProductId = productId;
                UnitPrice = unitPrice;
            }
        }

        public UCBanHang()
        {
            InitializeComponent();

            if (DemoPick.Services.DesignModeUtil.IsDesignMode(this))
            {
                return;
            }
            
            pnlLeft.Paint += (s, e) => e.Graphics.DrawLine(new Pen(Color.FromArgb(229, 231, 235), 1), pnlLeft.Width - 1, 0, pnlLeft.Width - 1, pnlLeft.Height);
            pnlRight.Paint += (s, e) => e.Graphics.DrawLine(new Pen(Color.FromArgb(229, 231, 235), 1), 0, 0, 0, pnlRight.Height);
            pnlTotals.Paint += (s, e) => {
                Pen dashPen = new Pen(Color.LightGray, 1) { DashStyle = DashStyle.Dash };
                e.Graphics.DrawLine(dashPen, 0, 75, pnlTotals.Width, 75);
            };

            SetupCartColumns();
            btnAddProduct.Click += BtnAddProduct_Click;
            btnCheckout.Click += BtnSaveOrder_Click;
            btnClearOrder.Click += BtnClearOrder_Click;

            LoadAllData(resetCart: true);
        }

        private void ShowSelectCourtHintIfNeeded()
        {
            if (_shownSelectCourtHint) return;
            if (!string.IsNullOrWhiteSpace(_selectedCourtName)) return;

            _shownSelectCourtHint = true;
            try
            {
                lblRightTitle.Text = "Chưa chọn sân";
                new UIPage().ShowInfoTip("Bạn cần chọn sân (bên trái) rồi mới thêm hàng.");
            }
            catch
            {
                // Best effort.
            }
        }

        private bool EnsureCourtSelectedForAdd()
        {
            if (!string.IsNullOrWhiteSpace(_selectedCourtName)) return true;

            ShowSelectCourtHintIfNeeded();
            MessageBox.Show("Bạn cần chọn sân trước thì mới được thêm hàng.", "Chưa chọn sân", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
        }

        private void BtnAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                using (var f = new FrmThemSP())
                {
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        LoadCatalog();
                        new UIPage().ShowSuccessTip("Đã cập nhật danh sách hàng hóa.");
                    }
                }
            }
            catch (Exception ex)
            {
                DatabaseHelper.TryLog("POS Add Product Error", ex, "UCBanHang.BtnAddProduct_Click");
                new UIPage().ShowErrorTip("Không thể mở form thêm sản phẩm: " + ex.Message);
            }
        }

        private void BtnClearOrder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_selectedCourtName))
            {
                ShowSelectCourtHintIfNeeded();
                return;
            }
            lstCart.Items.Clear();
            PosService.ClearPendingOrder(_selectedCourtName);
            new UIPage().ShowSuccessTip($"Đã xóa đơn chờ của {_selectedCourtName}.");
        }

        private void BtnSaveOrder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_selectedCourtName))
            {
                ShowSelectCourtHintIfNeeded();
                return;
            }
            if (lstCart.Items.Count == 0)
            {
                PosService.ClearPendingOrder(_selectedCourtName);
                new UIPage().ShowSuccessTip("Đã xóa trắng giỏ hàng.");
                return;
            }

            try
            {
                var lines = new System.Collections.Generic.List<PosService.CartLine>();
                foreach (ListViewItem item in lstCart.Items)
                {
                    if (!(item.Tag is CartItemTag meta))
                        throw new InvalidOperationException("Lỗi: Không xác định được sản phẩm.");

                    if (!int.TryParse(item.SubItems[1].Text, out int qty))
                        throw new InvalidOperationException($"Số lượng không hợp lệ cho '{item.Text}'.");

                    lines.Add(new PosService.CartLine(meta.ProductId, item.Text, qty, meta.UnitPrice));
                }

                PosService.SavePendingOrder(_selectedCourtName, lines);
                new UIPage().ShowSuccessTip($"Đã LƯU order cho {_selectedCourtName} thành công!");
            }
            catch (Exception ex)
            {
                new UIPage().ShowErrorTip("Lỗi lưu đơn: " + ex.Message);
            }
        }

        private void SetupCartColumns()
        {
            lstCart.Columns.Add("Sản phẩm", 140);
            lstCart.Columns.Add("SL", 40);
            lstCart.Columns.Add("Thành tiền", 110);
        }

        private void LoadAllData(bool resetCart)
        {
            LoadCatalog();
            LoadCourts();

            if (resetCart)
            {
                lstCart.Items.Clear();
                if (string.IsNullOrWhiteSpace(_selectedCourtName))
                {
                    lblRightTitle.Text = "Chọn sân để thêm hàng";
                }
            }
        }

        private void LoadCatalog()
        {
            try
            {
                var chips = flpCategories.Controls.OfType<UCCategoryChip>().ToList();

                // Use Designer chip texts as defaults so UI still shows categories even when DB is empty.
                var baseCats = chips
                    .Select(c => (c.Tag ?? c.Text)?.ToString())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s => s.Trim())
                    .ToList();

                var catList = new System.Collections.Generic.List<string> { "Tất cả" };
                foreach (var c in baseCats)
                {
                    if (string.Equals(c, "Tất cả", StringComparison.OrdinalIgnoreCase)) continue;
                    if (!catList.Contains(c)) catList.Add(c);
                }

                var dtCats = DemoPick.Services.DatabaseHelper.ExecuteQuery(
                    "SELECT DISTINCT Category FROM Products WHERE Category IS NOT NULL AND LTRIM(RTRIM(Category)) <> '' ORDER BY Category");
                foreach (System.Data.DataRow row in dtCats.Rows)
                {
                    string cat = row[0].ToString()?.Trim();
                    if (string.IsNullOrWhiteSpace(cat)) continue;
                    if (string.Equals(cat, "Tất cả", StringComparison.OrdinalIgnoreCase)) continue;
                    if (!catList.Contains(cat)) catList.Add(cat);
                }

                int bindCount = Math.Min(catList.Count, chips.Count);
                for (int i = 0; i < chips.Count; i++)
                {
                    var chip = chips[i];

                    if (i < bindCount)
                    {
                        chip.Visible = true;
                        chip.Tag = catList[i];
                        chip.Text = catList[i];
                        chip.IsActive = chip.Text == "Tất cả";

                        chip.Click -= CategoryChip_Click;
                        chip.Click += CategoryChip_Click;
                    }
                    else
                    {
                        // Hide unused placeholder chips (if any were added in Designer).
                        chip.Visible = false;
                    }
                }

                if (catList.Count > chips.Count)
                {
                    DatabaseHelper.TryLog(
                        "POS Category Chips Insufficient",
                        new InvalidOperationException($"Need {catList.Count} chips, but only {chips.Count} exist in Designer."),
                        "UCBanHang.LoadCatalog");
                }

                flpProducts.Controls.Clear();
                var dtProds = DatabaseHelper.ExecuteQuery("SELECT ProductID, Name, Price, Category FROM Products");
                int totalProds = 0;

                foreach (System.Data.DataRow row in dtProds.Rows)
                {
                    totalProds++;
                    int prodId = Convert.ToInt32(row["ProductID"]);
                    string nameTxt = row["Name"].ToString();
                    decimal priceVal = Convert.ToDecimal(row["Price"]);
                    string priceTxt = priceVal.ToString("N0") + "đ";

                    Panel pnlProd = new Panel { Size = new Size(150, 180), BackColor = Color.White, Margin = new Padding(10), Cursor = Cursors.Hand, Tag = row["Category"].ToString() };
                    pnlProd.Paint += (s, e) => e.Graphics.DrawRectangle(new Pen(Color.FromArgb(229, 231, 235), 1), 0, 0, pnlProd.Width - 1, pnlProd.Height - 1);

                    PictureBox pic = new PictureBox { Size = new Size(130, 100), Location = new Point(10, 10), BackColor = Color.FromArgb(243, 244, 246) };
                    pic.Enabled = false;
                    Label nameLbl = new Label { Text = nameTxt, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(26, 35, 50), Location = new Point(10, 120), AutoSize = true };
                    nameLbl.Enabled = false;
                    Label priceLbl = new Label { Text = priceTxt, Font = new Font("Segoe UI", 10F, FontStyle.Regular), ForeColor = Color.FromArgb(76, 175, 80), Location = new Point(10, 145), AutoSize = true };
                    priceLbl.Enabled = false;

                    pnlProd.Controls.AddRange(new Control[] { pic, nameLbl, priceLbl });
                    pnlProd.Click += (s, e) => AddToCart(prodId, nameTxt, priceVal);

                    flpProducts.Controls.Add(pnlProd);
                }

                if (totalProds == 0)
                {
                    flpProducts.Controls.Add(new Label { Name = "lblEmptyProd", Text = "Toàn bộ Máy tính tiền đang trống vãn. Sếp nhập hàng vào Kho trước nhé!", Font = new Font("Segoe UI", 11F, FontStyle.Italic), AutoSize = true, Margin = new Padding(20), ForeColor = Color.Gray });
                }
            }
            catch (Exception ex)
            {
                DatabaseHelper.TryLog("POS Load Catalog Error", ex, "UCBanHang.LoadCatalog");
            }
        }

        public void RefreshOnActivated()
        {
            // Called when user navigates back to POS, so it picks up changes from Inventory.
            LoadCatalog();
            LoadCourts();
        }

        private void CategoryChip_Click(object sender, EventArgs e)
        {
            if (!(sender is UCCategoryChip chip)) return;
            string filterCat = (chip.Tag ?? chip.Text ?? "").ToString();

            foreach (var cb in flpCategories.Controls.OfType<UCCategoryChip>())
            {
                cb.SetActive(cb == chip);
            }

            int hitCount = 0;
            foreach (Control pCtrl in flpProducts.Controls)
            {
                if (pCtrl.Name == "lblEmptyProd") continue;
                if (filterCat == "Tất cả" || pCtrl.Tag?.ToString() == filterCat) { pCtrl.Visible = true; hitCount++; }
                else pCtrl.Visible = false;
            }

            if (hitCount == 0)
            {
                if (!flpProducts.Controls.ContainsKey("lblEmptyProd"))
                {
                    flpProducts.Controls.Add(new Label { Name = "lblEmptyProd", Text = "Chưa có món hàng nào thuộc mục này.", Font = new Font("Segoe UI", 11F, FontStyle.Italic), AutoSize = true, Margin = new Padding(20), ForeColor = Color.Gray });
                }
                flpProducts.Controls["lblEmptyProd"].Visible = true;
            }
            else
            {
                if (flpProducts.Controls.ContainsKey("lblEmptyProd")) flpProducts.Controls["lblEmptyProd"].Visible = false;
            }
        }

        private void LoadCourts()
        {
            try
            {
                flpCourts.Controls.Clear();
                var bookCtrl = new DemoPick.Controllers.BookingController();
                var courts = bookCtrl.GetCourts();
                var bookings = bookCtrl.GetBookingsByDate(DateTime.Now);

                foreach (var c in courts)
                {
                    var currentBooking = bookings.Find(b =>
                        b.CourtID == c.CourtID &&
                        !string.Equals(b.Status, "Maintenance", StringComparison.OrdinalIgnoreCase) &&
                        DateTime.Now >= b.StartTime && DateTime.Now <= b.EndTime);
                    bool active = currentBooking != null;
                    string statusTxt = active ? "Đang chơi" : "Trống";
                    string timeTxt = active ? $"{(int)(currentBooking.EndTime - DateTime.Now).TotalMinutes} phút" : "-";
                    Color lineCol = active ? Color.FromArgb(76, 175, 80) : Color.LightGray;

                    Panel pnlCtx = new Panel { Size = new Size(240, 80), BackColor = Color.White, Margin = new Padding(0, 0, 0, 10) };
                    pnlCtx.Paint += (s, e) =>
                    {
                        e.Graphics.DrawRectangle(new Pen(Color.FromArgb(229, 231, 235), 1), 0, 0, pnlCtx.Width - 1, pnlCtx.Height - 1);
                        e.Graphics.FillRectangle(new SolidBrush(lineCol), 0, 10, 4, pnlCtx.Height - 20);
                    };

                    Label cName = new Label { Text = c.Name, Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = Color.FromArgb(26, 35, 50), Location = new Point(15, 15), AutoSize = true };
                    Label badge = new Label { Text = statusTxt, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = active ? Color.White : Color.Gray, BackColor = active ? Color.FromArgb(76, 175, 80) : Color.FromArgb(243, 244, 246), Location = new Point(150, 17), AutoSize = true, Padding = new Padding(2) };
                    Label cTime = new Label { Text = "🕒 " + timeTxt, Font = new Font("Segoe UI", 9F, FontStyle.Regular), ForeColor = Color.Gray, Location = new Point(15, 45), AutoSize = true };

                    pnlCtx.Controls.AddRange(new Control[] { cName, badge, cTime });
                    pnlCtx.Cursor = Cursors.Hand;

                    EventHandler selectCourt = (s, e) =>
                    {
                        lblRightTitle.Text = "Sản phẩm chờ - " + c.Name;
                        _selectedCourtName = c.Name;
                        foreach (Control p in flpCourts.Controls)
                        {
                            if (p is Panel panel) panel.BackColor = Color.White;
                        }
                        pnlCtx.BackColor = Color.FromArgb(235, 248, 235);
                        LoadPendingOrderForCourt(c.Name);
                    };

                    pnlCtx.Click += selectCourt;
                    cName.Click += selectCourt;
                    badge.Click += selectCourt;
                    cTime.Click += selectCourt;

                    flpCourts.Controls.Add(pnlCtx);
                }
            }
            catch (Exception ex)
            {
                DatabaseHelper.TryLog("POS Load Courts Error", ex, "UCBanHang.LoadCourts");
            }
        }

        private void LoadPendingOrderForCourt(string courtName)
        {
            lstCart.Items.Clear();
            var lines = PosService.GetPendingOrder(courtName);
            foreach (var line in lines)
            {
                var lvi = new ListViewItem(new[] { line.ProductName, line.Quantity.ToString(), (line.UnitPrice * line.Quantity).ToString("N0") + "đ" });
                lvi.Tag = new CartItemTag(line.ProductId, line.UnitPrice);
                lstCart.Items.Add(lvi);
            }
        }

        private void AddToCart(string prodName, decimal unitPrice)
        {
            AddToCart(0, prodName, unitPrice);
        }

        private void AddToCart(int productId, string prodName, decimal unitPrice)
        {
            if (!EnsureCourtSelectedForAdd()) return;

            bool found = false;
            foreach (ListViewItem item in lstCart.Items)
            {
                if (item.Tag is CartItemTag tag && tag.ProductId == productId && productId > 0)
                {
                    int qty = int.Parse(item.SubItems[1].Text) + 1;
                    item.SubItems[1].Text = qty.ToString();
                    item.SubItems[2].Text = (qty * unitPrice).ToString("N0") + "đ";
                    found = true;
                    break;
                }

                if (productId <= 0 && item.Text == prodName)
                {
                    int qty = int.Parse(item.SubItems[1].Text) + 1;
                    item.SubItems[1].Text = qty.ToString();
                    item.SubItems[2].Text = (qty * unitPrice).ToString("N0") + "đ";
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                var lvi = new ListViewItem(new[] { prodName, "1", unitPrice.ToString("N0") + "đ" });
                if (productId > 0)
                    lvi.Tag = new CartItemTag(productId, unitPrice);
                lstCart.Items.Add(lvi);
            }
        }


    }
}
