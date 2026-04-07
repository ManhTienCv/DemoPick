using System.Drawing;
using System.Windows.Forms;
using Sunny.UI;
using System.Windows.Forms.DataVisualization.Charting;

namespace DemoPick
{
    partial class UCKhoHang
    {
        private System.ComponentModel.IContainer components = null;

        // Top 4 Cards
        public UIPanel pnlCard1;
        public PictureBox pic1;
        public Label lblC1Title;
        public Label lblC1Value;
        public Label lblC1Badge;

        public UIPanel pnlCard2;
        public PictureBox pic2;
        public Label lblC2Title;
        public Label lblC2Value;
        public Label lblC2Badge;

        public UIPanel pnlCard3;
        public PictureBox pic3;
        public Label lblC3Title;
        public Label lblC3Value;
        public Label lblC3Badge;

        public UIPanel pnlCard4;
        public PictureBox pic4;
        public Label lblC4Title;
        public Label lblC4Value;
        public Label lblC4Badge;

        // Middle Section (List)
        public UIPanel pnlList;
        public Label lblListTitle;
        public UIButton btnThemSP;
        public ListView lstKhoHang;

        // Bottom Section
        public UIPanel pnlBotLeft;
        public Label lblBotLeftTitle;
        public ListView lstGiaoDich;

        public UIPanel pnlBotRight;
        public Label lblBotRightTitle;
        public Chart chartDuBao;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlCard1 = new Sunny.UI.UIPanel();
            this.pic1 = new System.Windows.Forms.PictureBox();
            this.lblC1Title = new System.Windows.Forms.Label();
            this.lblC1Value = new System.Windows.Forms.Label();
            this.lblC1Badge = new System.Windows.Forms.Label();

            this.pnlCard2 = new Sunny.UI.UIPanel();
            this.pic2 = new System.Windows.Forms.PictureBox();
            this.lblC2Title = new System.Windows.Forms.Label();
            this.lblC2Value = new System.Windows.Forms.Label();
            this.lblC2Badge = new System.Windows.Forms.Label();

            this.pnlCard3 = new Sunny.UI.UIPanel();
            this.pic3 = new System.Windows.Forms.PictureBox();
            this.lblC3Title = new System.Windows.Forms.Label();
            this.lblC3Value = new System.Windows.Forms.Label();
            this.lblC3Badge = new System.Windows.Forms.Label();

            this.pnlCard4 = new Sunny.UI.UIPanel();
            this.pic4 = new System.Windows.Forms.PictureBox();
            this.lblC4Title = new System.Windows.Forms.Label();
            this.lblC4Value = new System.Windows.Forms.Label();
            this.lblC4Badge = new System.Windows.Forms.Label();

            this.pnlList = new Sunny.UI.UIPanel();
            this.lblListTitle = new System.Windows.Forms.Label();
            this.btnThemSP = new Sunny.UI.UIButton();
            this.lstKhoHang = new System.Windows.Forms.ListView();

            this.pnlBotLeft = new Sunny.UI.UIPanel();
            this.lblBotLeftTitle = new System.Windows.Forms.Label();
            this.lstGiaoDich = new System.Windows.Forms.ListView();

            this.pnlBotRight = new Sunny.UI.UIPanel();
            this.lblBotRightTitle = new System.Windows.Forms.Label();
            this.chartDuBao = new System.Windows.Forms.DataVisualization.Charting.Chart();

            ((System.ComponentModel.ISupportInitialize)(this.chartDuBao)).BeginInit();
            this.SuspendLayout();

