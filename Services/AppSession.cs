namespace DemoPick.Services
{
    internal static class AppSession
    {
        internal static AuthService.AuthUser CurrentUser { get; private set; }

        internal static void SignIn(AuthService.AuthUser user)
        {
            CurrentUser = user;
        }

        internal static void SignOut()
        {
            CurrentUser = null;
        }

        internal static bool IsInRole(string role)
        {
            if (CurrentUser == null) return false;
            if (string.IsNullOrWhiteSpace(role)) return false;

            return string.Equals(CurrentUser.Role ?? "", role, System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
