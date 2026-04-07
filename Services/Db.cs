using System;
using System.Configuration;
using System.Data.SqlClient;

namespace DemoPick.Services
{
    internal static class Db
    {
        internal static string ConnectionString
        {
            get
            {
                var cs = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;
                if (string.IsNullOrWhiteSpace(cs))
                    throw new InvalidOperationException("Missing connection string: DefaultConnection");
                return cs;
            }
        }

        internal static SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        internal static SqlConnectionStringBuilder CreateBuilder()
        {
            return new SqlConnectionStringBuilder(ConnectionString);
        }
    }
}
