using DemoPick.Helpers;
using DemoPick.Data;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DemoPick
{
    public partial class UCCategoryChip : UserControl
    {
        private bool _isActive;

        public UCCategoryChip()
        {
            InitializeComponent();
            UpdateVisual();

            btnChip.Click += (s, e) => OnClick(e);
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get => btnChip.Text;
            set
            {
                base.Text = value;
                btnChip.Text = value;
                UpdateSizeToContent();
            }
        }

        [Browsable(false)]
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                UpdateVisual();
            }
        }

        public void SetActive(bool isActive) => IsActive = isActive;

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }

        private void UpdateVisual()
        {
            UpdateSizeToContent();

            // Keep colors aligned with existing UI.
            if (_isActive)
            {
                btnChip.FillColor = Color.FromArgb(76, 175, 80);
                btnChip.FillHoverColor = Color.FromArgb(86, 185, 90);
                btnChip.ForeColor = Color.White;
                btnChip.ForeHoverColor = Color.White;
                btnChip.RectColor = Color.FromArgb(76, 175, 80);
            }
            else
            {
                btnChip.FillColor = Color.White;
                btnChip.FillHoverColor = Color.FromArgb(240, 240, 240);
                btnChip.ForeColor = Color.FromArgb(107, 114, 128);
                btnChip.ForeHoverColor = Color.FromArgb(107, 114, 128);
                btnChip.RectColor = Color.FromArgb(229, 231, 235);
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            // When base.Text changes (design-time or runtime), keep the inner button in sync.
            btnChip.Text = base.Text;
            UpdateSizeToContent();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            UpdateSizeToContent();
        }

        private void UpdateSizeToContent()
        {
            // Keep a minimum width similar to the original hardcoded 100.
            var size = TextRenderer.MeasureText(btnChip.Text ?? string.Empty, btnChip.Font);
            int width = Math.Max(100, size.Width + 28);
            btnChip.Width = width;
            this.Width = width;
        }
    }
}

