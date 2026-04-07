using System;
using System.Configuration;

namespace DemoPick.Services
{
    /// <summary>
    /// Feature flags / policy toggles for authentication.
    /// 
    /// NOTE: Currently these are intentionally NOT wired into UI flows.
    /// They exist so we can enable/disable behaviors later with minimal code churn.
    /// </summary>
    internal static class AuthFeatures
    {
        /// <summary>
        /// Optional App.config key to disable self-registration.
        /// When missing/blank, defaults to enabled.
        /// 
        /// Future usage idea:
        /// - If false: hide Register entrypoints and block TryRegister from UI.
        /// </summary>
        internal const string EnableSelfRegistrationKey = "EnableSelfRegistration";

        /// <summary>
        /// Returns true when self-registration is allowed.
        /// Default: true.
        /// </summary>
        internal static bool IsSelfRegistrationEnabled()
        {
            string v = null;
            try
            {
                v = ConfigurationManager.AppSettings[EnableSelfRegistrationKey];
            }
            catch
            {
                // Ignore config errors; default to enabled.
            }

            if (string.IsNullOrWhiteSpace(v)) return true;
            return string.Equals(v.Trim(), "true", StringComparison.OrdinalIgnoreCase);
        }
    }
}
