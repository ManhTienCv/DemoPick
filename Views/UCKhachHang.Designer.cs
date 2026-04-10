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
            this.picFilter = new System.Windows.Forms.PictureBox();
            this.pnlCardFixed.SuspendLayout();
            this.pnlCardWalkin.SuspendLayout();
            this.pnlList.SuspendLayout();
            this.pnlBot1.SuspendLayout();
            this.pnlBot2.SuspendLayout();
            this.pnlBot3.SuspendLayout();
            this.pnlBot4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFilter)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlCardFixed
            // 
            this.pnlCardFixed.Controls.Add(this.lblFixedTitle);
            this.pnlCardFixed.Controls.Add(this.lblFixedValue);
            this.pnlCardFixed.Controls.Add(this.lblFixedDesc);
            this.pnlCardFixed.Controls.Add(this.lblFixedCount);
            this.pnlCardFixed.FillColor = System.Drawing.Color.White;
            this.pnlCardFixed.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.pnlCardFixed.Location = new System.Drawing.Point(20, 20);
            this.pnlCardFixed.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlCardFixed.MinimumSize = new System.Drawing.Size(1, 1);
            this.pnlCardFixed.Name = "pnlCardFixed";
            this.pnlCardFixed.Radius = 16;
            this.pnlCardFixed.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.pnlCardFixed.Size = new System.Drawing.Size(550, 140);
            this.pnlCardFixed.TabIndex = 0;
            this.pnlCardFixed.Text = null;
            this.pnlCardFixed.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFixedTitle
            // 
            this.lblFixedTitle.AutoSize = true;
            this.lblFixedTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblFixedTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.lblFixedTitle.Location = new System.Drawing.Point(30, 20);
            this.lblFixedTitle.Name = "lblFixedTitle";
            this.lblFixedTitle.Size = new System.Drawing.Size(148, 23);
            this.lblFixedTitle.TabIndex = 0;
            this.lblFixedTitle.Text = "KHÁCH CỐ ĐỊNH";
            // 
            // lblFixedValue
            // 
            this.lblFixedValue.AutoSize = true;
            this.lblFixedValue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblFixedValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblFixedValue.Location = new System.Drawing.Point(25, 45);
            this.lblFixedValue.Name = "lblFixedValue";
            this.lblFixedValue.Size = new System.Drawing.Size(260, 46);
            this.lblFixedValue.TabIndex = 1;
            this.lblFixedValue.Text = "Giảm 20-30k/h";
            // 
            // lblFixedDesc
            // 
            this.lblFixedDesc.AutoSize = true;
            this.lblFixedDesc.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblFixedDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblFixedDesc.Location = new System.Drawing.Point(30, 85);
            this.lblFixedDesc.Name = "lblFixedDesc";
            this.lblFixedDesc.Size = new System.Drawing.Size(449, 23);
            this.lblFixedDesc.TabIndex = 2;
            this.lblFixedDesc.Text = "Đạt từ 30 giờ chơi tích lũy. Trừ thẳng vào từng khung giờ.";
            // 
            // lblFixedCount
            // 
            this.lblFixedCount.AutoSize = true;
            this.lblFixedCount.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblFixedCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.lblFixedCount.Location = new System.Drawing.Point(30, 110);
            this.lblFixedCount.Name = "lblFixedCount";
            this.lblFixedCount.Size = new System.Drawing.Size(106, 23);
            this.lblFixedCount.TabIndex = 3;
            this.lblFixedCount.Text = "● 0 Hội viên";
            // 
            // pnlCardWalkin
            // 
            this.pnlCardWalkin.Controls.Add(this.lblWalkinTitle);
            this.pnlCardWalkin.Controls.Add(this.lblWalkinValue);
            this.pnlCardWalkin.Controls.Add(this.lblWalkinDesc);
            this.pnlCardWalkin.Controls.Add(this.lblWalkinCount);
            this.pnlCardWalkin.FillColor = System.Drawing.Color.White;
            this.pnlCardWalkin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.pnlCardWalkin.Location = new System.Drawing.Point(590, 20);
            this.pnlCardWalkin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlCardWalkin.MinimumSize = new System.Drawing.Size(1, 1);
            this.pnlCardWalkin.Name = "pnlCardWalkin";
            this.pnlCardWalkin.Radius = 16;
            this.pnlCardWalkin.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.pnlCardWalkin.Size = new System.Drawing.Size(550, 140);
            this.pnlCardWalkin.TabIndex = 1;
            this.pnlCardWalkin.Text = null;
            this.pnlCardWalkin.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWalkinTitle
            // 
            this.lblWalkinTitle.AutoSize = true;
            this.lblWalkinTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblWalkinTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.lblWalkinTitle.Location = new System.Drawing.Point(30, 20);
            this.lblWalkinTitle.Name = "lblWalkinTitle";
            this.lblWalkinTitle.Size = new System.Drawing.Size(153, 23);
            this.lblWalkinTitle.TabIndex = 0;
            this.lblWalkinTitle.Text = "KHÁCH VÃNG LAI";
            // 
            // lblWalkinValue
            // 
            this.lblWalkinValue.AutoSize = true;
            this.lblWalkinValue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblWalkinValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblWalkinValue.Location = new System.Drawing.Point(25, 45);
            this.lblWalkinValue.Name = "lblWalkinValue";
            this.lblWalkinValue.Size = new System.Drawing.Size(219, 46);
            this.lblWalkinValue.TabIndex = 1;
            this.lblWalkinValue.Text = "Giá niêm yết";
            // 
            // lblWalkinDesc
            // 
            this.lblWalkinDesc.AutoSize = true;
            this.lblWalkinDesc.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblWalkinDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblWalkinDesc.Location = new System.Drawing.Point(30, 85);
            this.lblWalkinDesc.Name = "lblWalkinDesc";
            this.lblWalkinDesc.Size = new System.Drawing.Size(501, 23);
            this.lblWalkinDesc.TabIndex = 2;
            this.lblWalkinDesc.Text = "Giá theo khung giờ tiêu chuẩn. Không tài khoản hoặc ít bến bãi.";
            // 
            // lblWalkinCount
            // 
            this.lblWalkinCount.AutoSize = true;
            this.lblWalkinCount.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblWalkinCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.lblWalkinCount.Location = new System.Drawing.Point(30, 110);
            this.lblWalkinCount.Name = "lblWalkinCount";
            this.lblWalkinCount.Size = new System.Drawing.Size(106, 23);
            this.lblWalkinCount.TabIndex = 3;
            this.lblWalkinCount.Text = "● 0 Hội viên";
            // 
            // pnlList
            // 
            this.pnlList.Controls.Add(this.lblTabAll);
            this.pnlList.Controls.Add(this.lblTabFixed);
            this.pnlList.Controls.Add(this.lblTabWalkin);
            this.pnlList.Controls.Add(this.lblTabNew);
            this.pnlList.Controls.Add(this.pnlTabIndicator);
            this.pnlList.Controls.Add(this.picFilter);
            this.pnlList.Controls.Add(this.lstKhachHang);
            this.pnlList.FillColor = System.Drawing.Color.White;
            this.pnlList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.pnlList.Location = new System.Drawing.Point(20, 180);
            this.pnlList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlList.MinimumSize = new System.Drawing.Size(1, 1);
            this.pnlList.Name = "pnlList";
            this.pnlList.Radius = 16;
            this.pnlList.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.pnlList.Size = new System.Drawing.Size(1120, 420);
            this.pnlList.TabIndex = 2;
            this.pnlList.Text = null;
            this.pnlList.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTabAll
            // 
            this.lblTabAll.AutoSize = true;
            this.lblTabAll.BackColor = System.Drawing.Color.White;
            this.lblTabAll.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblTabAll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.lblTabAll.Location = new System.Drawing.Point(25, 20);
            this.lblTabAll.Name = "lblTabAll";
            this.lblTabAll.Size = new System.Drawing.Size(62, 25);
            this.lblTabAll.TabIndex = 0;
            this.lblTabAll.Text = "Tất cả";
            // 
            // lblTabFixed
            // 
            this.lblTabFixed.AutoSize = true;
            this.lblTabFixed.BackColor = System.Drawing.Color.White;
            this.lblTabFixed.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblTabFixed.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblTabFixed.Location = new System.Drawing.Point(85, 20);
            this.lblTabFixed.Name = "lblTabFixed";
            this.lblTabFixed.Size = new System.Drawing.Size(78, 25);
            this.lblTabFixed.TabIndex = 1;
            this.lblTabFixed.Text = "Cố định";
            // 
            // lblTabWalkin
            // 
            this.lblTabWalkin.AutoSize = true;
            this.lblTabWalkin.BackColor = System.Drawing.Color.White;
            this.lblTabWalkin.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblTabWalkin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblTabWalkin.Location = new System.Drawing.Point(160, 20);
            this.lblTabWalkin.Name = "lblTabWalkin";
            this.lblTabWalkin.Size = new System.Drawing.Size(80, 25);
            this.lblTabWalkin.TabIndex = 2;
            this.lblTabWalkin.Text = "Vãng lai";
            // 
            // lblTabNew
            // 
            this.lblTabNew.AutoSize = true;
            this.lblTabNew.BackColor = System.Drawing.Color.White;
            this.lblTabNew.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblTabNew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblTabNew.Location = new System.Drawing.Point(240, 20);
            this.lblTabNew.Name = "lblTabNew";
            this.lblTabNew.Size = new System.Drawing.Size(93, 25);
            this.lblTabNew.TabIndex = 3;
            this.lblTabNew.Text = "Mới (24h)";
            // 
            // pnlTabIndicator
            // 
            this.pnlTabIndicator.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.pnlTabIndicator.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.pnlTabIndicator.Location = new System.Drawing.Point(25, 45);
            this.pnlTabIndicator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlTabIndicator.MinimumSize = new System.Drawing.Size(1, 1);
            this.pnlTabIndicator.Name = "pnlTabIndicator";
            this.pnlTabIndicator.Radius = 3;
            this.pnlTabIndicator.RectColor = System.Drawing.Color.Transparent;
            this.pnlTabIndicator.Size = new System.Drawing.Size(50, 3);
            this.pnlTabIndicator.TabIndex = 4;
            this.pnlTabIndicator.Text = null;
            this.pnlTabIndicator.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lstKhachHang
            // 
            this.lstKhachHang.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstKhachHang.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lstKhachHang.FullRowSelect = true;
            this.lstKhachHang.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstKhachHang.HideSelection = false;
            this.lstKhachHang.Location = new System.Drawing.Point(20, 60);
            this.lstKhachHang.Name = "lstKhachHang";
            this.lstKhachHang.OwnerDraw = true;
            this.lstKhachHang.Size = new System.Drawing.Size(1080, 340);
            this.lstKhachHang.TabIndex = 6;
            this.lstKhachHang.UseCompatibleStateImageBehavior = false;
            this.lstKhachHang.View = System.Windows.Forms.View.Details;
            // 
            // pnlBot1
            // 
            this.pnlBot1.Controls.Add(this.lblBot1Title);
            this.pnlBot1.Controls.Add(this.lblBot1Value);
            this.pnlBot1.Controls.Add(this.lblBot1Desc);
            this.pnlBot1.FillColor = System.Drawing.Color.White;
            this.pnlBot1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.pnlBot1.Location = new System.Drawing.Point(20, 620);
            this.pnlBot1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlBot1.MinimumSize = new System.Drawing.Size(1, 1);
            this.pnlBot1.Name = "pnlBot1";
            this.pnlBot1.Radius = 16;
            this.pnlBot1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.pnlBot1.Size = new System.Drawing.Size(265, 110);
            this.pnlBot1.TabIndex = 3;
            this.pnlBot1.Text = null;
            this.pnlBot1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBot1Title
            // 
            this.lblBot1Title.AutoSize = true;
            this.lblBot1Title.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblBot1Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblBot1Title.Location = new System.Drawing.Point(15, 15);
            this.lblBot1Title.Name = "lblBot1Title";
            this.lblBot1Title.Size = new System.Drawing.Size(129, 21);
            this.lblBot1Title.TabIndex = 0;
            this.lblBot1Title.Text = "Tổng khách hàng";
            // 
            // lblBot1Value
            // 
            this.lblBot1Value.AutoSize = true;
            this.lblBot1Value.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblBot1Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblBot1Value.Location = new System.Drawing.Point(10, 40);
            this.lblBot1Value.Name = "lblBot1Value";
            this.lblBot1Value.Size = new System.Drawing.Size(35, 41);
            this.lblBot1Value.TabIndex = 1;
            this.lblBot1Value.Text = "0";
            // 
            // lblBot1Desc
            // 
            this.lblBot1Desc.AutoSize = true;
            this.lblBot1Desc.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBot1Desc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.lblBot1Desc.Location = new System.Drawing.Point(15, 80);
            this.lblBot1Desc.Name = "lblBot1Desc";
            this.lblBot1Desc.Size = new System.Drawing.Size(131, 20);
            this.lblBot1Desc.TabIndex = 2;
            this.lblBot1Desc.Text = "↗ +12% tháng này";
            // 
            // pnlBot2
            // 
            this.pnlBot2.Controls.Add(this.lblBot2Title);
            this.pnlBot2.Controls.Add(this.lblBot2Value);
            this.pnlBot2.Controls.Add(this.lblBot2Desc);
            this.pnlBot2.FillColor = System.Drawing.Color.White;
            this.pnlBot2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.pnlBot2.Location = new System.Drawing.Point(305, 620);
            this.pnlBot2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlBot2.MinimumSize = new System.Drawing.Size(1, 1);
            this.pnlBot2.Name = "pnlBot2";
            this.pnlBot2.Radius = 16;
            this.pnlBot2.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.pnlBot2.Size = new System.Drawing.Size(265, 110);
            this.pnlBot2.TabIndex = 4;
            this.pnlBot2.Text = null;
            this.pnlBot2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBot2Title
            // 
            this.lblBot2Title.AutoSize = true;
            this.lblBot2Title.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblBot2Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblBot2Title.Location = new System.Drawing.Point(15, 15);
            this.lblBot2Title.Name = "lblBot2Title";
            this.lblBot2Title.Size = new System.Drawing.Size(107, 21);
            this.lblBot2Title.TabIndex = 0;
            this.lblBot2Title.Text = "Khách cố định";
            // 
            // lblBot2Value
            // 
            this.lblBot2Value.AutoSize = true;
            this.lblBot2Value.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblBot2Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblBot2Value.Location = new System.Drawing.Point(10, 40);
            this.lblBot2Value.Name = "lblBot2Value";
            this.lblBot2Value.Size = new System.Drawing.Size(35, 41);
            this.lblBot2Value.TabIndex = 1;
            this.lblBot2Value.Text = "0";
            // 
            // lblBot2Desc
            // 
            this.lblBot2Desc.AutoSize = true;
            this.lblBot2Desc.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBot2Desc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.lblBot2Desc.Location = new System.Drawing.Point(15, 80);
            this.lblBot2Desc.Name = "lblBot2Desc";
            this.lblBot2Desc.Size = new System.Drawing.Size(98, 20);
            this.lblBot2Desc.TabIndex = 2;
            this.lblBot2Desc.Text = "≥ 30h tích luỹ";
            // 
            // pnlBot3
            // 
            this.pnlBot3.Controls.Add(this.lblBot3Title);
            this.pnlBot3.Controls.Add(this.lblBot3Value);
            this.pnlBot3.Controls.Add(this.lblBot3Desc);
            this.pnlBot3.FillColor = System.Drawing.Color.White;
            this.pnlBot3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.pnlBot3.Location = new System.Drawing.Point(590, 620);
            this.pnlBot3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlBot3.MinimumSize = new System.Drawing.Size(1, 1);
            this.pnlBot3.Name = "pnlBot3";
            this.pnlBot3.Radius = 16;
            this.pnlBot3.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.pnlBot3.Size = new System.Drawing.Size(265, 110);
            this.pnlBot3.TabIndex = 5;
            this.pnlBot3.Text = null;
            this.pnlBot3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBot3Title
            // 
            this.lblBot3Title.AutoSize = true;
            this.lblBot3Title.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblBot3Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblBot3Title.Location = new System.Drawing.Point(15, 15);
            this.lblBot3Title.Name = "lblBot3Title";
            this.lblBot3Title.Size = new System.Drawing.Size(121, 21);
            this.lblBot3Title.TabIndex = 0;
            this.lblBot3Title.Text = "Doanh thu CRM";
            // 
            // lblBot3Value
            // 
            this.lblBot3Value.AutoSize = true;
            this.lblBot3Value.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblBot3Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblBot3Value.Location = new System.Drawing.Point(10, 40);
            this.lblBot3Value.Name = "lblBot3Value";
            this.lblBot3Value.Size = new System.Drawing.Size(54, 41);
            this.lblBot3Value.TabIndex = 1;
            this.lblBot3Value.Text = "0đ";
            // 
            // lblBot3Desc
            // 
            this.lblBot3Desc.AutoSize = true;
            this.lblBot3Desc.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBot3Desc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.lblBot3Desc.Location = new System.Drawing.Point(15, 80);
            this.lblBot3Desc.Name = "lblBot3Desc";
            this.lblBot3Desc.Size = new System.Drawing.Size(62, 20);
            this.lblBot3Desc.TabIndex = 2;
            this.lblBot3Desc.Text = "↗ +18%";
            // 
            // pnlBot4
            // 
            this.pnlBot4.Controls.Add(this.lblBot4Title);
            this.pnlBot4.Controls.Add(this.lblBot4Value);
            this.pnlBot4.Controls.Add(this.lblBot4Desc);
            this.pnlBot4.FillColor = System.Drawing.Color.White;
            this.pnlBot4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.pnlBot4.Location = new System.Drawing.Point(875, 620);
            this.pnlBot4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlBot4.MinimumSize = new System.Drawing.Size(1, 1);
            this.pnlBot4.Name = "pnlBot4";
            this.pnlBot4.Radius = 16;
            this.pnlBot4.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.pnlBot4.Size = new System.Drawing.Size(265, 110);
            this.pnlBot4.TabIndex = 6;
            this.pnlBot4.Text = null;
            this.pnlBot4.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBot4Title
            // 
            this.lblBot4Title.AutoSize = true;
            this.lblBot4Title.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblBot4Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblBot4Title.Location = new System.Drawing.Point(15, 15);
            this.lblBot4Title.Name = "lblBot4Title";
            this.lblBot4Title.Size = new System.Drawing.Size(111, 21);
            this.lblBot4Title.TabIndex = 0;
            this.lblBot4Title.Text = "Hoạt động sân";
            // 
            // lblBot4Value
            // 
            this.lblBot4Value.AutoSize = true;
            this.lblBot4Value.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblBot4Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblBot4Value.Location = new System.Drawing.Point(10, 40);
            this.lblBot4Value.Name = "lblBot4Value";
            this.lblBot4Value.Size = new System.Drawing.Size(61, 41);
            this.lblBot4Value.TabIndex = 1;
            this.lblBot4Value.Text = "0%";
            // 
            // lblBot4Desc
            // 
            this.lblBot4Desc.AutoSize = true;
            this.lblBot4Desc.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBot4Desc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblBot4Desc.Location = new System.Drawing.Point(15, 80);
            this.lblBot4Desc.Name = "lblBot4Desc";
            this.lblBot4Desc.Size = new System.Drawing.Size(147, 20);
            this.lblBot4Desc.TabIndex = 2;
            this.lblBot4Desc.Text = "Công suất trung bình";
            // 
            // picFilter
            // 
            this.picFilter.BackColor = System.Drawing.Color.White;
            this.picFilter.Location = new System.Drawing.Point(1070, 15);
            this.picFilter.Name = "picFilter";
            this.picFilter.Size = new System.Drawing.Size(30, 30);
            this.picFilter.TabIndex = 5;
            this.picFilter.TabStop = false;
            // 
            // UCKhachHang
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.pnlCardFixed);
            this.Controls.Add(this.pnlCardWalkin);
            this.Controls.Add(this.pnlList);
            this.Controls.Add(this.pnlBot1);
            this.Controls.Add(this.pnlBot2);
            this.Controls.Add(this.pnlBot3);
            this.Controls.Add(this.pnlBot4);
            this.Name = "UCKhachHang";
            this.Size = new System.Drawing.Size(1160, 820);
            this.pnlCardFixed.ResumeLayout(false);
            this.pnlCardFixed.PerformLayout();
            this.pnlCardWalkin.ResumeLayout(false);
            this.pnlCardWalkin.PerformLayout();
            this.pnlList.ResumeLayout(false);
            this.pnlList.PerformLayout();
            this.pnlBot1.ResumeLayout(false);
            this.pnlBot1.PerformLayout();
            this.pnlBot2.ResumeLayout(false);
            this.pnlBot2.PerformLayout();
            this.pnlBot3.ResumeLayout(false);
            this.pnlBot3.PerformLayout();
            this.pnlBot4.ResumeLayout(false);
            this.pnlBot4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFilter)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
