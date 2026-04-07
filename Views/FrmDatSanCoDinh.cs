using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DemoPick.Services;

namespace DemoPick
{
    public partial class FrmDatSanCoDinh : Form
    {
        private readonly DemoPick.Controllers.BookingController _controller = new DemoPick.Controllers.BookingController();

        public FrmDatSanCoDinh()
        {
            InitializeComponent();

            if (DesignModeUtil.IsDesignMode(this))
            {
                return;
            }
            
            // Logic for closing
            btnCancel.Click += (s, e) => this.Close();
            btnCancelTop.Click += (s, e) => this.Close();
            
            // Logic for switching modes
            rbKhachThue.CheckedChanged += RbMode_CheckedChanged;
            rbBaoTri.CheckedChanged += RbMode_CheckedChanged;

            // Save logic
            btnConfirm.Click += BtnConfirm_Click;

            this.Load += FrmDatSanCoDinh_Load;
            cbTime.SelectedIndex = 11; // 17:00
            cbDuration.SelectedIndex = 1; // 90 phút
            
            // Default dates
            dtStart.Value = DateTime.Now;
            dtEnd.Value = DateTime.Now.AddMonths(1);

            // Drag form logic
            this.MouseDown += Form_MouseDown;
            pnlHeader.MouseDown += Form_MouseDown;
            lblTitle.MouseDown += Form_MouseDown;

            // Form styles
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Region = new Region(RoundedRect(new Rectangle(0, 0, this.Width, this.Height), 20));
            this.Paint += Frm_Paint;
        }

