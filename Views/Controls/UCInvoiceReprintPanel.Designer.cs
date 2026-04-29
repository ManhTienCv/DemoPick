using DemoPick.Helpers;
using DemoPick.Data;
namespace DemoPick
{
    partial class UCInvoiceReprintPanel
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
            this.txtInvoiceId = new Sunny.UI.UITextBox();
            this.btnReprintById = new Sunny.UI.UIButton();
            this.btnReprintLast = new Sunny.UI.UIButton();
            this.SuspendLayout();
            // 
            // txtInvoiceId
            // 
            this.txtInvoiceId.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtInvoiceId.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtInvoiceId.Location = new System.Drawing.Point(0, 0);
            this.txtInvoiceId.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtInvoiceId.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtInvoiceId.Name = "txtInvoiceId";
            this.txtInvoiceId.Padding = new System.Windows.Forms.Padding(8, 4, 4, 4);
            this.txtInvoiceId.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.txtInvoiceId.ShowText = false;
            this.txtInvoiceId.Size = new System.Drawing.Size(120, 32);
            this.txtInvoiceId.TabIndex = 0;
            this.txtInvoiceId.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtInvoiceId.Watermark = "Mã HĐ";
            // 
            // btnReprintById
            // 
            this.btnReprintById.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReprintById.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.btnReprintById.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnReprintById.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnReprintById.ForeColor = System.Drawing.Color.White;
            this.btnReprintById.Location = new System.Drawing.Point(125, 0);
            this.btnReprintById.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnReprintById.Name = "btnReprintById";
            this.btnReprintById.Radius = 8;
            this.btnReprintById.RectColor = System.Drawing.Color.Transparent;
            this.btnReprintById.RectHoverColor = System.Drawing.Color.Transparent;
            this.btnReprintById.Size = new System.Drawing.Size(80, 32);
            this.btnReprintById.TabIndex = 1;
            this.btnReprintById.Text = "IN MÃ";
            this.btnReprintById.TipsFont = new System.Drawing.Font("Segoe UI", 9F);
            // 
            // btnReprintLast
            // 
            this.btnReprintLast.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReprintLast.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.btnReprintLast.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            this.btnReprintLast.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnReprintLast.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.btnReprintLast.Location = new System.Drawing.Point(0, 40);
            this.btnReprintLast.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnReprintLast.Name = "btnReprintLast";
            this.btnReprintLast.Radius = 15;
            this.btnReprintLast.RectColor = System.Drawing.Color.Transparent;
            this.btnReprintLast.RectHoverColor = System.Drawing.Color.Transparent;
            this.btnReprintLast.Size = new System.Drawing.Size(205, 32);
            this.btnReprintLast.TabIndex = 2;
            this.btnReprintLast.Text = "IN LẠI HĐ";
            this.btnReprintLast.TipsFont = new System.Drawing.Font("Segoe UI", 9F);
            // 
            // UCInvoiceReprintPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnReprintLast);
            this.Controls.Add(this.btnReprintById);
            this.Controls.Add(this.txtInvoiceId);
            this.Name = "UCInvoiceReprintPanel";
            this.Size = new System.Drawing.Size(205, 72);
            this.ResumeLayout(false);

        }

        private Sunny.UI.UITextBox txtInvoiceId;
        private Sunny.UI.UIButton btnReprintById;
        private Sunny.UI.UIButton btnReprintLast;
    }
}
