using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DemoPick.Services
{
    public static class InvoiceService
    {
        public sealed class InvoiceHeader
        {
            public int InvoiceID { get; set; }
            public DateTime CreatedAt { get; set; }
            public int? MemberID { get; set; }
            public string MemberName { get; set; }
            public string PaymentMethod { get; set; }
            public decimal TotalAmount { get; set; }
            public decimal DiscountAmount { get; set; }
            public decimal FinalAmount { get; set; }
            public DateTime? BookingStartTime { get; set; }
            public DateTime? BookingEndTime { get; set; }
        }

        public sealed class InvoiceLine
        {
            public string ItemName { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal LineTotal { get; set; }
        }

        public static InvoiceHeader GetInvoiceHeader(int invoiceId)
        {
            return GetInvoiceHeader(invoiceId, null);
        }

        public static InvoiceHeader GetInvoiceHeader(int invoiceId, string courtName)
        {
            var dt = DatabaseHelper.ExecuteQuery(
                @";WITH Inv AS (
    SELECT TOP (1)
        i.InvoiceID,
        i.CreatedAt,
        i.MemberID,
        m.FullName AS MemberName,
        i.PaymentMethod,
        i.TotalAmount,
        i.DiscountAmount,
        i.FinalAmount
    FROM dbo.Invoices i
    LEFT JOIN dbo.Members m ON m.MemberID = i.MemberID
    WHERE i.InvoiceID = @InvoiceID
), Bk AS (
    SELECT TOP (1)
        b.StartTime AS BookingStartTime,
        b.EndTime AS BookingEndTime
    FROM dbo.Bookings b
    INNER JOIN dbo.Courts c ON c.CourtID = b.CourtID
    CROSS JOIN Inv
    WHERE @CourtName IS NOT NULL
      AND LTRIM(RTRIM(@CourtName)) <> ''
      AND c.Name = @CourtName
            AND b.Status = 'Paid'
      AND b.EndTime >= DATEADD(MINUTE, -10, Inv.CreatedAt)
      AND b.EndTime <= DATEADD(MINUTE,  10, Inv.CreatedAt)
    ORDER BY ABS(DATEDIFF(SECOND, b.EndTime, Inv.CreatedAt))
)
SELECT
    Inv.*,
    Bk.BookingStartTime,
    Bk.BookingEndTime
FROM Inv
LEFT JOIN Bk ON 1 = 1;",
                new SqlParameter("@InvoiceID", invoiceId),
                new SqlParameter("@CourtName", (object)(courtName ?? "") ?? DBNull.Value)
            );

            if (dt.Rows.Count == 0)
                throw new InvalidOperationException($"Không tìm thấy hóa đơn #{invoiceId}.");

            DataRow r = dt.Rows[0];

            return new InvoiceHeader
            {
                InvoiceID = Convert.ToInt32(r["InvoiceID"]),
                CreatedAt = Convert.ToDateTime(r["CreatedAt"]),
                MemberID = r["MemberID"] == DBNull.Value ? (int?)null : Convert.ToInt32(r["MemberID"]),
                MemberName = r["MemberName"] == DBNull.Value ? "" : r["MemberName"].ToString(),
                PaymentMethod = r["PaymentMethod"] == DBNull.Value ? "" : r["PaymentMethod"].ToString(),
                TotalAmount = Convert.ToDecimal(r["TotalAmount"]),
                DiscountAmount = Convert.ToDecimal(r["DiscountAmount"]),
                FinalAmount = Convert.ToDecimal(r["FinalAmount"]),
                BookingStartTime = r.Table.Columns.Contains("BookingStartTime") && r["BookingStartTime"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(r["BookingStartTime"]) : null,
                BookingEndTime = r.Table.Columns.Contains("BookingEndTime") && r["BookingEndTime"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(r["BookingEndTime"]) : null
            };
        }

        public static List<InvoiceLine> GetInvoiceLines(int invoiceId)
        {
            var list = new List<InvoiceLine>();
            var dt = DatabaseHelper.ExecuteQuery(
                @"SELECT
                      CASE
                          WHEN d.ProductID IS NULL AND d.BookingID IS NOT NULL THEN N'Tiền sân'
                                                    WHEN d.ProductID IS NULL THEN N'(Dịch vụ)'
                          ELSE ISNULL(p.Name, N'(Dịch vụ)')
                      END AS ItemName,
                      d.Quantity,
                      d.UnitPrice,
                      CAST(d.Quantity * d.UnitPrice AS DECIMAL(18,2)) AS LineTotal
                  FROM dbo.InvoiceDetails d
                  LEFT JOIN dbo.Products p ON p.ProductID = d.ProductID
                  WHERE d.InvoiceID = @InvoiceID
                  ORDER BY d.DetailID ASC",
                new SqlParameter("@InvoiceID", invoiceId)
            );

            foreach (DataRow r in dt.Rows)
            {
                list.Add(new InvoiceLine
                {
                    ItemName = r["ItemName"] == DBNull.Value ? "" : r["ItemName"].ToString(),
                    Quantity = Convert.ToInt32(r["Quantity"]),
                    UnitPrice = Convert.ToDecimal(r["UnitPrice"]),
                    LineTotal = Convert.ToDecimal(r["LineTotal"])
                });
            }

            return list;
        }
    }
}
