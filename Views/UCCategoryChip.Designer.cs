using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Sunny.UI;

namespace DemoPick
{
    partial class UCCategoryChip
    {
        private IContainer components = null;

        public UIButton btnChip;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.btnChip = new Sunny.UI.UIButton();
            this.SuspendLayout();

            // 
            // btnChip
            // 
            this.btnChip.Cursor = Cursors.Hand;
            this.btnChip.FillColor = Color.White;
            this.btnChip.FillHoverColor = Color.FromArgb(240, 240, 240);
            this.btnChip.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnChip.ForeColor = Color.FromArgb(107, 114, 128);
            this.btnChip.ForeHoverColor = Color.FromArgb(107, 114, 128);
            this.btnChip.Location = new Point(0, 0);
            this.btnChip.MinimumSize = new Size(1, 1);
            this.btnChip.Name = "btnChip";
            this.btnChip.Radius = 18;
            this.btnChip.RectColor = Color.FromArgb(229, 231, 235);
            this.btnChip.Size = new Size(100, 35);
            this.btnChip.TabIndex = 0;
            this.btnChip.Text = "Danh mục";
            this.btnChip.TipsFont = new Font("Segoe UI", 9F);

            //
            // UCCategoryChip
            //
            this.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = Color.Transparent;
            this.Controls.Add(this.btnChip);
            this.Name = "UCCategoryChip";
            this.Size = new Size(100, 35);
            this.ResumeLayout(false);
        }
    }
}
