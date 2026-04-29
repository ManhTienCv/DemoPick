using System;
using System.Windows.Forms;
using DemoPick.Services;
using DemoPick.Data;
using DemoPick.Helpers;
using Sunny.UI;

namespace DemoPick
{
    public partial class UCThanhToan
    {
        private int _lastCompletedInvoiceId = 0;
        private string _lastCompletedCourtName = string.Empty;

        private void InitializeReprintButton()
        {
            if (ucInvoiceReprintPanel == null)
            {
                return;
            }

            ucInvoiceReprintPanel.ReprintByIdRequested -= UcInvoiceReprintPanel_ReprintByIdRequested;
            ucInvoiceReprintPanel.ReprintLastRequested -= UcInvoiceReprintPanel_ReprintLastRequested;

            ucInvoiceReprintPanel.ReprintByIdRequested += UcInvoiceReprintPanel_ReprintByIdRequested;
            ucInvoiceReprintPanel.ReprintLastRequested += UcInvoiceReprintPanel_ReprintLastRequested;
        }

        private void UcInvoiceReprintPanel_ReprintByIdRequested(object sender, EventArgs e)
        {
            BtnReprintById_Click(sender, EventArgs.Empty);
        }

        private void UcInvoiceReprintPanel_ReprintLastRequested(object sender, EventArgs e)
        {
            BtnReprintLastInvoice_Click(sender, EventArgs.Empty);
        }

        private void UpdateReprintButtonState()
        {
            if (ucInvoiceReprintPanel == null)
            {
                return;
            }

            bool canReprint = _lastCompletedInvoiceId > 0;
            ucInvoiceReprintPanel.SetLastInvoiceEnabled(canReprint);
        }

        private void BtnReprintLastInvoice_Click(object sender, EventArgs e)
        {
            if (_lastCompletedInvoiceId <= 0)
            {
                new UIPage().ShowWarningTip("Chưa có hóa đơn nào để in lại.");
                return;
            }

            ShowInvoicePreview(_lastCompletedInvoiceId, _lastCompletedCourtName);
        }

        private void BtnReprintById_Click(object sender, EventArgs e)
        {
            if (ucInvoiceReprintPanel == null)
            {
                return;
            }

            string raw = (ucInvoiceReprintPanel.InvoiceIdText ?? string.Empty).Trim();
            if (!int.TryParse(raw, out int invoiceId) || invoiceId <= 0)
            {
                new UIPage().ShowWarningTip("Vui lòng nhập mã hóa đơn hợp lệ (số dương).");
                ucInvoiceReprintPanel.FocusInvoiceInput();
                return;
            }

            string courtName = invoiceId == _lastCompletedInvoiceId
                ? (_lastCompletedCourtName ?? string.Empty)
                : string.Empty;

            ShowInvoicePreview(invoiceId, courtName);

            _lastCompletedInvoiceId = invoiceId;
            _lastCompletedCourtName = courtName;
            UpdateReprintButtonState();
            ReloadPaymentHistory();
        }

        private void ShowInvoicePreview(int invoiceId, string courtName)
        {
            if (invoiceId <= 0)
            {
                return;
            }

            try
            {
                using (var frm = new FrmInvoicePreview(invoiceId, courtName ?? string.Empty))
                {
                    frm.ShowDialog(FindForm());
                }
            }
            catch (Exception ex)
            {
                DatabaseHelper.TryLog("Print error", ex, "UCThanhToan.ShowInvoicePreview");
                new UIPage().ShowErrorTip("Không thể mở hóa đơn để in lại.");
            }
        }
    }
}


