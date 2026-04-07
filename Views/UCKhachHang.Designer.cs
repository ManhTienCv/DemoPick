using System.Drawing;
using System.Windows.Forms;
using Sunny.UI;

namespace DemoPick
{
    partial class UCKhachHang
    {
        private System.ComponentModel.IContainer components = null;

        // Top 2 Cards
        public UIPanel pnlCardFixed;
        public Label lblFixedTitle;
        public Label lblFixedValue;
        public Label lblFixedDesc;
        public Label lblFixedCount;

        public UIPanel pnlCardWalkin;
        public Label lblWalkinTitle;
        public Label lblWalkinValue;
        public Label lblWalkinDesc;
        public Label lblWalkinCount;

        // Middle Section (List)
        public UIPanel pnlList;
        public Label lblTabAll;
        public Label lblTabFixed;
        public Label lblTabWalkin;
        public Label lblTabNew;
        public UIPanel pnlTabIndicator;
        public PictureBox picFilter;
        public ListView lstKhachHang;

        // Bottom 4 Cards
        public UIPanel pnlBot1;
        public Label lblBot1Title;
        public Label lblBot1Value;
        public Label lblBot1Desc;

        public UIPanel pnlBot2;
        public Label lblBot2Title;
        public Label lblBot2Value;
        public Label lblBot2Desc;

        public UIPanel pnlBot3;
        public Label lblBot3Title;
        public Label lblBot3Value;
        public Label lblBot3Desc;

        public UIPanel pnlBot4;
        public Label lblBot4Title;
        public Label lblBot4Value;
        public Label lblBot4Desc;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlCardFixed = new Sunny.UI.UIPanel();
            this.lblFixedTitle = new System.Windows.Forms.Label();
            this.lblFixedValue = new System.Windows.Forms.Label();
            this.lblFixedDesc = new System.Windows.Forms.Label();
            this.lblFixedCount = new System.Windows.Forms.Label();

            this.pnlCardWalkin = new Sunny.UI.UIPanel();
            this.lblWalkinTitle = new System.Windows.Forms.Label();
            this.lblWalkinValue = new System.Windows.Forms.Label();
            this.lblWalkinDesc = new System.Windows.Forms.Label();
            this.lblWalkinCount = new System.Windows.Forms.Label();

            this.pnlList = new Sunny.UI.UIPanel();
            this.lblTabAll = new System.Windows.Forms.Label();
            this.lblTabFixed = new System.Windows.Forms.Label();
            this.lblTabWalkin = new System.Windows.Forms.Label();
            this.lblTabNew = new System.Windows.Forms.Label();
            this.pnlTabIndicator = new Sunny.UI.UIPanel();
            this.picFilter = new System.Windows.Forms.PictureBox();
            this.lstKhachHang = new System.Windows.Forms.ListView();

            this.pnlBot1 = new Sunny.UI.UIPanel();
            this.lblBot1Title = new System.Windows.Forms.Label();
            this.lblBot1Value = new System.Windows.Forms.Label();
            this.lblBot1Desc = new System.Windows.Forms.Label();

            this.pnlBot2 = new Sunny.UI.UIPanel();
            this.lblBot2Title = new System.Windows.Forms.Label();
            this.lblBot2Value = new System.Windows.Forms.Label();
            this.lblBot2Desc = new System.Windows.Forms.Label();

            this.pnlBot3 = new Sunny.UI.UIPanel();
            this.lblBot3Title = new System.Windows.Forms.Label();
            this.lblBot3Value = new System.Windows.Forms.Label();
            this.lblBot3Desc = new System.Windows.Forms.Label();

            this.pnlBot4 = new Sunny.UI.UIPanel();
            this.lblBot4Title = new System.Windows.Forms.Label();
            this.lblBot4Value = new System.Windows.Forms.Label();
            this.lblBot4Desc = new System.Windows.Forms.Label();

            this.SuspendLayout();

            // Card FIXED
            this.pnlCardFixed.Radius = 16;
            this.pnlCardFixed.FillColor = Color.White;
            this.pnlCardFixed.RectColor = Color.FromArgb(229, 231, 235);
            this.pnlCardFixed.Size = new Size(550, 140);
            this.pnlCardFixed.Location = new Point(20, 20);
            this.pnlCardFixed.Controls.Add(this.lblFixedTitle);
            this.pnlCardFixed.Controls.Add(this.lblFixedValue);
            this.pnlCardFixed.Controls.Add(this.lblFixedDesc);
            this.pnlCardFixed.Controls.Add(this.lblFixedCount);
            
