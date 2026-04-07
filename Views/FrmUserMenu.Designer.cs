using System.Drawing;
using System.Windows.Forms;
using Sunny.UI;

namespace DemoPick
{
    partial class FrmUserMenu
    {
        private System.ComponentModel.IContainer components = null;

        private UIButton btnDoiMatKhau;
        private UIButton btnDangXuat;
        private UIButton btnThoat;
        private Panel pnlBorder;

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
            this.pnlBorder = new System.Windows.Forms.Panel();
            this.btnDoiMatKhau = new Sunny.UI.UIButton();
            this.btnDangXuat = new Sunny.UI.UIButton();
            this.btnThoat = new Sunny.UI.UIButton();
            
            this.pnlBorder.SuspendLayout();
            this.SuspendLayout();

            // pnlBorder
            this.pnlBorder.BackColor = System.Drawing.Color.White;
            this.pnlBorder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBorder.Controls.Add(this.btnThoat);
            this.pnlBorder.Controls.Add(this.btnDangXuat);
            this.pnlBorder.Controls.Add(this.btnDoiMatKhau);
            // btnDoiMatKhau
            this.btnDoiMatKhau.Text = "🔑  Đổi mật khẩu";
            this.btnDoiMatKhau.Location = new System.Drawing.Point(5, 5);
            this.btnDoiMatKhau.Size = new System.Drawing.Size(190, 40);
            this.btnDoiMatKhau.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnDoiMatKhau.FillColor = System.Drawing.Color.White;
            this.btnDoiMatKhau.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.btnDoiMatKhau.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.btnDoiMatKhau.ForeHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.btnDoiMatKhau.RectColor = System.Drawing.Color.Transparent;
            this.btnDoiMatKhau.RectHoverColor = System.Drawing.Color.Transparent;
            this.btnDoiMatKhau.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDoiMatKhau.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDoiMatKhau.Click += new System.EventHandler(this.btnDoiMatKhau_Click);

            // btnDangXuat
            this.btnDangXuat.Text = "↩️  Đăng xuất";
            this.btnDangXuat.Location = new System.Drawing.Point(5, 50);
            this.btnDangXuat.Size = new System.Drawing.Size(190, 40);
            this.btnDangXuat.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnDangXuat.FillColor = System.Drawing.Color.White;
            this.btnDangXuat.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.btnDangXuat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.btnDangXuat.ForeHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.btnDangXuat.RectColor = System.Drawing.Color.Transparent;
            this.btnDangXuat.RectHoverColor = System.Drawing.Color.Transparent;
            this.btnDangXuat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDangXuat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDangXuat.Click += new System.EventHandler(this.btnDangXuat_Click);

            // btnThoat
            this.btnThoat.Text = "🚪  Thoát hệ thống";
            this.btnThoat.Location = new System.Drawing.Point(5, 95);
            this.btnThoat.Size = new System.Drawing.Size(190, 40);
            this.btnThoat.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnThoat.FillColor = System.Drawing.Color.White;
            this.btnThoat.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.btnThoat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.btnThoat.ForeHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.btnThoat.RectColor = System.Drawing.Color.Transparent;
            this.btnThoat.RectHoverColor = System.Drawing.Color.Transparent;
            this.btnThoat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThoat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);

            // FrmUserMenu
            this.ClientSize = new System.Drawing.Size(200, 140);
            this.Controls.Add(this.pnlBorder);
            this.Name = "FrmUserMenu";
            
            this.pnlBorder.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
