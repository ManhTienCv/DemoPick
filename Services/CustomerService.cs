using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DemoPick.Models;

namespace DemoPick.Services
{
    public class CustomerService
    {
        public async Task<List<CustomerModel>> GetAllCustomersAsync()
        {
            var list = new List<CustomerModel>();
            string query = "SELECT MemberID, FullName, Phone, TotalHoursPurchased, IsFixed, TotalSpent, CreatedAt FROM Members ORDER BY CreatedAt DESC";

            await Task.Run(() => {
                var dt = DatabaseHelper.ExecuteQuery(query);
                foreach (DataRow row in dt.Rows)
                {
                    bool isFixed = row.Table.Columns.Contains("IsFixed") && row["IsFixed"] != DBNull.Value && Convert.ToBoolean(row["IsFixed"]);
                    decimal hours = row.Table.Columns.Contains("TotalHoursPurchased") && row["TotalHoursPurchased"] != DBNull.Value ? Convert.ToDecimal(row["TotalHoursPurchased"]) : 0m;
                    string type = isFixed ? "Cố định" : "Vãng lai";
                    DateTime created = row.Table.Columns.Contains("CreatedAt") && row["CreatedAt"] != DBNull.Value ? Convert.ToDateTime(row["CreatedAt"]) : DateTime.MinValue;

                    list.Add(new CustomerModel
                    {
                        Id = "#PB" + row["MemberID"].ToString(),
                        Name = row["FullName"].ToString(),
                        Phone = row["Phone"].ToString(),
                        CustomerType = type,
                        TotalHours = hours,
                        TotalSpent = Convert.ToDecimal(row["TotalSpent"]).ToString("N0") + "đ",
                        CreatedAt = created
                    });
                }
            });
            
            return list;
        }
    }
}
