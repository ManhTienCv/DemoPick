namespace DemoPick
{
    partial class UCPaymentHistoryPanel
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtSearch = new Sunny.UI.UITextBox();
            this.btnSearch = new Sunny.UI.UIButton();
            this.lstHistory = new System.Windows.Forms.ListView();
            this.colInvoiceId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCustomer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTotal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnOpen = new Sunny.UI.UIButton();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(145, 23);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Lịch sử thanh toán";
            // 
            // txtSearch
            // 
            this.txtSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSearch.Location = new System.Drawing.Point(0, 30);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSearch.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Padding = new System.Windows.Forms.Padding(8, 4, 4, 4);
            this.txtSearch.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.txtSearch.ShowText = false;
            this.txtSearch.Size = new System.Drawing.Size(188, 32);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtSearch.Watermark = "Tìm mã/SĐT/khách";
            // 
            // btnSearch
            // 
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.btnSearch.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(194, 30);
            this.btnSearch.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Radius = 8;
            this.btnSearch.RectColor = System.Drawing.Color.Transparent;
            this.btnSearch.RectHoverColor = System.Drawing.Color.Transparent;
            this.btnSearch.Size = new System.Drawing.Size(86, 32);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Lọc";
            this.btnSearch.TipsFont = new System.Drawing.Font("Segoe UI", 9F);
            // 
            // lstHistory
            // 
            this.lstHistory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colInvoiceId,
            this.colTime,
            this.colCustomer,
            this.colTotal});
            this.lstHistory.FullRowSelect = true;
            this.lstHistory.GridLines = false;
            this.lstHistory.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstHistory.HideSelection = false;
            this.lstHistory.Location = new System.Drawing.Point(0, 68);
            this.lstHistory.MultiSelect = false;
            this.lstHistory.Name = "lstHistory";
            this.lstHistory.ShowItemToolTips = true;
            this.lstHistory.Size = new System.Drawing.Size(280, 190);
            this.lstHistory.TabIndex = 3;
            this.lstHistory.UseCompatibleStateImageBehavior = false;
            this.lstHistory.View = System.Windows.Forms.View.Details;
            // 
            // colInvoiceId
            // 
            this.colInvoiceId.Text = "Mã";
            this.colInvoiceId.Width = 48;
            // 
            // colTime
            // 
            this.colTime.Text = "Giờ";
            this.colTime.Width = 84;
            // 
            // colCustomer
            // 
            this.colCustomer.Text = "Khách";
            this.colCustomer.Width = 82;
            // 
            // colTotal
            // 
            this.colTotal.Text = "Tổng";
            this.colTotal.Width = 64;
            // 
            // btnOpen
            // 
            this.btnOpen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpen.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnOpen.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(150)))), ((int)(((byte)(105)))));
            this.btnOpen.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnOpen.ForeColor = System.Drawing.Color.White;
            this.btnOpen.Location = new System.Drawing.Point(0, 264);
            this.btnOpen.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Radius = 10;
            this.btnOpen.RectColor = System.Drawing.Color.Transparent;
            this.btnOpen.RectHoverColor = System.Drawing.Color.Transparent;
            this.btnOpen.Size = new System.Drawing.Size(280, 36);
            this.btnOpen.TabIndex = 4;
            this.btnOpen.Text = "Mở / In hóa đơn đã chọn";
            this.btnOpen.TipsFont = new System.Drawing.Font("Segoe UI", 9F);
            // 
            // UCPaymentHistoryPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.lstHistory);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.lblTitle);
            this.Name = "UCPaymentHistoryPanel";
            this.Size = new System.Drawing.Size(280, 300);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblTitle;
        private Sunny.UI.UITextBox txtSearch;
        private Sunny.UI.UIButton btnSearch;
        private System.Windows.Forms.ListView lstHistory;
        private System.Windows.Forms.ColumnHeader colInvoiceId;
        private System.Windows.Forms.ColumnHeader colTime;
        private System.Windows.Forms.ColumnHeader colCustomer;
        private System.Windows.Forms.ColumnHeader colTotal;
        private Sunny.UI.UIButton btnOpen;
    }
}