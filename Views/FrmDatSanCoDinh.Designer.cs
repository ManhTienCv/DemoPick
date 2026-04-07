using System.Drawing;
using System.Windows.Forms;
using Sunny.UI;

namespace DemoPick
{
    partial class FrmDatSanCoDinh
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Label lblTitle;
        private Label btnCancelTop;
        
        private UIRadioButton rbKhachThue;
        private UIRadioButton rbBaoTri;
        
        private Label lblName;
        private UITextBox txtName;
        private Label lblPhone;
        private UITextBox txtPhone;
        
        private Label lblCourt;
        private UIComboBox cbCourt;
        private Label lblTime;
        private UIComboBox cbTime;
        private Label lblDuration;
        private UIComboBox cbDuration;
        
        private Label lblDateRange;
        private UIDatePicker dtStart;
        private Label lblTo;
        private UIDatePicker dtEnd;
        
        private Label lblRepeat;
        private UICheckBox chkMon;
        private UICheckBox chkTue;
        private UICheckBox chkWed;
        private UICheckBox chkThu;
        private UICheckBox chkFri;
        private UICheckBox chkSat;
        private UICheckBox chkSun;
        
        private UIButton btnCancel;
        private UIButton btnConfirm;

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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnCancelTop = new System.Windows.Forms.Label();
            
            this.rbKhachThue = new Sunny.UI.UIRadioButton();
            this.rbBaoTri = new Sunny.UI.UIRadioButton();
            
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new Sunny.UI.UITextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtPhone = new Sunny.UI.UITextBox();
            
            this.lblCourt = new System.Windows.Forms.Label();
            this.cbCourt = new Sunny.UI.UIComboBox();
            this.lblTime = new System.Windows.Forms.Label();
            this.cbTime = new Sunny.UI.UIComboBox();
            this.lblDuration = new System.Windows.Forms.Label();
            this.cbDuration = new Sunny.UI.UIComboBox();
            
            this.lblDateRange = new System.Windows.Forms.Label();
            this.dtStart = new Sunny.UI.UIDatePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtEnd = new Sunny.UI.UIDatePicker();
            
            this.lblRepeat = new System.Windows.Forms.Label();
            this.chkMon = new Sunny.UI.UICheckBox();
            this.chkTue = new Sunny.UI.UICheckBox();
            this.chkWed = new Sunny.UI.UICheckBox();
            this.chkThu = new Sunny.UI.UICheckBox();
            this.chkFri = new Sunny.UI.UICheckBox();
            this.chkSat = new Sunny.UI.UICheckBox();
            this.chkSun = new Sunny.UI.UICheckBox();
            
