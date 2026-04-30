using DemoPick.Helpers;
using DemoPick.Data;
namespace DemoPick
{
    partial class UCAuditLog
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvLogs;
        private System.Windows.Forms.Label lblTitle;

        // KPI cards
        private Sunny.UI.UIPanel cardTotal;
        private System.Windows.Forms.Label lblCardTotalValue;
        private System.Windows.Forms.Label lblCardTotalLabel;

        private Sunny.UI.UIPanel cardPos;
        private System.Windows.Forms.Label lblCardPosValue;
        private System.Windows.Forms.Label lblCardPosLabel;

        private Sunny.UI.UIPanel cardInventory;
        private System.Windows.Forms.Label lblCardInventoryValue;
        private System.Windows.Forms.Label lblCardInventoryLabel;

        private Sunny.UI.UIPanel cardError;
        private System.Windows.Forms.Label lblCardErrorValue;
        private System.Windows.Forms.Label lblCardErrorLabel;

        // Search
        private Sunny.UI.UITextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvLogs = new System.Windows.Forms.DataGridView();
            this.lblTitle = new System.Windows.Forms.Label();

            this.cardTotal = new Sunny.UI.UIPanel();
            this.lblCardTotalValue = new System.Windows.Forms.Label();
            this.lblCardTotalLabel = new System.Windows.Forms.Label();

            this.cardPos = new Sunny.UI.UIPanel();
            this.lblCardPosValue = new System.Windows.Forms.Label();
            this.lblCardPosLabel = new System.Windows.Forms.Label();

            this.cardInventory = new Sunny.UI.UIPanel();
            this.lblCardInventoryValue = new System.Windows.Forms.Label();
            this.lblCardInventoryLabel = new System.Windows.Forms.Label();

            this.cardError = new Sunny.UI.UIPanel();
            this.lblCardErrorValue = new System.Windows.Forms.Label();
            this.lblCardErrorLabel = new System.Windows.Forms.Label();