            this.lblFixedTitle.Text = "KHÁCH CỐ ĐỊNH";
            this.lblFixedTitle.ForeColor = Color.FromArgb(22, 163, 74); 
            this.lblFixedTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblFixedTitle.Location = new Point(30, 20);
            this.lblFixedTitle.AutoSize = true;

            this.lblFixedValue.Text = "Giảm 20-30k/h";
            this.lblFixedValue.ForeColor = Color.FromArgb(31, 41, 55);
            this.lblFixedValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            this.lblFixedValue.Location = new Point(25, 45);
            this.lblFixedValue.AutoSize = true;

            this.lblFixedDesc.Text = "Đạt từ 30 giờ chơi tích lũy. Trừ thẳng vào từng khung giờ.";
            this.lblFixedDesc.ForeColor = Color.FromArgb(107, 114, 128);
            this.lblFixedDesc.Font = new Font("Segoe UI", 10F);
            this.lblFixedDesc.Location = new Point(30, 85);
            this.lblFixedDesc.AutoSize = true;

            this.lblFixedCount.Text = "● 0 Hội viên";
            this.lblFixedCount.ForeColor = Color.FromArgb(22, 163, 74);
            this.lblFixedCount.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblFixedCount.Location = new Point(30, 110);
            this.lblFixedCount.AutoSize = true;

            // Card WALKIN
            this.pnlCardWalkin.Radius = 16;
            this.pnlCardWalkin.FillColor = Color.White;
            this.pnlCardWalkin.RectColor = Color.FromArgb(229, 231, 235);
            this.pnlCardWalkin.Size = new Size(550, 140);
            this.pnlCardWalkin.Location = new Point(590, 20);
            this.pnlCardWalkin.Controls.Add(this.lblWalkinTitle);
            this.pnlCardWalkin.Controls.Add(this.lblWalkinValue);
            this.pnlCardWalkin.Controls.Add(this.lblWalkinDesc);
            this.pnlCardWalkin.Controls.Add(this.lblWalkinCount);
            
            this.lblWalkinTitle.Text = "KHÁCH VÃNG LAI";
            this.lblWalkinTitle.ForeColor = Color.FromArgb(59, 130, 246); 
            this.lblWalkinTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblWalkinTitle.Location = new Point(30, 20);
            this.lblWalkinTitle.AutoSize = true;

            this.lblWalkinValue.Text = "Giá niêm yết";
            this.lblWalkinValue.ForeColor = Color.FromArgb(31, 41, 55);
            this.lblWalkinValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            this.lblWalkinValue.Location = new Point(25, 45);
            this.lblWalkinValue.AutoSize = true;

            this.lblWalkinDesc.Text = "Giá theo khung giờ tiêu chuẩn. Không tài khoản hoặc ít bến bãi.";
            this.lblWalkinDesc.ForeColor = Color.FromArgb(107, 114, 128);
            this.lblWalkinDesc.Font = new Font("Segoe UI", 10F);
            this.lblWalkinDesc.Location = new Point(30, 85);
            this.lblWalkinDesc.AutoSize = true;

            this.lblWalkinCount.Text = "● 0 Hội viên";
            this.lblWalkinCount.ForeColor = Color.FromArgb(59, 130, 246);
            this.lblWalkinCount.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblWalkinCount.Location = new Point(30, 110);
            this.lblWalkinCount.AutoSize = true;

            // Middle List Section
            this.pnlList.Radius = 16;
            this.pnlList.FillColor = Color.White;
            this.pnlList.RectColor = Color.FromArgb(229, 231, 235);
            this.pnlList.Size = new Size(1120, 420);
            this.pnlList.Location = new Point(20, 180);
            
            // Tabs
            this.lblTabAll.Text = "Tất cả";
            this.lblTabAll.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.lblTabAll.ForeColor = Color.FromArgb(22, 163, 74); // Active Green
            this.lblTabAll.Location = new Point(25, 20);
            this.lblTabAll.AutoSize = true;
            this.lblTabAll.BackColor = Color.White;

            this.lblTabFixed.Text = "Cố định";
            this.lblTabFixed.Font = new Font("Segoe UI", 11F);
            this.lblTabFixed.ForeColor = Color.FromArgb(107, 114, 128);
            this.lblTabFixed.Location = new Point(85, 20);
            this.lblTabFixed.AutoSize = true;
            this.lblTabFixed.BackColor = Color.White;

