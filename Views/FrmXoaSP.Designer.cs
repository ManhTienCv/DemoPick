namespace DemoPick
{
    partial class FrmXoaSP
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel _pnlTop;
        private System.Windows.Forms.Label _lblTitle;
        private System.Windows.Forms.Panel _pnlBottom;
        private System.Windows.Forms.ColumnHeader _colId;
        private System.Windows.Forms.ColumnHeader _colName;
        private System.Windows.Forms.ColumnHeader _colCategory;
        private System.Windows.Forms.ColumnHeader _colPrice;
        private System.Windows.Forms.ColumnHeader _colStock;

        private Sunny.UI.UIButton _btnDelete;
        private Sunny.UI.UIButton _btnClose;

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
            this._pnlTop = new System.Windows.Forms.Panel();
            this._lblTitle = new System.Windows.Forms.Label();
            this._lstProducts = new System.Windows.Forms.ListView();
            this._colId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._colCategory = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._colPrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._colStock = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._pnlBottom = new System.Windows.Forms.Panel();
            this._btnClose = new Sunny.UI.UIButton();
            this._btnDelete = new Sunny.UI.UIButton();
            this._pnlTop.SuspendLayout();
            this._pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // _pnlTop
            // 
            this._pnlTop.Controls.Add(this._lblTitle);
            this._pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnlTop.Location = new System.Drawing.Point(0, 35);
            this._pnlTop.Name = "_pnlTop";
            this._pnlTop.Padding = new System.Windows.Forms.Padding(12, 10, 12, 0);
            this._pnlTop.Size = new System.Drawing.Size(1024, 48);
            this._pnlTop.TabIndex = 0;
            // 
            // _lblTitle
            // 
            this._lblTitle.AutoSize = true;
            this._lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this._lblTitle.Location = new System.Drawing.Point(12, 10);
            this._lblTitle.Name = "_lblTitle";
            this._lblTitle.Size = new System.Drawing.Size(239, 28);
            this._lblTitle.TabIndex = 0;
            this._lblTitle.Text = "Danh sách sản phẩm để xóa";
            // 
            // _lstProducts
            // 
            this._lstProducts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._lstProducts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._colId,
            this._colName,
            this._colCategory,
            this._colPrice,
            this._colStock});
            this._lstProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lstProducts.Font = new System.Drawing.Font("Segoe UI", 10F);
            this._lstProducts.FullRowSelect = true;
            this._lstProducts.HideSelection = false;
            this._lstProducts.Location = new System.Drawing.Point(0, 83);
            this._lstProducts.MultiSelect = false;
            this._lstProducts.Name = "_lstProducts";
            this._lstProducts.Size = new System.Drawing.Size(1024, 453);
            this._lstProducts.TabIndex = 1;
            this._lstProducts.UseCompatibleStateImageBehavior = false;
            this._lstProducts.View = System.Windows.Forms.View.Details;
            // 
            // _colId
            // 
            this._colId.Text = "ID";
            this._colId.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._colId.Width = 70;
            // 
            // _colName
            // 
            this._colName.Text = "Tên";
            this._colName.Width = 320;
            // 
            // _colCategory
            // 
            this._colCategory.Text = "Danh mục";
            this._colCategory.Width = 180;
            // 
            // _colPrice
            // 
            this._colPrice.Text = "Giá";
            this._colPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._colPrice.Width = 120;
            // 
            // _colStock
            // 
            this._colStock.Text = "Tồn";
            this._colStock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._colStock.Width = 80;
            // 
            // _pnlBottom
            // 
            this._pnlBottom.Controls.Add(this._btnClose);
            this._pnlBottom.Controls.Add(this._btnDelete);
            this._pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._pnlBottom.Location = new System.Drawing.Point(0, 536);
            this._pnlBottom.Name = "_pnlBottom";
            this._pnlBottom.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this._pnlBottom.Size = new System.Drawing.Size(1024, 64);
            this._pnlBottom.TabIndex = 2;
            // 
            // _btnClose
            // 
            this._btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnClose.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this._btnClose.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(134)))), ((int)(((byte)(148)))));
            this._btnClose.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this._btnClose.Location = new System.Drawing.Point(892, 10);
            this._btnClose.MinimumSize = new System.Drawing.Size(1, 1);
            this._btnClose.Name = "_btnClose";
            this._btnClose.Radius = 14;
            this._btnClose.RectColor = System.Drawing.Color.Transparent;
            this._btnClose.Size = new System.Drawing.Size(120, 35);
            this._btnClose.TabIndex = 0;
            this._btnClose.Text = "Đóng";
            this._btnClose.TipsFont = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            // 
            // _btnDelete
            // 
            this._btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnDelete.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this._btnDelete.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(86)))), ((int)(((byte)(70)))));
            this._btnDelete.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this._btnDelete.Location = new System.Drawing.Point(702, 10);
            this._btnDelete.MinimumSize = new System.Drawing.Size(1, 1);
            this._btnDelete.Name = "_btnDelete";
            this._btnDelete.Radius = 14;
            this._btnDelete.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this._btnDelete.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(86)))), ((int)(((byte)(70)))));
            this._btnDelete.Size = new System.Drawing.Size(180, 35);
            this._btnDelete.TabIndex = 1;
            this._btnDelete.Text = "Xóa sản phẩm";
            this._btnDelete.TipsFont = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            // 
            // FrmXoaSP
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1024, 600);
            this.Controls.Add(this._lstProducts);
            this.Controls.Add(this._pnlBottom);
            this.Controls.Add(this._pnlTop);
            this.MinimumSize = new System.Drawing.Size(900, 520);
            this.Name = "FrmXoaSP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Xóa sản phẩm";
            this._pnlTop.ResumeLayout(false);
            this._pnlTop.PerformLayout();
            this._pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.ListView _lstProducts;
    }
}