        private void FrmDatSanCoDinh_Load(object sender, EventArgs e)
        {
            try
            {
                var courts = _controller.GetCourts();
                cbCourt.DataSource = courts;
                cbCourt.DisplayMember = "Name";
                cbCourt.ValueMember = "CourtID";

                if (courts.Count > 0)
                    cbCourt.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Data Sân: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RbMode_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBaoTri.Checked)
            {
                txtName.Enabled = false;
                txtPhone.Enabled = false;
                txtName.Text = "Ban Quản Lý (Bảo Trì)";
                txtPhone.Text = "N/A";
                
                btnConfirm.FillColor = Color.FromArgb(231, 76, 60); // Red
                btnConfirm.FillHoverColor = Color.FromArgb(241, 86, 70); 
                btnConfirm.Text = "Xác nhận Khóa Sân";
            }
            else
            {
                txtName.Enabled = true;
                txtPhone.Enabled = true;
                txtName.Text = "";
                txtPhone.Text = "";
                
                btnConfirm.FillColor = Color.FromArgb(46, 204, 113); // Green
                btnConfirm.FillHoverColor = Color.FromArgb(56, 214, 123);
                btnConfirm.Text = "Tạo Lịch Cố Định";
            }
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (rbKhachThue.Checked && (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPhone.Text)))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin khách thuê!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if at least one day is checked
            if (!chkMon.Checked && !chkTue.Checked && !chkWed.Checked && !chkThu.Checked && 
                !chkFri.Checked && !chkSat.Checked && !chkSun.Checked)
            {
                MessageBox.Show("Vui lòng chọn ít nhất 1 ngày trong tuần để lặp lại!", "Lý lịch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dtEnd.Value <= dtStart.Value)
            {
                MessageBox.Show("Ngày kết thúc phải lớn hơn ngày bắt đầu!", "Lỗi ngày", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int courtId = 0;
            try
            {
                if (cbCourt.SelectedValue != null)
                    courtId = (int)cbCourt.SelectedValue;
            }
            catch { courtId = 0; }
            if (courtId <= 0)
            {
                MessageBox.Show("Không xác định được sân. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string timeStr = cbTime.SelectedItem?.ToString() ?? "17:00";
            if (!TryParseHourMinute(timeStr, out int hh, out int mm))
            {
                MessageBox.Show("Giờ bắt đầu không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int durationMins = ParseDurationMinutes(cbDuration.SelectedItem?.ToString());
            if (durationMins <= 0) durationMins = 90;

            bool dMon = chkMon.Checked;
            bool dTue = chkTue.Checked;
            bool dWed = chkWed.Checked;
            bool dThu = chkThu.Checked;
            bool dFri = chkFri.Checked;
            bool dSat = chkSat.Checked;
            bool dSun = chkSun.Checked;

            string status = rbBaoTri.Checked ? "Maintenance" : "Confirmed";
            string guestName = rbBaoTri.Checked ? (txtName.Text ?? "Ban Quản Lý (Bảo Trì)") : (txtName.Text.Trim() + " - " + txtPhone.Text.Trim());

            int created = 0;
            int conflicts = 0;
            int skippedPast = 0;
            int errors = 0;

            DateTime from = dtStart.Value.Date;
            DateTime to = dtEnd.Value.Date;
            DateTime now = DateTime.Now;

            for (DateTime d = from; d <= to; d = d.AddDays(1))
            {
                if (!IsSelectedDay(d.DayOfWeek, dMon, dTue, dWed, dThu, dFri, dSat, dSun))
                    continue;

                DateTime start = d.AddHours(hh).AddMinutes(mm);
                DateTime end = start.AddMinutes(durationMins);
                if (end <= start) continue;

                // Skip occurrences in the past to reduce accidental spam.
                if (end <= now)
                {
                    skippedPast++;
                    continue;
                }

                try
                {
                    _controller.SubmitBooking(courtId, guestName, start, end, status);
                    created++;
                }
                catch (Exception ex)
                {
                    // Stored proc uses RAISERROR for conflicts.
                    if ((ex.Message ?? "").IndexOf("already booked", StringComparison.OrdinalIgnoreCase) >= 0)
                        conflicts++;
                    else
                        errors++;
                }
            }

            if (created <= 0)
            {
                MessageBox.Show("Không tạo được lịch nào (có thể do trùng lịch hoặc toàn bộ mốc thời gian đã qua).", "Không có thay đổi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.Cancel;
                return;
            }

            string modeStr = rbBaoTri.Checked ? "BẢO TRÌ SÂN" : "THUÊ CỐ ĐỊNH";
            MessageBox.Show(
                $"Đã tạo lịch {modeStr}!\n\n" +
                $"- Tạo mới: {created}\n" +
                (conflicts > 0 ? $"- Trùng lịch: {conflicts}\n" : "") +
                (skippedPast > 0 ? $"- Bỏ qua (quá khứ): {skippedPast}\n" : "") +
                (errors > 0 ? $"- Lỗi khác: {errors}\n" : ""),
                "Thành công",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private static bool IsSelectedDay(DayOfWeek day, bool mon, bool tue, bool wed, bool thu, bool fri, bool sat, bool sun)
        {
            switch (day)
            {
                case DayOfWeek.Monday: return mon;
                case DayOfWeek.Tuesday: return tue;
                case DayOfWeek.Wednesday: return wed;
                case DayOfWeek.Thursday: return thu;
                case DayOfWeek.Friday: return fri;
                case DayOfWeek.Saturday: return sat;
                case DayOfWeek.Sunday: return sun;
                default: return false;
            }
        }

        private static int ParseDurationMinutes(string durationText)
        {
            string t = durationText ?? "";
            if (t.IndexOf("60", StringComparison.OrdinalIgnoreCase) >= 0) return 60;
            if (t.IndexOf("90", StringComparison.OrdinalIgnoreCase) >= 0) return 90;
            if (t.IndexOf("120", StringComparison.OrdinalIgnoreCase) >= 0) return 120;
            if (t.IndexOf("180", StringComparison.OrdinalIgnoreCase) >= 0) return 180;
            return 0;
        }

        private static bool TryParseHourMinute(string timeText, out int hours, out int minutes)
        {
            hours = 0;
            minutes = 0;

            string t = (timeText ?? "").Trim();
            string[] parts = t.Split(':');
            if (parts.Length < 2) return false;
            if (!int.TryParse(parts[0], out hours)) return false;
            if (!int.TryParse(parts[1], out minutes)) return false;
            if (hours < 0 || hours > 23) return false;
            if (minutes < 0 || minutes > 59) return false;
            return true;
        }

        private void Frm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen p = new Pen(Color.FromArgb(220, 220, 220), 1))
            {
                e.Graphics.DrawPath(p, RoundedRect(new Rectangle(0, 0, this.Width - 1, this.Height - 1), 20));
            }
        }

        private GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, NativeMethods.WM_NCLBUTTONDOWN, NativeMethods.HT_CAPTION, 0);
            }
        }
    }
}