            this.txtSearch = new Sunny.UI.UITextBox();
            this.lblSearch = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(26, 35, 50);
            this.lblTitle.Location = new System.Drawing.Point(24, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(242, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Nhật ký hoạt động";
            // 
            // === KPI CARD: Total ===
            // 
            this.cardTotal.BackColor = System.Drawing.Color.White;
            this.cardTotal.FillColor = System.Drawing.Color.White;
            this.cardTotal.Location = new System.Drawing.Point(24, 70);
            this.cardTotal.Name = "cardTotal";
            this.cardTotal.Radius = 12;
            this.cardTotal.RectColor = System.Drawing.Color.FromArgb(229, 231, 235);
            this.cardTotal.Size = new System.Drawing.Size(250, 80);
            this.cardTotal.TabIndex = 10;
            this.cardTotal.Controls.Add(this.lblCardTotalValue);
            this.cardTotal.Controls.Add(this.lblCardTotalLabel);

            this.lblCardTotalValue.AutoSize = true;
            this.lblCardTotalValue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblCardTotalValue.ForeColor = System.Drawing.Color.FromArgb(26, 35, 50);
            this.lblCardTotalValue.Location = new System.Drawing.Point(16, 10);
            this.lblCardTotalValue.Name = "lblCardTotalValue";
            this.lblCardTotalValue.Text = "0";

            this.lblCardTotalLabel.AutoSize = true;
            this.lblCardTotalLabel.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCardTotalLabel.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.lblCardTotalLabel.Location = new System.Drawing.Point(16, 52);
            this.lblCardTotalLabel.Name = "lblCardTotalLabel";
            this.lblCardTotalLabel.Text = "Tổng sự kiện hôm nay";
            // 
            // === KPI CARD: POS ===
            // 
            this.cardPos.BackColor = System.Drawing.Color.White;
            this.cardPos.FillColor = System.Drawing.Color.White;
            this.cardPos.Location = new System.Drawing.Point(290, 70);
            this.cardPos.Name = "cardPos";
            this.cardPos.Radius = 12;
            this.cardPos.RectColor = System.Drawing.Color.FromArgb(229, 231, 235);
            this.cardPos.Size = new System.Drawing.Size(250, 80);
            this.cardPos.TabIndex = 11;
            this.cardPos.Controls.Add(this.lblCardPosValue);
            this.cardPos.Controls.Add(this.lblCardPosLabel);

            this.lblCardPosValue.AutoSize = true;
            this.lblCardPosValue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblCardPosValue.ForeColor = System.Drawing.Color.FromArgb(22, 163, 74);
            this.lblCardPosValue.Location = new System.Drawing.Point(16, 10);
            this.lblCardPosValue.Name = "lblCardPosValue";
            this.lblCardPosValue.Text = "0";

            this.lblCardPosLabel.AutoSize = true;
            this.lblCardPosLabel.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCardPosLabel.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.lblCardPosLabel.Location = new System.Drawing.Point(16, 52);
            this.lblCardPosLabel.Name = "lblCardPosLabel";
            this.lblCardPosLabel.Text = "Thanh toán POS";
            // 
            // === KPI CARD: Inventory ===
            // 
            this.cardInventory.BackColor = System.Drawing.Color.White;
            this.cardInventory.FillColor = System.Drawing.Color.White;
            this.cardInventory.Location = new System.Drawing.Point(556, 70);
            this.cardInventory.Name = "cardInventory";
            this.cardInventory.Radius = 12;
            this.cardInventory.RectColor = System.Drawing.Color.FromArgb(229, 231, 235);
            this.cardInventory.Size = new System.Drawing.Size(250, 80);
            this.cardInventory.TabIndex = 12;
            this.cardInventory.Controls.Add(this.lblCardInventoryValue);
            this.cardInventory.Controls.Add(this.lblCardInventoryLabel);

            this.lblCardInventoryValue.AutoSize = true;
            this.lblCardInventoryValue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblCardInventoryValue.ForeColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.lblCardInventoryValue.Location = new System.Drawing.Point(16, 10);
            this.lblCardInventoryValue.Name = "lblCardInventoryValue";
            this.lblCardInventoryValue.Text = "0";

            this.lblCardInventoryLabel.AutoSize = true;
            this.lblCardInventoryLabel.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCardInventoryLabel.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.lblCardInventoryLabel.Location = new System.Drawing.Point(16, 52);
            this.lblCardInventoryLabel.Name = "lblCardInventoryLabel";
            this.lblCardInventoryLabel.Text = "Nhập / Xuất kho";
            // 
            // === KPI CARD: Error ===
            // 
            this.cardError.BackColor = System.Drawing.Color.White;
            this.cardError.FillColor = System.Drawing.Color.White;
            this.cardError.Location = new System.Drawing.Point(822, 70);
            this.cardError.Name = "cardError";
            this.cardError.Radius = 12;
            this.cardError.RectColor = System.Drawing.Color.FromArgb(229, 231, 235);
            this.cardError.Size = new System.Drawing.Size(250, 80);
            this.cardError.TabIndex = 13;
            this.cardError.Controls.Add(this.lblCardErrorValue);
            this.cardError.Controls.Add(this.lblCardErrorLabel);

            this.lblCardErrorValue.AutoSize = true;
            this.lblCardErrorValue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblCardErrorValue.ForeColor = System.Drawing.Color.FromArgb(220, 38, 38);
            this.lblCardErrorValue.Location = new System.Drawing.Point(16, 10);
            this.lblCardErrorValue.Name = "lblCardErrorValue";
            this.lblCardErrorValue.Text = "0";

            this.lblCardErrorLabel.AutoSize = true;
            this.lblCardErrorLabel.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCardErrorLabel.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.lblCardErrorLabel.Location = new System.Drawing.Point(16, 52);
            this.lblCardErrorLabel.Name = "lblCardErrorLabel";
            this.lblCardErrorLabel.Text = "Lỗi / Cảnh báo";
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSearch.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.lblSearch.Location = new System.Drawing.Point(24, 168);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Text = "Tìm kiếm:";
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtSearch.Location = new System.Drawing.Point(110, 164);
            this.txtSearch.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Padding = new System.Windows.Forms.Padding(5);
            this.txtSearch.Radius = 6;
            this.txtSearch.ShowText = false;
            this.txtSearch.Size = new System.Drawing.Size(350, 36);
            this.txtSearch.TabIndex = 14;
            this.txtSearch.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtSearch.Watermark = "Nhập từ khóa: tên sự kiện, chi tiết...";
            // 
            // dgvLogs
            // 
            this.dgvLogs.AllowUserToAddRows = false;
            this.dgvLogs.AllowUserToDeleteRows = false;
            this.dgvLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLogs.BackgroundColor = System.Drawing.Color.White;
            this.dgvLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLogs.Location = new System.Drawing.Point(24, 210);
            this.dgvLogs.Name = "dgvLogs";
            this.dgvLogs.ReadOnly = true;
            this.dgvLogs.RowHeadersVisible = false;
            this.dgvLogs.RowHeadersWidth = 51;
            this.dgvLogs.RowTemplate.Height = 38;
            this.dgvLogs.Size = new System.Drawing.Size(1150, 570);
            this.dgvLogs.TabIndex = 1;
            this.dgvLogs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvLogs.BackgroundColor = System.Drawing.Color.FromArgb(250, 250, 250);
            this.dgvLogs.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.dgvLogs.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.dgvLogs.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.dgvLogs.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(249, 250, 251);
            this.dgvLogs.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.dgvLogs.ColumnHeadersDefaultCellStyle.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.dgvLogs.EnableHeadersVisualStyles = false;
            this.dgvLogs.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvLogs.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvLogs.GridColor = System.Drawing.Color.FromArgb(238, 238, 238);
            this.dgvLogs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLogs.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(238, 242, 255);
            this.dgvLogs.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(26, 35, 50);
            this.dgvLogs.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(249, 250, 251);
            // 
            // UCAuditLog
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.cardError);
            this.Controls.Add(this.cardInventory);
            this.Controls.Add(this.cardPos);
            this.Controls.Add(this.cardTotal);
            this.Controls.Add(this.dgvLogs);
            this.Controls.Add(this.lblTitle);
            this.Name = "UCAuditLog";
            this.Size = new System.Drawing.Size(1200, 800);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
