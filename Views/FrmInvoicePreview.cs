using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using DemoPick.Services;
using Microsoft.Reporting.WinForms;
using Sunny.UI;

namespace DemoPick
{
    public sealed class FrmInvoicePreview : Form
    {
        private readonly int _invoiceId;
        private readonly string _courtName;

        private ReportViewer _viewer;
        private UIPanel _topBar;
        private UIButton _btnExportPdf;
        private UIButton _btnExportExcel;
        private UIButton _btnClose;

        public FrmInvoicePreview(int invoiceId, string courtName)
        {
            _invoiceId = invoiceId;
            _courtName = courtName ?? "";

            InitializeUi();

            Load += (s, e) => LoadReport();
        }

        private void InitializeUi()
        {
            Text = "Hóa đơn";
            StartPosition = FormStartPosition.CenterParent;
            Width = 950;
            Height = 700;

            _topBar = new UIPanel
            {
                Dock = DockStyle.Top,
                Height = 48,
                FillColor = System.Drawing.Color.White,
                RectColor = System.Drawing.Color.FromArgb(229, 231, 235),
                Radius = 0,
                Text = null
            };

            _btnExportPdf = new UIButton
            {
                Text = "Xuất PDF",
                Width = 120,
                Height = 32,
                Left = 12,
                Top = 8,
                Radius = 8,
                FillColor = System.Drawing.Color.FromArgb(76, 175, 80),
                FillHoverColor = System.Drawing.Color.FromArgb(86, 185, 90),
                RectColor = System.Drawing.Color.Transparent,
                Cursor = Cursors.Hand,
            };
            _btnExportPdf.Click += (s, e) => Export("PDF", "pdf", "PDF (*.pdf)|*.pdf");

            _btnExportExcel = new UIButton
            {
                Text = "Xuất Excel",
                Width = 120,
                Height = 32,
                Left = 140,
                Top = 8,
                Radius = 8,
                FillColor = System.Drawing.Color.FromArgb(59, 130, 246),
                FillHoverColor = System.Drawing.Color.FromArgb(79, 150, 255),
                RectColor = System.Drawing.Color.Transparent,
                Cursor = Cursors.Hand,
            };
            _btnExportExcel.Click += (s, e) => Export("EXCELOPENXML", "xlsx", "Excel (*.xlsx)|*.xlsx");

            _btnClose = new UIButton
            {
                Text = "Đóng",
                Width = 120,
                Height = 32,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Radius = 8,
                FillColor = System.Drawing.Color.FromArgb(107, 114, 128),
                FillHoverColor = System.Drawing.Color.FromArgb(127, 134, 148),
                RectColor = System.Drawing.Color.Transparent,
                Cursor = Cursors.Hand,
            };
            _btnClose.Top = 8;
            _btnClose.Left = _topBar.Width - _btnClose.Width - 12;
            _btnClose.Click += (s, e) => Close();
            _topBar.SizeChanged += (s, e) => _btnClose.Left = _topBar.Width - _btnClose.Width - 12;

            _topBar.Controls.Add(_btnExportPdf);
            _topBar.Controls.Add(_btnExportExcel);
            _topBar.Controls.Add(_btnClose);

            _viewer = new ReportViewer
            {
                Dock = DockStyle.Fill,
                ProcessingMode = ProcessingMode.Local
            };

            Controls.Add(_viewer);
            Controls.Add(_topBar);
        }

        private void LoadReport()
        {
            try
            {
                var header = InvoiceService.GetInvoiceHeader(_invoiceId, _courtName);
                var lines = InvoiceService.GetInvoiceLines(_invoiceId);

                var headerDt = new DataTable("InvoiceHeader");
                headerDt.Columns.Add("InvoiceID", typeof(int));
                headerDt.Columns.Add("CreatedAt", typeof(DateTime));
                headerDt.Columns.Add("MemberName", typeof(string));
                headerDt.Columns.Add("PaymentMethod", typeof(string));
                headerDt.Columns.Add("TotalAmount", typeof(decimal));
                headerDt.Columns.Add("DiscountAmount", typeof(decimal));
                headerDt.Columns.Add("FinalAmount", typeof(decimal));
                headerDt.Columns.Add("BookingStartTime", typeof(DateTime));
                headerDt.Columns.Add("BookingEndTime", typeof(DateTime));

                headerDt.Rows.Add(
                    header.InvoiceID,
                    header.CreatedAt,
                    header.MemberName ?? "",
                    header.PaymentMethod ?? "",
                    header.TotalAmount,
                    header.DiscountAmount,
                    header.FinalAmount,
                    (object)header.BookingStartTime ?? DBNull.Value,
                    (object)header.BookingEndTime ?? DBNull.Value
                );

                var linesDt = new DataTable("InvoiceLines");
                linesDt.Columns.Add("ItemName", typeof(string));
                linesDt.Columns.Add("Quantity", typeof(int));
                linesDt.Columns.Add("UnitPrice", typeof(decimal));
                linesDt.Columns.Add("LineTotal", typeof(decimal));

                foreach (var l in lines)
                {
                    linesDt.Rows.Add(l.ItemName ?? "", l.Quantity, l.UnitPrice, l.LineTotal);
                }

                _viewer.Reset();
                _viewer.LocalReport.EnableExternalImages = true;
                _viewer.LocalReport.ReportEmbeddedResource = "DemoPick.Reports.Bill.rdlc";
                _viewer.LocalReport.DataSources.Clear();
                _viewer.LocalReport.DataSources.Add(new ReportDataSource("InvoiceHeader", headerDt));
                _viewer.LocalReport.DataSources.Add(new ReportDataSource("InvoiceLines", linesDt));

                string logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "pick.png");
                string logoUri = File.Exists(logoPath) ? new Uri(logoPath).AbsoluteUri : "";

                string qrPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "qr.png");
                string qrUri = File.Exists(qrPath) ? new Uri(qrPath).AbsoluteUri : "";

                _viewer.LocalReport.SetParameters(new[]
                {
                    new ReportParameter("LogoPath", logoUri, true),
                    new ReportParameter("QrPath", qrUri, true),
                    new ReportParameter("CourtName", _courtName ?? "", true)
                });

                _viewer.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải hóa đơn để in/xem trước.\n\n" + ex.Message,
                    "Lỗi hóa đơn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Close();
            }
        }

        private void Export(string renderFormat, string fileExt, string filter)
        {
            try
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = filter;
                    sfd.FileName = $"Invoice_{_invoiceId}.{fileExt}";

                    if (sfd.ShowDialog(this) != DialogResult.OK)
                        return;

                    Warning[] warnings;
                    string[] streamIds;
                    string mimeType;
                    string encoding;
                    string extension;

                    byte[] bytes = _viewer.LocalReport.Render(
                        renderFormat,
                        null,
                        out mimeType,
                        out encoding,
                        out extension,
                        out streamIds,
                        out warnings
                    );

                    File.WriteAllBytes(sfd.FileName, bytes);

                    new UIPage().ShowSuccessTip($"Đã xuất file: {sfd.FileName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Xuất file thất bại: " + ex.Message,
                    "Lỗi xuất hóa đơn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