            // Card 1
            this.pnlCard1.Radius = 16; this.pnlCard1.FillColor = Color.White; this.pnlCard1.RectColor = Color.FromArgb(229, 231, 235);
            this.pnlCard1.Size = new Size(265, 120); this.pnlCard1.Location = new Point(20, 20);
            this.pic1.Size = new Size(40, 40); this.pic1.Location = new Point(15, 15);
            this.pic1.BackColor = Color.FromArgb(232, 245, 233); // light green
            this.lblC1Badge.Text = ""; this.lblC1Badge.Location = new Point(210, 20); this.lblC1Badge.ForeColor = Color.FromArgb(76, 175, 80); this.lblC1Badge.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold); this.lblC1Badge.AutoSize = true;
            this.lblC1Title.Text = "Tổng giá trị kho"; this.lblC1Title.Location = new Point(15, 65); this.lblC1Title.ForeColor = Color.FromArgb(107, 114, 128); this.lblC1Title.Font = new Font("Segoe UI", 9.5F); this.lblC1Title.AutoSize = true;
            this.lblC1Value.Text = "0 đ"; this.lblC1Value.Location = new Point(10, 85); this.lblC1Value.ForeColor = Color.FromArgb(31, 41, 55); this.lblC1Value.Font = new Font("Segoe UI", 16F, FontStyle.Bold); this.lblC1Value.AutoSize = true;
            this.pnlCard1.Controls.Add(pic1); this.pnlCard1.Controls.Add(lblC1Badge); this.pnlCard1.Controls.Add(lblC1Title); this.pnlCard1.Controls.Add(lblC1Value);