            this.btnCancel = new Sunny.UI.UIButton();
            this.btnConfirm = new Sunny.UI.UIButton();
            
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            
            // pnlHeader
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.pnlHeader.Controls.Add(this.btnCancelTop);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(700, 70);
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(35)))), ((int)(((byte)(50)))));
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(400, 37);
            this.lblTitle.Text = "Lập Lịch Bảo Trì & Thuê Cố Định";
            
            // btnCancelTop
            this.btnCancelTop.AutoSize = true;
            this.btnCancelTop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelTop.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCancelTop.ForeColor = System.Drawing.Color.Silver;
            this.btnCancelTop.Location = new System.Drawing.Point(650, 20);
            this.btnCancelTop.Name = "btnCancelTop";
            this.btnCancelTop.Size = new System.Drawing.Size(25, 28);
            this.btnCancelTop.Text = "X";
            
            // rbKhachThue
            this.rbKhachThue.Checked = true;
            this.rbKhachThue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbKhachThue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.rbKhachThue.Location = new System.Drawing.Point(30, 90);
            this.rbKhachThue.Name = "rbKhachThue";
            this.rbKhachThue.RadioButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.rbKhachThue.Size = new System.Drawing.Size(180, 29);
            this.rbKhachThue.Text = "Khách thuê";
            
            // rbBaoTri
            this.rbBaoTri.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbBaoTri.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.rbBaoTri.Location = new System.Drawing.Point(220, 90);
            this.rbBaoTri.Name = "rbBaoTri";
            this.rbBaoTri.RadioButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.rbBaoTri.Size = new System.Drawing.Size(150, 29);
            this.rbBaoTri.Text = "Bảo trì sân";
            
            // lblName
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblName.Location = new System.Drawing.Point(30, 140);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(125, 23);
            this.lblName.Text = "Khách đại diện:";
            
            // txtName
            this.txtName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtName.FillColor = System.Drawing.Color.White;
            this.txtName.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtName.Location = new System.Drawing.Point(30, 165);
            this.txtName.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtName.Name = "txtName";
            this.txtName.Padding = new System.Windows.Forms.Padding(5);
            this.txtName.Radius = 6;
            this.txtName.ShowText = false;
            this.txtName.Size = new System.Drawing.Size(300, 36);
            this.txtName.Watermark = "Nhập họ tên";
            
            // lblPhone
            this.lblPhone.AutoSize = true;
            this.lblPhone.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPhone.Location = new System.Drawing.Point(30, 215);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(115, 23);
            this.lblPhone.Text = "Số điện thoại:";
            
            // txtPhone
            this.txtPhone.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPhone.FillColor = System.Drawing.Color.White;
            this.txtPhone.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtPhone.Location = new System.Drawing.Point(30, 240);
            this.txtPhone.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Padding = new System.Windows.Forms.Padding(5);
            this.txtPhone.Radius = 6;
            this.txtPhone.ShowText = false;
            this.txtPhone.Size = new System.Drawing.Size(300, 36);
            this.txtPhone.Watermark = "Nhập SĐT";
            
            // lblDateRange
            this.lblDateRange.AutoSize = true;
            this.lblDateRange.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDateRange.Location = new System.Drawing.Point(30, 290);
            this.lblDateRange.Name = "lblDateRange";
            this.lblDateRange.Size = new System.Drawing.Size(145, 23);
            this.lblDateRange.Text = "Chu kỳ (Từ - Đến):";
            
            // dtStart
            this.dtStart.FillColor = System.Drawing.Color.White;
            this.dtStart.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.dtStart.Location = new System.Drawing.Point(30, 315);
            this.dtStart.MinimumSize = new System.Drawing.Size(63, 0);
            this.dtStart.Name = "dtStart";
            this.dtStart.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.dtStart.Size = new System.Drawing.Size(130, 36);
            this.dtStart.SymbolDropDown = 61555;
            this.dtStart.SymbolNormal = 61555;
            this.dtStart.Text = "2023-11-20";
            
            // lblTo
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTo.Location = new System.Drawing.Point(165, 320);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(25, 23);
            this.lblTo.Text = "->";
            
            // dtEnd
            this.dtEnd.FillColor = System.Drawing.Color.White;
            this.dtEnd.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.dtEnd.Location = new System.Drawing.Point(200, 315);
            this.dtEnd.MinimumSize = new System.Drawing.Size(63, 0);
            this.dtEnd.Name = "dtEnd";
            this.dtEnd.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.dtEnd.Size = new System.Drawing.Size(130, 36);
            this.dtEnd.SymbolDropDown = 61555;
            this.dtEnd.SymbolNormal = 61555;
            this.dtEnd.Text = "2023-12-20";
            
            // lblCourt
            this.lblCourt.AutoSize = true;
            this.lblCourt.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCourt.Location = new System.Drawing.Point(370, 95);
            this.lblCourt.Name = "lblCourt";
            this.lblCourt.Size = new System.Drawing.Size(123, 23);
            this.lblCourt.Text = "Chọn Mục tiêu:";
            
            // cbCourt
            this.cbCourt.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cbCourt.FillColor = System.Drawing.Color.White;
            this.cbCourt.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cbCourt.Items.AddRange(new object[] { "Sân số 1", "Sân số 2", "Sân số 3", "Sân VIP 1", "Sân VIP 2", "Tất Cả Các Sân" });
            this.cbCourt.Location = new System.Drawing.Point(370, 120);
            this.cbCourt.MinimumSize = new System.Drawing.Size(63, 0);
            this.cbCourt.Name = "cbCourt";
            this.cbCourt.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cbCourt.Size = new System.Drawing.Size(300, 36);
            
            // lblTime
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTime.Location = new System.Drawing.Point(370, 170);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(137, 23);
            this.lblTime.Text = "Ca bắt đầu chốt:";
            
            // cbTime
            this.cbTime.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cbTime.FillColor = System.Drawing.Color.White;
            this.cbTime.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cbTime.Items.AddRange(new object[] { "06:00", "07:00", "08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00", "22:00", "23:00" });
            this.cbTime.Location = new System.Drawing.Point(370, 195);
            this.cbTime.MinimumSize = new System.Drawing.Size(63, 0);
            this.cbTime.Name = "cbTime";
            this.cbTime.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cbTime.Size = new System.Drawing.Size(140, 36);
            
            // lblDuration
            this.lblDuration.AutoSize = true;
            this.lblDuration.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDuration.Location = new System.Drawing.Point(530, 170);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(96, 23);
            this.lblDuration.Text = "Thời lượng:";
            
            // cbDuration
            this.cbDuration.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cbDuration.FillColor = System.Drawing.Color.White;
            this.cbDuration.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cbDuration.Items.AddRange(new object[] { "60 phút", "90 phút", "120 phút", "Nguyên ngày" });
            this.cbDuration.Location = new System.Drawing.Point(530, 195);
            this.cbDuration.MinimumSize = new System.Drawing.Size(63, 0);
            this.cbDuration.Name = "cbDuration";
            this.cbDuration.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cbDuration.Size = new System.Drawing.Size(140, 36);
            
            // lblRepeat
            this.lblRepeat.AutoSize = true;
            this.lblRepeat.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblRepeat.Location = new System.Drawing.Point(370, 250);
            this.lblRepeat.Name = "lblRepeat";
            this.lblRepeat.Size = new System.Drawing.Size(161, 23);
            this.lblRepeat.Text = "Áp dụng cho ngày:";
            
            // chkMon
            this.chkMon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkMon.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkMon.Location = new System.Drawing.Point(370, 280);
            this.chkMon.Name = "chkMon";
            this.chkMon.Size = new System.Drawing.Size(80, 29);
            this.chkMon.Text = "Thứ 2";
            
            // chkTue
            this.chkTue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkTue.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkTue.Location = new System.Drawing.Point(460, 280);
            this.chkTue.Name = "chkTue";
            this.chkTue.Size = new System.Drawing.Size(80, 29);
            this.chkTue.Text = "Thứ 3";
            
            // chkWed
            this.chkWed.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkWed.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkWed.Location = new System.Drawing.Point(550, 280);
            this.chkWed.Name = "chkWed";
            this.chkWed.Size = new System.Drawing.Size(80, 29);
            this.chkWed.Text = "Thứ 4";
            
            // chkThu
            this.chkThu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkThu.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkThu.Location = new System.Drawing.Point(370, 315);
            this.chkThu.Name = "chkThu";
            this.chkThu.Size = new System.Drawing.Size(80, 29);
            this.chkThu.Text = "Thứ 5";
            
            // chkFri
            this.chkFri.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkFri.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkFri.Location = new System.Drawing.Point(460, 315);
            this.chkFri.Name = "chkFri";
            this.chkFri.Size = new System.Drawing.Size(80, 29);
            this.chkFri.Text = "Thứ 6";
            
            // chkSat
            this.chkSat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkSat.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkSat.Location = new System.Drawing.Point(550, 315);
            this.chkSat.Name = "chkSat";
            this.chkSat.Size = new System.Drawing.Size(80, 29);
            this.chkSat.Text = "Thứ 7";
            
            // chkSun
            this.chkSun.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkSun.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkSun.ForeColor = System.Drawing.Color.Red;
            this.chkSun.Location = new System.Drawing.Point(370, 350);
            this.chkSun.Name = "chkSun";
            this.chkSun.Size = new System.Drawing.Size(120, 29);
            this.chkSun.Text = "Chủ Nhật";
            
            // btnCancel
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.FillColor = System.Drawing.Color.White;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.btnCancel.ForeHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.btnCancel.Location = new System.Drawing.Point(30, 420);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Radius = 8;
            this.btnCancel.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnCancel.Size = new System.Drawing.Size(200, 45);
            this.btnCancel.Text = "Hủy bỏ";
            
            // btnConfirm
            this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfirm.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnConfirm.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(214)))), ((int)(((byte)(123)))));
            this.btnConfirm.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnConfirm.Location = new System.Drawing.Point(250, 420);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Radius = 8;
            this.btnConfirm.RectColor = System.Drawing.Color.Transparent;
            this.btnConfirm.Size = new System.Drawing.Size(420, 45);
            this.btnConfirm.Text = "Tạo Lịch Cố Định";
            
            // FrmDatSanCoDinh
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(700, 500);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.chkSun);
            this.Controls.Add(this.chkSat);
            this.Controls.Add(this.chkFri);
            this.Controls.Add(this.chkThu);
            this.Controls.Add(this.chkWed);
            this.Controls.Add(this.chkTue);
            this.Controls.Add(this.chkMon);
            this.Controls.Add(this.lblRepeat);
            this.Controls.Add(this.cbDuration);
            this.Controls.Add(this.lblDuration);
            this.Controls.Add(this.cbTime);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.cbCourt);
            this.Controls.Add(this.lblCourt);
            this.Controls.Add(this.dtEnd);
            this.Controls.Add(this.lblTo);
            this.Controls.Add(this.dtStart);
            this.Controls.Add(this.lblDateRange);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.rbBaoTri);
            this.Controls.Add(this.rbKhachThue);
            this.Controls.Add(this.pnlHeader);
            this.Name = "FrmDatSanCoDinh";
            
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
