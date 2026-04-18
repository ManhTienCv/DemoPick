using System.Data.SqlClient;

namespace DemoPick.Services
{
    internal static class AuthLoginAttemptTracker
    {
        internal static void RecordFailedLoginAttempt(int accountId, int maxFailedLoginAttempts, int lockoutMinutes)
        {
            // Best-effort lockout. If the DB schema doesn't have these columns yet, ignore errors.
            DatabaseHelper.ExecuteNonQuery(
                SqlQueries.Auth.RecordFailedLoginAttempt,
                new SqlParameter("@Id", accountId),
                new SqlParameter("@Max", maxFailedLoginAttempts),
                new SqlParameter("@Minutes", lockoutMinutes));
        }

        internal static void ResetFailedLogin(int accountId)
        {
            DatabaseHelper.ExecuteNonQuery(
                SqlQueries.Auth.ResetFailedLogin,
                new SqlParameter("@Id", accountId));
        }
    }
}