            // Card 2
            this.pnlCard2.Radius = 16; this.pnlCard2.FillColor = Color.White; this.pnlCard2.RectColor = Color.FromArgb(229, 231, 235);
            this.pnlCard2.Size = new Size(265, 120); this.pnlCard2.Location = new Point(305, 20);
            this.pic2.Size = new Size(40, 40); this.pic2.Location = new Point(15, 15);
            this.pic2.BackColor = Color.FromArgb(253, 232, 232); // light red
            this.lblC2Badge.Text = ""; this.lblC2Badge.Location = new Point(200, 20); this.lblC2Badge.ForeColor = Color.FromArgb(245, 108, 108); this.lblC2Badge.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold); this.lblC2Badge.AutoSize = true;
            this.lblC2Title.Text = "Cảnh báo hết hàng"; this.lblC2Title.Location = new Point(15, 65); this.lblC2Title.ForeColor = Color.FromArgb(107, 114, 128); this.lblC2Title.Font = new Font("Segoe UI", 9.5F); this.lblC2Title.AutoSize = true;
            this.lblC2Value.Text = "0 Sản phẩm"; this.lblC2Value.Location = new Point(10, 85); this.lblC2Value.ForeColor = Color.FromArgb(31, 41, 55); this.lblC2Value.Font = new Font("Segoe UI", 16F, FontStyle.Bold); this.lblC2Value.AutoSize = true;
            this.pnlCard2.Controls.Add(pic2); this.pnlCard2.Controls.Add(lblC2Badge); this.pnlCard2.Controls.Add(lblC2Title); this.pnlCard2.Controls.Add(lblC2Value);

            // Card 3
            this.pnlCard3.Radius = 16; this.pnlCard3.FillColor = Color.White; this.pnlCard3.RectColor = Color.FromArgb(229, 231, 235);
            this.pnlCard3.Size = new Size(265, 120); this.pnlCard3.Location = new Point(590, 20);
            this.pic3.Size = new Size(40, 40); this.pic3.Location = new Point(15, 15);
            this.pic3.BackColor = Color.FromArgb(232, 245, 233); // light green
            this.lblC3Badge.Text = ""; this.lblC3Badge.Location = new Point(210, 20); this.lblC3Badge.ForeColor = Color.FromArgb(107, 114, 128); this.lblC3Badge.Font = new Font("Segoe UI", 8.5F); this.lblC3Badge.AutoSize = true;
            this.lblC3Title.Text = "Sản phẩm đã bán"; this.lblC3Title.Location = new Point(15, 65); this.lblC3Title.ForeColor = Color.FromArgb(107, 114, 128); this.lblC3Title.Font = new Font("Segoe UI", 9.5F); this.lblC3Title.AutoSize = true;
            this.lblC3Value.Text = "0 Giao dịch"; this.lblC3Value.Location = new Point(10, 85); this.lblC3Value.ForeColor = Color.FromArgb(31, 41, 55); this.lblC3Value.Font = new Font("Segoe UI", 16F, FontStyle.Bold); this.lblC3Value.AutoSize = true;
            this.pnlCard3.Controls.Add(pic3); this.pnlCard3.Controls.Add(lblC3Badge); this.pnlCard3.Controls.Add(lblC3Title); this.pnlCard3.Controls.Add(lblC3Value);

            // Card 4
            this.pnlCard4.Radius = 16; this.pnlCard4.FillColor = Color.White; this.pnlCard4.RectColor = Color.FromArgb(229, 231, 235);
            this.pnlCard4.Size = new Size(265, 120); this.pnlCard4.Location = new Point(875, 20);
            this.pic4.Size = new Size(40, 40); this.pic4.Location = new Point(15, 15);
            this.pic4.BackColor = Color.FromArgb(232, 245, 233); // light green
            this.lblC4Badge.Text = ""; this.lblC4Badge.Location = new Point(200, 20); this.lblC4Badge.ForeColor = Color.FromArgb(107, 114, 128); this.lblC4Badge.Font = new Font("Segoe UI", 8.5F); this.lblC4Badge.AutoSize = true;
            this.lblC4Title.Text = "Hóa đơn xuất"; this.lblC4Title.Location = new Point(15, 65); this.lblC4Title.ForeColor = Color.FromArgb(107, 114, 128); this.lblC4Title.Font = new Font("Segoe UI", 9.5F); this.lblC4Title.AutoSize = true;
            this.lblC4Value.Text = "0 Đơn"; this.lblC4Value.Location = new Point(10, 85); this.lblC4Value.ForeColor = Color.FromArgb(31, 41, 55); this.lblC4Value.Font = new Font("Segoe UI", 16F, FontStyle.Bold); this.lblC4Value.AutoSize = true;
            this.pnlCard4.Controls.Add(pic4); this.pnlCard4.Controls.Add(lblC4Badge); this.pnlCard4.Controls.Add(lblC4Title); this.pnlCard4.Controls.Add(lblC4Value);

            // Middle Section
            this.pnlList.Radius = 16;
            this.pnlList.FillColor = Color.White;
            this.pnlList.RectColor = Color.FromArgb(229, 231, 235);
            this.pnlList.Size = new Size(1120, 260);
            this.pnlList.Location = new Point(20, 160);
            
            this.lblListTitle.Text = "Danh sách tồn kho";
            this.lblListTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblListTitle.Location = new Point(20, 20);
            this.lblListTitle.AutoSize = true;
            this.lblListTitle.BackColor = Color.White;

            // 
            // btnThemSP
            // 
            this.btnThemSP.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThemSP.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnThemSP.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(185)))), ((int)(((byte)(90)))));
            this.btnThemSP.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnThemSP.Location = new System.Drawing.Point(900, 15);
            this.btnThemSP.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnThemSP.Name = "btnThemSP";
            this.btnThemSP.Radius = 14;
            this.btnThemSP.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnThemSP.Size = new System.Drawing.Size(200, 35);
            this.btnThemSP.TabIndex = 0;
            this.btnThemSP.Text = "[+] NHẬP HÀNG HÓA";
            this.btnThemSP.TipsFont = new System.Drawing.Font("Segoe UI", 9F);
            this.btnThemSP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));

            this.lstKhoHang.BorderStyle = BorderStyle.None;
            this.lstKhoHang.View = View.Details;
            this.lstKhoHang.FullRowSelect = true;
            this.lstKhoHang.Font = new Font("Segoe UI", 10F);
            this.lstKhoHang.Location = new Point(20, 60);
            this.lstKhoHang.Size = new Size(1080, 180);
            this.lstKhoHang.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            
            this.lstKhoHang.Columns.Add("SẢN PHẨM", 280);
            this.lstKhoHang.Columns.Add("DANH MỤC", 200);
            this.lstKhoHang.Columns.Add("TỒN KHO", 200);
            this.lstKhoHang.Columns.Add("TRẠNG THÁI", 200);
            this.lstKhoHang.Columns.Add("GIÁ", 150);

            this.pnlList.Controls.Add(this.lblListTitle);
            this.pnlList.Controls.Add(this.btnThemSP);
            this.pnlList.Controls.Add(this.lstKhoHang);

            // Bottom Left
            this.pnlBotLeft.Radius = 16;
            this.pnlBotLeft.FillColor = Color.White;
            this.pnlBotLeft.RectColor = Color.FromArgb(229, 231, 235);
            this.pnlBotLeft.Size = new Size(540, 260);
            this.pnlBotLeft.Location = new Point(20, 440);
            
            this.lblBotLeftTitle.Text = "Giao dịch gần đây";
            this.lblBotLeftTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblBotLeftTitle.Location = new Point(20, 20);
            this.lblBotLeftTitle.AutoSize = true;
            this.lblBotLeftTitle.BackColor = Color.White;

            this.lstGiaoDich.BorderStyle = BorderStyle.None;
            this.lstGiaoDich.View = View.Details;
            this.lstGiaoDich.FullRowSelect = true;
            this.lstGiaoDich.Font = new Font("Segoe UI", 10F);
            this.lstGiaoDich.Location = new Point(20, 60);
            this.lstGiaoDich.Size = new Size(500, 180);
            this.lstGiaoDich.HeaderStyle = ColumnHeaderStyle.None;
            this.lstGiaoDich.Columns.Add("Event", 400);
            this.lstGiaoDich.Columns.Add("Time", 100);

            this.pnlBotLeft.Controls.Add(lblBotLeftTitle);
            this.pnlBotLeft.Controls.Add(lstGiaoDich);

            // Bottom Right
            this.pnlBotRight.Radius = 16;
            this.pnlBotRight.FillColor = Color.White;
            this.pnlBotRight.RectColor = Color.FromArgb(229, 231, 235);
            this.pnlBotRight.Size = new Size(560, 260);
            this.pnlBotRight.Location = new Point(580, 440);
            
            this.lblBotRightTitle.Text = "Dự báo tồn kho";
            this.lblBotRightTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblBotRightTitle.Location = new Point(20, 20);
            this.lblBotRightTitle.AutoSize = true;
            this.lblBotRightTitle.BackColor = Color.White;

            // Chart bar
            ChartArea ca = new ChartArea("MainArea");
            ca.AxisX.MajorGrid.Enabled = false;
            ca.AxisY.MajorGrid.LineColor = Color.FromArgb(229, 231, 235);
            this.chartDuBao.ChartAreas.Add(ca);
            this.chartDuBao.Location = new Point(20, 60);
            this.chartDuBao.Size = new Size(520, 180);
            this.chartDuBao.Series.Clear();
            Series series = new Series("Forecast");
            series.ChartType = SeriesChartType.Column;
            series.Color = Color.FromArgb(76, 175, 80);
            this.chartDuBao.Series.Add(series);

            this.pnlBotRight.Controls.Add(lblBotRightTitle);
            this.pnlBotRight.Controls.Add(chartDuBao);

            this.BackColor = Color.FromArgb(250, 250, 250);
            this.Size = new Size(1160, 820);
            this.AutoScroll = true;

            this.Controls.Add(this.pnlCard1);
            this.Controls.Add(this.pnlCard2);
            this.Controls.Add(this.pnlCard3);
            this.Controls.Add(this.pnlCard4);
            this.Controls.Add(this.pnlList);
            this.Controls.Add(this.pnlBotLeft);
            this.Controls.Add(this.pnlBotRight);

            ((System.ComponentModel.ISupportInitialize)(this.chartDuBao)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
