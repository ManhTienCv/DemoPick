using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DemoPick.Services
{
    internal static class DesignModeUtil
    {
        internal static bool IsDesignMode(Control control)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return true;
            }

            try
            {
                return control?.Site?.DesignMode == true;
            }
            catch
            {
                return false;
            }
        }
    }
}
