using DemoPick.Helpers;
using DemoPick.Data;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DemoPick
{
    public partial class UCInvoiceReprintPanel : UserControl
    {
        public event EventHandler ReprintByIdRequested;
        public event EventHandler ReprintLastRequested;

        public UCInvoiceReprintPanel()
        {
            InitializeComponent();

            txtInvoiceId.KeyDown += TxtInvoiceId_KeyDown;
            btnReprintById.Click += BtnReprintById_Click;
            btnReprintLast.Click += BtnReprintLast_Click;

            SetLastInvoiceEnabled(false);
        }

        public string InvoiceIdText
        {
            get { return txtInvoiceId == null ? string.Empty : txtInvoiceId.Text; }
            set
            {
                if (txtInvoiceId != null)
                {
                    txtInvoiceId.Text = value ?? string.Empty;
                }
            }
        }

        public void FocusInvoiceInput()
        {
            if (txtInvoiceId == null)
            {
                return;
            }

            txtInvoiceId.Focus();
            txtInvoiceId.SelectAll();
        }

        public void SetLastInvoiceEnabled(bool canReprint)
        {
            if (btnReprintLast == null)
            {
                return;
            }

            btnReprintLast.Enabled = canReprint;

            if (canReprint)
            {
                btnReprintLast.FillColor = Color.FromArgb(59, 130, 246);
                btnReprintLast.FillHoverColor = Color.FromArgb(37, 99, 235);
                btnReprintLast.ForeColor = Color.White;
            }
            else
            {
                btnReprintLast.FillColor = Color.FromArgb(229, 231, 235);
                btnReprintLast.FillHoverColor = Color.FromArgb(209, 213, 219);
                btnReprintLast.ForeColor = Color.FromArgb(75, 85, 99);
            }
        }

        private void BtnReprintById_Click(object sender, EventArgs e)
        {
            var handler = ReprintByIdRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void BtnReprintLast_Click(object sender, EventArgs e)
        {
            var handler = ReprintLastRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void TxtInvoiceId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e == null || e.KeyCode != Keys.Enter)
            {
                return;
            }

            e.Handled = true;
            e.SuppressKeyPress = true;
            BtnReprintById_Click(this, EventArgs.Empty);
        }
    }
}
