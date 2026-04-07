using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DemoPick.Services;

namespace DemoPick
{
    public partial class UCKhoHang : UserControl
    {
        private InventoryService _inventoryService;

        public UCKhoHang()
        {
            InitializeComponent();

            if (DesignModeUtil.IsDesignMode(this))
            {
                return;
            }
            _inventoryService = new InventoryService();

            btnThemSP.Click += BtnThemSP_Click;

            LoadDataAsync();
        }

        private void BtnThemSP_Click(object sender, EventArgs e)
        {
            try
            {
                using (var f = new FrmThemSP())
                {
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        LoadDataAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                DatabaseHelper.TryLog("Inventory Add Product Error", ex, "UCKhoHang.BtnThemSP_Click");
                MessageBox.Show("Không thể mở form nhập hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void LoadDataAsync()
        {
            var items = await _inventoryService.GetInventoryItemsAsync();
            lstKhoHang.Items.Clear();
            foreach(var item in items)
            {
                lstKhoHang.Items.Add(new ListViewItem(new[] { $"{item.Name}\nSKU: {item.Sku}", item.Category, item.Stock, item.Status, item.Price }));
            }

            var txs = await _inventoryService.GetRecentTransactionsAsync();
            lstGiaoDich.Items.Clear();
            foreach(var tx in txs)
            {
                string sub = (tx.SubDesc ?? "").Replace("\r", " ").Replace("\n", " ").Trim();
                string line = string.IsNullOrWhiteSpace(sub) ? (tx.EventDesc ?? "") : $"{tx.EventDesc} — {sub}";
                lstGiaoDich.Items.Add(new ListViewItem(new[] { line, tx.Time }));
            }

            // Sync Static KPI Labels
            try {
                var dt = DatabaseHelper.ExecuteQuery(@"
                    SELECT 
                        ISNULL(SUM(Price * StockQuantity), 0) as TotalVal,
                        (SELECT COUNT(*) FROM Products WHERE StockQuantity <= MinThreshold AND Category != N'Dịch vụ đi kèm') as CriticalItems,
                        (SELECT ISNULL(SUM(Quantity), 0) FROM InvoiceDetails) as Sales,
                        (SELECT COUNT(*) FROM Invoices) as InvoicesCount
                    FROM Products WHERE Category != N'Dịch vụ đi kèm'
                ");
                if (dt.Rows.Count > 0)
                {
                    decimal totalVal = Convert.ToDecimal(dt.Rows[0]["TotalVal"]);
                    lblC1Value.Text = totalVal == 0 ? "0đ" : totalVal.ToString("N0") + " đ";
                    lblC2Value.Text = dt.Rows[0]["CriticalItems"].ToString() + " SP";
                    lblC3Value.Text = dt.Rows[0]["Sales"].ToString() + " Xuất";
                    lblC4Value.Text = dt.Rows[0]["InvoicesCount"].ToString() + " Đơn";
                }
                else 
                {
                    lblC1Value.Text = "0đ";
                    lblC2Value.Text = "0 SP";
                    lblC3Value.Text = "0 Xuất";
                    lblC4Value.Text = "0 Đơn";
                }
                
                lblC1Title.Text = "Tổng giá trị kho";
                lblC2Title.Text = "Cảnh báo hết hàng";
                lblC3Title.Text = "Sản phẩm đã bán";
                lblC4Title.Text = "Hóa đơn xuất";
            }
            catch (Exception ex)
            {
                DatabaseHelper.TryLog("Inventory KPI Sync Error", ex, "UCKhoHang.LoadDataAsync KPI Sync");
            }

            // Chart (Clear mock data since DB is clean)
            chartDuBao.Series[0].Points.Clear();
            chartDuBao.Series[0].Points.AddXY("T2", 0);
            chartDuBao.Series[0].Points.AddXY("T3", 0);
            chartDuBao.Series[0].Points.AddXY("T4", 0);
            chartDuBao.Series[0].Points.AddXY("T5", 0);
            chartDuBao.Series[0].Points.AddXY("T6", 0);
            chartDuBao.Series[0].Points.AddXY("T7", 0);
            chartDuBao.Series[0].Points.AddXY("CN", 0);
        }

        public void RefreshOnActivated()
        {
            LoadDataAsync();
        }
    }
}
