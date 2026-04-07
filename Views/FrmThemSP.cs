using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using DemoPick.Services;

namespace DemoPick
{
    public partial class FrmThemSP : Sunny.UI.UIForm
    {
        public FrmThemSP()
        {
            InitializeComponent();
            txtSKU.Text = "PD-" + DateTime.Now.ToString("yyMMddHHmm");
            SetupForm();
        }

        private void SetupForm()
        {
            btnDong.Click += (s, e) => this.Close();
            btnLuu.Click += BtnLuu_Click;
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTen.Text) || string.IsNullOrWhiteSpace(txtGia.Text))
            {
                MessageBox.Show("Sếp vui lòng nhập đủ Tên Món và Đơn Giá nhé!", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtGia.Text.Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal price) || price <= 0)
            {
                MessageBox.Show("Đơn giá không hợp lệ (phải là số > 0).", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtSoLuong.Text.Trim(), out int qty) || qty <= 0)
            {
                MessageBox.Show("Số lượng không hợp lệ (phải là số nguyên > 0).", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string sql = @"
                    INSERT INTO Products (SKU, Name, Category, Price, StockQuantity, MinThreshold)
                    VALUES (@SKU, @Name, @Category, @Price, @StockQuantity, @MinThreshold)";

                var p = new System.Data.SqlClient.SqlParameter[]
                {
                    new System.Data.SqlClient.SqlParameter("@SKU", txtSKU.Text.Trim()),
                    new System.Data.SqlClient.SqlParameter("@Name", txtTen.Text.Trim()),
                    new System.Data.SqlClient.SqlParameter("@Category", cboLoai.SelectedItem?.ToString() ?? ""),
                    new System.Data.SqlClient.SqlParameter("@Price", price),
                    new System.Data.SqlClient.SqlParameter("@StockQuantity", qty),
                    new System.Data.SqlClient.SqlParameter("@MinThreshold", 5)
                };

                DatabaseHelper.ExecuteNonQuery(sql, p);

                // Add Transaction Log
                string logSql = "INSERT INTO SystemLogs (EventDesc, SubDesc) VALUES (@EventDesc, @SubDesc)";
                DatabaseHelper.ExecuteNonQuery(
                    logSql,
                    new System.Data.SqlClient.SqlParameter("@EventDesc", "Nhập Kho Trực Tiếp"),
                    new System.Data.SqlClient.SqlParameter("@SubDesc", $"+{txtSoLuong.Text} {txtTen.Text}")
                );

                MessageBox.Show("Bơm hàng thành công rực rỡ! Đội POS vỗ tay!", "Cập Nhật SQL", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mẻ nạp hàng lỗi CSDL: " + ex.Message, "Phản Lệnh", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
