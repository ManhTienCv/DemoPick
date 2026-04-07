using System;
using System.Data;
using System.Data.SqlClient;

namespace DemoPick.Services
{
    public static class DatabaseHelper
    {
        public static SqlConnection GetConnection()
        {
            return Db.CreateConnection();
        }

        // Option 1: Execute and get DataTable (SELECT)
        public static DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                var dt = new DataTable();
                var da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
        }

        // Option 2: Execute non-query (INSERT, UPDATE, DELETE)
        public static int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        // Overload: Execute non-query within an existing transaction
        public static int ExecuteNonQuery(SqlConnection conn, SqlTransaction tran, string query, params SqlParameter[] parameters)
        {
            using (var cmd = new SqlCommand(query, conn, tran))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }

        // Option 3: Execute scalar (Get single value like SCOPE_IDENTITY)
        public static object ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                return cmd.ExecuteScalar();
            }
        }

        // Overload: Execute scalar within an existing transaction
        public static object ExecuteScalar(SqlConnection conn, SqlTransaction tran, string query, params SqlParameter[] parameters)
        {
            using (var cmd = new SqlCommand(query, conn, tran))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteScalar();
            }
        }

        // Best-effort logging to SystemLogs. Never throws.
        public static void TryLog(string eventDesc, string subDesc)
        {
            try
            {
                ExecuteNonQuery(
                    "INSERT INTO SystemLogs (EventDesc, SubDesc) VALUES (@EventDesc, @SubDesc)",
                    new SqlParameter("@EventDesc", eventDesc ?? ""),
                    new SqlParameter("@SubDesc", (object)subDesc ?? DBNull.Value)
                );
            }
            catch
            {
                // Intentionally swallow: logging must never break the app.
            }
        }

        public static void TryLog(string eventDesc, Exception ex, string context = null)
        {
            string sub = ex == null ? (context ?? "") : $"{context}\n{ex.GetType().Name}: {ex.Message}";
            TryLog(eventDesc, sub);
        }
    }
}
