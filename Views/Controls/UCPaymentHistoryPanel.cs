using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DemoPick
{
    public partial class UCPaymentHistoryPanel : UserControl
    {
        public sealed class HistoryRow
        {
            public string InvoiceCode { get; set; }
            public string TimeText { get; set; }
            public string CustomerText { get; set; }
            public string TotalText { get; set; }
            public string ToolTipText { get; set; }
            public bool IsHighlighted { get; set; }
            public object Tag { get; set; }
        }

        public event EventHandler SearchRequested;
        public event EventHandler OpenRequested;

        public UCPaymentHistoryPanel()
        {
            InitializeComponent();

            txtSearch.KeyDown += TxtSearch_KeyDown;
            btnSearch.Click += BtnSearch_Click;
            btnOpen.Click += BtnOpen_Click;
            lstHistory.DoubleClick += LstHistory_DoubleClick;
        }

        public string SearchKeyword
        {
            get
            {
                return txtSearch == null ? string.Empty : txtSearch.Text;
            }
            set
            {
                if (txtSearch != null)
                {
                    txtSearch.Text = value ?? string.Empty;
                }
            }
        }

        public void BindRows(IReadOnlyList<HistoryRow> rows)
        {
            if (lstHistory == null)
            {
                return;
            }

            lstHistory.BeginUpdate();
            lstHistory.Items.Clear();

            if (rows != null)
            {
                for (int i = 0; i < rows.Count; i++)
                {
                    var row = rows[i];
                    if (row == null)
                    {
                        continue;
                    }

                    var lvi = new ListViewItem(row.InvoiceCode ?? string.Empty);
                    lvi.SubItems.Add(row.TimeText ?? string.Empty);
                    lvi.SubItems.Add(row.CustomerText ?? string.Empty);
                    lvi.SubItems.Add(row.TotalText ?? string.Empty);
                    lvi.ToolTipText = row.ToolTipText ?? string.Empty;
                    lvi.Tag = row.Tag;

                    if (row.IsHighlighted)
                    {
                        lvi.BackColor = Color.FromArgb(239, 246, 255);
                    }

                    lstHistory.Items.Add(lvi);
                }
            }

            lstHistory.EndUpdate();
        }

        public bool TryGetSelectedTag<T>(out T selected) where T : class
        {
            selected = null;

            if (lstHistory == null || lstHistory.SelectedItems.Count <= 0)
            {
                return false;
            }

            selected = lstHistory.SelectedItems[0].Tag as T;
            return selected != null;
        }

        private void RaiseSearchRequested()
        {
            var handler = SearchRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void RaiseOpenRequested()
        {
            var handler = OpenRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e == null || e.KeyCode != Keys.Enter)
            {
                return;
            }

            e.Handled = true;
            e.SuppressKeyPress = true;
            RaiseSearchRequested();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            RaiseSearchRequested();
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            RaiseOpenRequested();
        }

        private void LstHistory_DoubleClick(object sender, EventArgs e)
        {
            RaiseOpenRequested();
        }
    }
}