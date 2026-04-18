using System;
using System.Data.SqlClient;

namespace DemoPick.Services
{
    internal static class SmokeMaintenanceHelper
    {
        internal static bool HasMemberByPhone(string phone)
        {
            object obj = DatabaseHelper.ExecuteScalar(
                "SELECT TOP 1 1 FROM dbo.Members WHERE Phone = @Phone",
                new SqlParameter("@Phone", phone)
            );
            return obj != null && obj != DBNull.Value;
        }

        internal static string BuildUniquePhone()
        {
            string digits = Math.Abs(Guid.NewGuid().GetHashCode()).ToString("D10");
            return "09" + digits.Substring(0, 8);
        }

        internal static string CleanupLegacySmokeArtifacts()
        {
            int delBooking = 0;
            int delMembers = 0;
            int delAccounts = 0;

            try
            {
                delBooking = DatabaseHelper.ExecuteNonQuery(
                    "DELETE FROM dbo.Bookings WHERE ISNULL(LTRIM(RTRIM(GuestName)), '') LIKE 'SMOKE%'");
            }
            catch
            {
            }

            try
            {
                delMembers = DatabaseHelper.ExecuteNonQuery(
                    "DELETE FROM dbo.Members WHERE ISNULL(LTRIM(RTRIM(FullName)), '') LIKE 'SMOKE%'");
            }
            catch
            {
            }

            try
            {
                delAccounts = DatabaseHelper.ExecuteNonQuery(
                    "DELETE FROM dbo.StaffAccounts WHERE ISNULL(LTRIM(RTRIM(Username)), '') LIKE 'smoke_%' OR ISNULL(LTRIM(RTRIM(Email)), '') LIKE 'smoke_%'");
            }
            catch
            {
            }

            return "Bookings=" + delBooking + ", Members=" + delMembers + ", StaffAccounts=" + delAccounts;
        }
    }
}