            this.lblTabWalkin.Text = "Vãng lai";
            this.lblTabWalkin.Font = new Font("Segoe UI", 11F);
            this.lblTabWalkin.ForeColor = Color.FromArgb(107, 114, 128);
            this.lblTabWalkin.Location = new Point(160, 20);
            this.lblTabWalkin.AutoSize = true;
            this.lblTabWalkin.BackColor = Color.White;

            this.lblTabNew.Text = "Mới (24h)";
            this.lblTabNew.Font = new Font("Segoe UI", 11F);
            this.lblTabNew.ForeColor = Color.FromArgb(107, 114, 128);
            this.lblTabNew.Location = new Point(240, 20);
            this.lblTabNew.AutoSize = true;
            this.lblTabNew.BackColor = Color.White;

            this.pnlTabIndicator.Size = new Size(50, 3);
            this.pnlTabIndicator.FillColor = Color.FromArgb(22, 163, 74);
            this.pnlTabIndicator.RectColor = Color.Transparent;
            this.pnlTabIndicator.Location = new Point(25, 45);

            // Filter Icon
            this.picFilter.Size = new Size(30, 30);
            this.picFilter.Location = new Point(1070, 15);
            this.picFilter.BackColor = Color.White;

            // ListView
            this.lstKhachHang.BorderStyle = BorderStyle.None;
            this.lstKhachHang.View = View.Details;
            this.lstKhachHang.FullRowSelect = true;
            this.lstKhachHang.Font = new Font("Segoe UI", 10.5F);
            this.lstKhachHang.Location = new Point(20, 60);
            this.lstKhachHang.Size = new Size(1080, 340);
            this.lstKhachHang.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.lstKhachHang.OwnerDraw = true;
            
            // Add columns
            this.lstKhachHang.Columns.Add("KHÁCH HÀNG", 250);
            this.lstKhachHang.Columns.Add("LIÊN HỆ", 200);
            this.lstKhachHang.Columns.Add("PHÂN LOẠI", 200);
            this.lstKhachHang.Columns.Add("GIỜ TÍCH LŨY", 250);
            this.lstKhachHang.Columns.Add("TỔNG CHI", 150);

            this.pnlList.Controls.Add(this.lblTabAll);
            this.pnlList.Controls.Add(this.lblTabFixed);
            this.pnlList.Controls.Add(this.lblTabWalkin);
            this.pnlList.Controls.Add(this.lblTabNew);
            this.pnlList.Controls.Add(this.pnlTabIndicator);
            this.pnlList.Controls.Add(this.picFilter);
            this.pnlList.Controls.Add(this.lstKhachHang);

            // Bottom Cards
            int botY = 620;
            this.pnlBot1.Radius = 16; this.pnlBot1.FillColor = Color.White; this.pnlBot1.RectColor = Color.FromArgb(229, 231, 235);
            this.pnlBot1.Size = new Size(265, 110); this.pnlBot1.Location = new Point(20, botY);
            
            this.lblBot1Title.Text = "Tổng khách hàng"; this.lblBot1Title.ForeColor = Color.FromArgb(107, 114, 128); this.lblBot1Title.Font = new Font("Segoe UI", 9.5F);
            this.lblBot1Title.Location = new Point(15, 15); this.lblBot1Title.AutoSize = true;
            this.lblBot1Value.Text = "0"; this.lblBot1Value.ForeColor = Color.FromArgb(31, 41, 55); this.lblBot1Value.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            this.lblBot1Value.Location = new Point(10, 40); this.lblBot1Value.AutoSize = true;
            this.lblBot1Desc.Text = "↗ +12% tháng này"; this.lblBot1Desc.ForeColor = Color.FromArgb(22, 163, 74); this.lblBot1Desc.Font = new Font("Segoe UI", 9F);
            this.lblBot1Desc.Location = new Point(15, 80); this.lblBot1Desc.AutoSize = true;
            this.pnlBot1.Controls.Add(lblBot1Title); this.pnlBot1.Controls.Add(lblBot1Value); this.pnlBot1.Controls.Add(lblBot1Desc);

