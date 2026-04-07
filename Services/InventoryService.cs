using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DemoPick.Models;

namespace DemoPick.Services
{
    public class InventoryService
    {
        public async Task<List<InventoryItemModel>> GetInventoryItemsAsync()
        {
            var list = new List<InventoryItemModel>();
            string query = "SELECT SKU, Name, Category, StockQuantity, MinThreshold, Price FROM Products WHERE Category != N'Dịch vụ đi kèm'";

            await Task.Run(() => {
                var dt = DatabaseHelper.ExecuteQuery(query);
                foreach (DataRow row in dt.Rows)
                {
                    int stock = Convert.ToInt32(row["StockQuantity"]);
                    int min = Convert.ToInt32(row["MinThreshold"]);
                    string status = "Healthy";
                    if (stock <= 0) status = "Out of Stock";
                    else if (stock <= min) status = "Critical Low";
                    else if (stock <= min * 2) status = "Warning";

                    list.Add(new InventoryItemModel
                    {
                        Sku = row["SKU"].ToString(),
                        Name = row["Name"].ToString(),
                        Category = row["Category"].ToString(),
                        Stock = $"{stock} / {min * 10}", // Giổ hàng max giả định
                        Status = status,
                        Price = Convert.ToDecimal(row["Price"]).ToString("N0") + "đ"
                    });
                }
            });
            return list;
        }

        public async Task<List<TransactionModel>> GetRecentTransactionsAsync()
        {
            var list = new List<TransactionModel>();
            // Inventory screen should show actual inventory/sales transactions,
            // not generic system error logs from other modules.
            string query = @"
SELECT TOP 10 EventDesc, SubDesc, CreatedAt
FROM dbo.SystemLogs
WHERE EventDesc IN (N'Nhập Kho Trực Tiếp', N'POS Checkout')
ORDER BY CreatedAt DESC";

            await Task.Run(() => {
                var dt = DatabaseHelper.ExecuteQuery(query);
                foreach (DataRow row in dt.Rows)
                {
                    DateTime time = Convert.ToDateTime(row["CreatedAt"]);
                    string timeStr;
                    var span = DateTime.Now - time;
                    if (span.TotalMinutes < 60) timeStr = $"{(int)span.TotalMinutes} phút trước";
                    else if (span.TotalHours < 24) timeStr = $"{(int)span.TotalHours} giờ trước";
                    else timeStr = "Hôm qua";

                    list.Add(new TransactionModel
                    {
                        EventDesc = row["EventDesc"].ToString(),
                        SubDesc = row["SubDesc"] != DBNull.Value ? (row["SubDesc"].ToString() ?? "") : "",
                        Time = timeStr
                    });
                }
            });
            return list;
        }
    }
}