            this.pnlBot2.Radius = 16; this.pnlBot2.FillColor = Color.White; this.pnlBot2.RectColor = Color.FromArgb(229, 231, 235);
            this.pnlBot2.Size = new Size(265, 110); this.pnlBot2.Location = new Point(305, botY);
            this.lblBot2Title.Text = "Khách cố định"; this.lblBot2Title.ForeColor = Color.FromArgb(107, 114, 128); this.lblBot2Title.Font = new Font("Segoe UI", 9.5F);
            this.lblBot2Title.Location = new Point(15, 15); this.lblBot2Title.AutoSize = true;
            this.lblBot2Value.Text = "0"; this.lblBot2Value.ForeColor = Color.FromArgb(31, 41, 55); this.lblBot2Value.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            this.lblBot2Value.Location = new Point(10, 40); this.lblBot2Value.AutoSize = true;
            this.lblBot2Desc.Text = "≥ 30h tích luỹ"; this.lblBot2Desc.ForeColor = Color.FromArgb(22, 163, 74); this.lblBot2Desc.Font = new Font("Segoe UI", 9F);
            this.lblBot2Desc.Location = new Point(15, 80); this.lblBot2Desc.AutoSize = true;
            this.pnlBot2.Controls.Add(lblBot2Title); this.pnlBot2.Controls.Add(lblBot2Value); this.pnlBot2.Controls.Add(lblBot2Desc);

            this.pnlBot3.Radius = 16; this.pnlBot3.FillColor = Color.White; this.pnlBot3.RectColor = Color.FromArgb(229, 231, 235);
            this.pnlBot3.Size = new Size(265, 110); this.pnlBot3.Location = new Point(590, botY);
            this.lblBot3Title.Text = "Doanh thu CRM"; this.lblBot3Title.ForeColor = Color.FromArgb(107, 114, 128); this.lblBot3Title.Font = new Font("Segoe UI", 9.5F);
            this.lblBot3Title.Location = new Point(15, 15); this.lblBot3Title.AutoSize = true;
            this.lblBot3Value.Text = "0đ"; this.lblBot3Value.ForeColor = Color.FromArgb(31, 41, 55); this.lblBot3Value.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            this.lblBot3Value.Location = new Point(10, 40); this.lblBot3Value.AutoSize = true;
            this.lblBot3Desc.Text = "↗ +18%"; this.lblBot3Desc.ForeColor = Color.FromArgb(22, 163, 74); this.lblBot3Desc.Font = new Font("Segoe UI", 9F);
            this.lblBot3Desc.Location = new Point(15, 80); this.lblBot3Desc.AutoSize = true;
            this.pnlBot3.Controls.Add(lblBot3Title); this.pnlBot3.Controls.Add(lblBot3Value); this.pnlBot3.Controls.Add(lblBot3Desc);

            this.pnlBot4.Radius = 16; this.pnlBot4.FillColor = Color.White; this.pnlBot4.RectColor = Color.FromArgb(229, 231, 235);
            this.pnlBot4.Size = new Size(265, 110); this.pnlBot4.Location = new Point(875, botY);
            this.lblBot4Title.Text = "Hoạt động sân"; this.lblBot4Title.ForeColor = Color.FromArgb(107, 114, 128); this.lblBot4Title.Font = new Font("Segoe UI", 9.5F);
            this.lblBot4Title.Location = new Point(15, 15); this.lblBot4Title.AutoSize = true;
            this.lblBot4Value.Text = "0%"; this.lblBot4Value.ForeColor = Color.FromArgb(31, 41, 55); this.lblBot4Value.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            this.lblBot4Value.Location = new Point(10, 40); this.lblBot4Value.AutoSize = true;
            this.lblBot4Desc.Text = "Công suất trung bình"; this.lblBot4Desc.ForeColor = Color.FromArgb(107, 114, 128); this.lblBot4Desc.Font = new Font("Segoe UI", 9F);
            this.lblBot4Desc.Location = new Point(15, 80); this.lblBot4Desc.AutoSize = true;
            this.pnlBot4.Controls.Add(lblBot4Title); this.pnlBot4.Controls.Add(lblBot4Value); this.pnlBot4.Controls.Add(lblBot4Desc);

            this.BackColor = Color.FromArgb(250, 250, 250);
            this.Size = new Size(1160, 820);
            this.AutoScroll = true;
            this.Controls.Add(this.pnlCardFixed);
            this.Controls.Add(this.pnlCardWalkin);
            this.Controls.Add(this.pnlList);
            this.Controls.Add(this.pnlBot1);
            this.Controls.Add(this.pnlBot2);
            this.Controls.Add(this.pnlBot3);
            this.Controls.Add(this.pnlBot4);

            this.ResumeLayout(false);
        }
    }
}
