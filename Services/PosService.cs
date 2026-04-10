using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DemoPick.Services
{
    public class PosService
    {
        public sealed class CartLine
        {
            public int ProductId { get; }
            public string ProductName { get; }
            public int Quantity { get; }
            public decimal UnitPrice { get; }

            public CartLine(int productId, string productName, int quantity, decimal unitPrice)
            {
                ProductId = productId;
                ProductName = productName ?? "";
                Quantity = quantity;
                UnitPrice = unitPrice;
            }
        }

        // --- PENDING ORDER STATE MANAGEMENT ---
        public static Dictionary<string, List<CartLine>> PendingOrders { get; } = new Dictionary<string, List<CartLine>>(StringComparer.OrdinalIgnoreCase);

        public static void SavePendingOrder(string courtName, List<CartLine> lines)
        {
            if (string.IsNullOrWhiteSpace(courtName)) return;
            PendingOrders[courtName] = lines ?? new List<CartLine>();
        }

        public static List<CartLine> GetPendingOrder(string courtName)
        {
            if (string.IsNullOrWhiteSpace(courtName)) return new List<CartLine>();
            if (PendingOrders.TryGetValue(courtName, out var lines)) return new List<CartLine>(lines);
            return new List<CartLine>();
        }

        public static void ClearPendingOrder(string courtName)
        {
            if (string.IsNullOrWhiteSpace(courtName)) return;
            PendingOrders.Remove(courtName);
        }
        // --------------------------------------

        public int Checkout(
            int memberId,
            IReadOnlyList<CartLine> lines,
            decimal totalAmount,
            decimal discountAmount,
            decimal finalAmount,
            string paymentMethod,
            string courtNameForLog)
        {
            // Allow court-only checkouts (no POS products) so we can close bookings and issue invoices.
            if (lines == null)
                lines = Array.Empty<CartLine>();

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (var tran = conn.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        bool hasReduceStockTrigger = TriggerExists(conn, tran, "trg_ReduceStock");
                        bool hasUpdateMemberTrigger = TriggerExists(conn, tran, "trg_UpdateMemberTier");

                        int closedBookingId = TryCloseActiveBookingByCourtName(conn, tran, courtNameForLog);
                        int creditedBookingId = closedBookingId;

                        if (creditedBookingId == 0)
                        {
                            // Fallback: if booking already ended (EndTime <= now), still credit the member's hours
                            // using the most recent booking for this court (typically "khách chơi trễ" scenario).
                            creditedBookingId = TryMarkMostRecentEndedBookingPaidByCourtName(conn, tran, courtNameForLog);
                        }

                        int effectiveMemberId = ResolveMemberForCheckout(conn, tran, memberId, creditedBookingId);
                        int invoiceId = InsertInvoice(conn, tran, effectiveMemberId, totalAmount, discountAmount, finalAmount, paymentMethod);

                        // 1) Insert POS product lines only (ProductId > 0)
                        foreach (var line in lines)
                        {
                            if (line == null) continue;
                            if (line.ProductId <= 0) continue;
                            if (line.Quantity <= 0) continue;

                            EnsureStockAndMaybeReduce(conn, tran, line.ProductId, line.Quantity, line.ProductName, hasReduceStockTrigger);
                            InsertInvoiceDetail(conn, tran, invoiceId, productId: line.ProductId, bookingId: null, qty: line.Quantity, unitPrice: line.UnitPrice);
                        }

                        // 2) Insert court/service lines after we know which booking got credited.
                        // These lines should NOT reduce stock, so ProductID must be NULL.
                        foreach (var line in lines)
                        {
                            if (line == null) continue;
                            if (line.ProductId > 0) continue;
                            if (line.Quantity <= 0) continue;

                            int? bookingId = creditedBookingId > 0 ? (int?)creditedBookingId : null;
                            InsertInvoiceDetail(conn, tran, invoiceId, productId: null, bookingId: bookingId, qty: line.Quantity, unitPrice: line.UnitPrice);
                        }

                        // Reject accidental empty invoices.
                        if (lines.Count == 0 && finalAmount <= 0 && creditedBookingId == 0)
                            throw new InvalidOperationException("Không có dữ liệu thanh toán.");

                        if (effectiveMemberId > 0)
                        {
                            if (!hasUpdateMemberTrigger)
                            {
                                DatabaseHelper.ExecuteNonQuery(
                                    conn,
                                    tran,
                                    "UPDATE Members SET TotalSpent = ISNULL(TotalSpent, 0) + @FinalAmount WHERE MemberID = @MemberID",
                                    new SqlParameter("@FinalAmount", finalAmount),
                                    new SqlParameter("@MemberID", effectiveMemberId)
                                );
                            }

                            if (closedBookingId > 0)
                            {
                                object durationObj = DatabaseHelper.ExecuteScalar(
                                    conn, tran,
                                    "SELECT DATEDIFF(minute, StartTime, EndTime) / 60.0 FROM Bookings WHERE BookingID = @BookingID",
                                    new SqlParameter("@BookingID", closedBookingId)
                                );
                                decimal addedHours = (durationObj != null && durationObj != DBNull.Value) ? Convert.ToDecimal(durationObj) : 0m;
                                
                                if (addedHours > 0)
                                {
                                    DatabaseHelper.ExecuteNonQuery(
                                        conn, tran,
                                        "UPDATE Members SET TotalHoursPurchased = ISNULL(TotalHoursPurchased, 0) + @Hrs WHERE MemberID = @MemberID",
                                        new SqlParameter("@Hrs", addedHours),
                                        new SqlParameter("@MemberID", effectiveMemberId)
                                    );

                                    // Auto-upgrade to Fixed customer if TotalHoursPurchased >= 30
                                    DatabaseHelper.ExecuteNonQuery(
                                        conn, tran,
                                        "UPDATE Members SET IsFixed = 1 WHERE MemberID = @MemberID AND TotalHoursPurchased >= 30 AND IsFixed = 0",
                                        new SqlParameter("@MemberID", effectiveMemberId)
                                    );
                                }
                            }
                            else if (creditedBookingId > 0)
                            {
                                object durationObj = DatabaseHelper.ExecuteScalar(
                                    conn, tran,
                                    "SELECT DATEDIFF(minute, StartTime, EndTime) / 60.0 FROM Bookings WHERE BookingID = @BookingID",
                                    new SqlParameter("@BookingID", creditedBookingId)
                                );
                                decimal addedHours = (durationObj != null && durationObj != DBNull.Value) ? Convert.ToDecimal(durationObj) : 0m;

                                if (addedHours > 0)
                                {
                                    DatabaseHelper.ExecuteNonQuery(
                                        conn, tran,
                                        "UPDATE Members SET TotalHoursPurchased = ISNULL(TotalHoursPurchased, 0) + @Hrs WHERE MemberID = @MemberID",
                                        new SqlParameter("@Hrs", addedHours),
                                        new SqlParameter("@MemberID", effectiveMemberId)
                                    );

                                    DatabaseHelper.ExecuteNonQuery(
                                        conn, tran,
                                        "UPDATE Members SET IsFixed = 1 WHERE MemberID = @MemberID AND TotalHoursPurchased >= 30 AND IsFixed = 0",
                                        new SqlParameter("@MemberID", effectiveMemberId)
                                    );
                                }
                            }
                        }

                        DatabaseHelper.ExecuteNonQuery(
                            conn,
                            tran,
                            "INSERT INTO SystemLogs (EventDesc, SubDesc) VALUES (@EventDesc, @SubDesc)",
                            new SqlParameter("@EventDesc", "POS Checkout"),
                            new SqlParameter(
                                "@SubDesc",
                                $"InvoiceID={invoiceId}; MemberID={(effectiveMemberId > 0 ? effectiveMemberId.ToString() : "-")}; Court={courtNameForLog}; BookingID={(creditedBookingId > 0 ? creditedBookingId.ToString() : "-")}; Total={finalAmount:N0}đ; Method={paymentMethod}"
                            )
                        );

                        tran.Commit();
                        return invoiceId;
                    }
                    catch
                    {
                        try { tran.Rollback(); }
                        catch (Exception rbEx)
                        {
                            DatabaseHelper.TryLog("POS Rollback Error", rbEx, "PosService.Checkout");
                        }
                        throw;
                    }
                }
            }
        }

        private static int ResolveMemberForCheckout(SqlConnection conn, SqlTransaction tran, int memberId, int creditedBookingId)
        {
            if (memberId > 0) return memberId;
            if (creditedBookingId <= 0) return 0;

            object existingMemberObj = DatabaseHelper.ExecuteScalar(
                conn,
                tran,
                "SELECT MemberID FROM dbo.Bookings WHERE BookingID = @BookingID",
                new SqlParameter("@BookingID", creditedBookingId)
            );

            if (existingMemberObj != null && existingMemberObj != DBNull.Value)
            {
                int bookingMemberId = Convert.ToInt32(existingMemberObj);
                if (bookingMemberId > 0) return bookingMemberId;
            }

            string guestNameRaw = Convert.ToString(
                DatabaseHelper.ExecuteScalar(
                    conn,
                    tran,
                    "SELECT GuestName FROM dbo.Bookings WHERE BookingID = @BookingID",
                    new SqlParameter("@BookingID", creditedBookingId)
                )
            ) ?? "";

            ParseGuestInfo(guestNameRaw, out var fullName, out var phone);
            if (string.IsNullOrWhiteSpace(phone)) return 0;

            object existingByPhoneObj = DatabaseHelper.ExecuteScalar(
                conn,
                tran,
                "SELECT TOP (1) MemberID FROM dbo.Members WITH (UPDLOCK, HOLDLOCK) WHERE Phone = @Phone ORDER BY MemberID DESC",
                new SqlParameter("@Phone", phone)
            );

            int resolvedMemberId;

            if (existingByPhoneObj != null && existingByPhoneObj != DBNull.Value)
            {
                resolvedMemberId = Convert.ToInt32(existingByPhoneObj);

                if (!string.IsNullOrWhiteSpace(fullName))
                {
                    DatabaseHelper.ExecuteNonQuery(
                        conn,
                        tran,
                        "UPDATE dbo.Members SET FullName = @FullName WHERE MemberID = @MemberID AND (FullName IS NULL OR LTRIM(RTRIM(FullName)) = '')",
                        new SqlParameter("@FullName", fullName),
                        new SqlParameter("@MemberID", resolvedMemberId)
                    );
                }
            }
            else
            {
                object insertedObj = DatabaseHelper.ExecuteScalar(
                    conn,
                    tran,
                    @"INSERT INTO dbo.Members (FullName, Phone, IsFixed)
                      VALUES (@FullName, @Phone, 0);
                      SELECT CAST(SCOPE_IDENTITY() AS INT);",
                    new SqlParameter("@FullName", string.IsNullOrWhiteSpace(fullName) ? "Khach le" : fullName),
                    new SqlParameter("@Phone", phone)
                );

                if (insertedObj == null || insertedObj == DBNull.Value)
                    return 0;

                resolvedMemberId = Convert.ToInt32(insertedObj);
            }

            DatabaseHelper.ExecuteNonQuery(
                conn,
                tran,
                "UPDATE dbo.Bookings SET MemberID = @MemberID WHERE BookingID = @BookingID AND (MemberID IS NULL OR MemberID = 0)",
                new SqlParameter("@MemberID", resolvedMemberId),
                new SqlParameter("@BookingID", creditedBookingId)
            );

            return resolvedMemberId;
        }

        private static void ParseGuestInfo(string guestNameRaw, out string fullName, out string phone)
        {
            fullName = "";
            phone = "";

            string raw = (guestNameRaw ?? "").Trim();
            if (raw.Length == 0) return;

            int sep = raw.LastIndexOf(" - ", StringComparison.Ordinal);
            if (sep >= 0)
            {
                fullName = raw.Substring(0, sep).Trim();
                phone = raw.Substring(sep + 3).Trim();
            }
            else
            {
                int dash = raw.LastIndexOf('-');
                if (dash > 0 && dash < raw.Length - 1)
                {
                    fullName = raw.Substring(0, dash).Trim();
                    phone = raw.Substring(dash + 1).Trim();
                }
                else
                {
                    fullName = raw;
                }
            }

            phone = NormalizePhone(phone);

            if (string.IsNullOrWhiteSpace(fullName) && !string.IsNullOrWhiteSpace(phone))
                fullName = "Khach " + phone;
        }

        private static string NormalizePhone(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";

            var sb = new StringBuilder(input.Length);
            foreach (char c in input)
            {
                if (char.IsDigit(c)) sb.Append(c);
            }

            return sb.ToString();
        }

        private static bool TriggerExists(SqlConnection conn, SqlTransaction tran, string triggerName)
        {
            try
            {
                object result = DatabaseHelper.ExecuteScalar(
                    conn,
                    tran,
                    "SELECT 1 FROM sys.triggers WHERE name = @Name AND is_disabled = 0",
                    new SqlParameter("@Name", triggerName)
                );
                return result != null;
            }
            catch
            {
                return false;
            }
        }

        private static int InsertInvoice(SqlConnection conn, SqlTransaction tran, int memberId, decimal totalAmount, decimal discountAmount, decimal finalAmount, string paymentMethod)
        {
            var memberParam = new SqlParameter("@MemberID", SqlDbType.Int);
            memberParam.Value = memberId > 0 ? (object)memberId : DBNull.Value;

            object idObj = DatabaseHelper.ExecuteScalar(
                conn,
                tran,
                @"INSERT INTO Invoices (MemberID, TotalAmount, DiscountAmount, FinalAmount, PaymentMethod)
                  VALUES (@MemberID, @TotalAmount, @DiscountAmount, @FinalAmount, @PaymentMethod);
                  SELECT CAST(SCOPE_IDENTITY() AS INT);",
                memberParam,
                new SqlParameter("@TotalAmount", totalAmount),
                new SqlParameter("@DiscountAmount", discountAmount),
                new SqlParameter("@FinalAmount", finalAmount),
                new SqlParameter("@PaymentMethod", paymentMethod)
            );

            if (idObj == null || idObj == DBNull.Value)
                throw new InvalidOperationException("Không lấy được InvoiceID sau khi tạo hóa đơn.");

            return Convert.ToInt32(idObj);
        }

        private static void InsertInvoiceDetail(SqlConnection conn, SqlTransaction tran, int invoiceId, int productId, int qty, decimal unitPrice)
        {
            InsertInvoiceDetail(conn, tran, invoiceId, productId: productId, bookingId: null, qty: qty, unitPrice: unitPrice);
        }

        private static void InsertInvoiceDetail(SqlConnection conn, SqlTransaction tran, int invoiceId, int? productId, int? bookingId, int qty, decimal unitPrice)
        {
            var pProduct = new SqlParameter("@ProductID", SqlDbType.Int);
            pProduct.Value = productId.HasValue && productId.Value > 0 ? (object)productId.Value : DBNull.Value;

            var pBooking = new SqlParameter("@BookingID", SqlDbType.Int);
            pBooking.Value = bookingId.HasValue && bookingId.Value > 0 ? (object)bookingId.Value : DBNull.Value;

            DatabaseHelper.ExecuteNonQuery(
                conn,
                tran,
                "INSERT INTO InvoiceDetails (InvoiceID, ProductID, BookingID, Quantity, UnitPrice) VALUES (@InvoiceID, @ProductID, @BookingID, @Quantity, @UnitPrice)",
                new SqlParameter("@InvoiceID", invoiceId),
                pProduct,
                pBooking,
                new SqlParameter("@Quantity", qty),
                new SqlParameter("@UnitPrice", unitPrice)
            );
        }

        private static void EnsureStockAndMaybeReduce(SqlConnection conn, SqlTransaction tran, int productId, int qty, string prodName, bool hasReduceStockTrigger)
        {
            if (qty <= 0) return;

            if (hasReduceStockTrigger)
            {
                object stockObj = DatabaseHelper.ExecuteScalar(
                    conn,
                    tran,
                    "SELECT StockQuantity FROM Products WITH (UPDLOCK, ROWLOCK) WHERE ProductID = @ProductID",
                    new SqlParameter("@ProductID", productId)
                );

                if (stockObj == null || stockObj == DBNull.Value)
                    throw new InvalidOperationException($"Không tìm thấy sản phẩm: {prodName}");

                int stock = Convert.ToInt32(stockObj);
                if (stock < qty)
                    throw new InvalidOperationException($"Sản phẩm '{prodName}' không đủ hàng (còn {stock}, cần {qty}).");

                return; // Trigger will reduce after InvoiceDetails insert
            }

            int affected = DatabaseHelper.ExecuteNonQuery(
                conn,
                tran,
                "UPDATE Products SET StockQuantity = StockQuantity - @Qty WHERE ProductID = @ProductID AND StockQuantity >= @Qty",
                new SqlParameter("@Qty", qty),
                new SqlParameter("@ProductID", productId)
            );

            if (affected == 0)
                throw new InvalidOperationException($"Sản phẩm '{prodName}' không đủ hàng để trừ kho.");
        }

        private static int TryCloseActiveBookingByCourtName(SqlConnection conn, SqlTransaction tran, string courtName)
        {
            string trimmed = (courtName ?? "").Trim();
            if (string.IsNullOrWhiteSpace(trimmed))
                return 0;

            object bookingIdObj = DatabaseHelper.ExecuteScalar(
                conn,
                tran,
                @"DECLARE @Now DATETIME = GETDATE();
SELECT TOP (1) b.BookingID
FROM dbo.Bookings b
INNER JOIN dbo.Courts c ON c.CourtID = b.CourtID
WHERE c.Name = @CourtName
  AND b.Status <> 'Cancelled'
    AND b.Status <> 'Maintenance'
  AND b.StartTime <= @Now
  AND b.EndTime > @Now
ORDER BY b.StartTime DESC, b.BookingID DESC;",
                new SqlParameter("@CourtName", trimmed)
            );

            if (bookingIdObj == null || bookingIdObj == DBNull.Value)
                return 0;

            int bookingId = Convert.ToInt32(bookingIdObj);

            int affected = DatabaseHelper.ExecuteNonQuery(
                conn,
                tran,
                @"DECLARE @Now DATETIME = GETDATE();
UPDATE dbo.Bookings
SET EndTime = @Now,
    Status = @Status
WHERE BookingID = @BookingID
  AND Status <> 'Cancelled'
    AND Status <> 'Maintenance'
  AND StartTime <= @Now
  AND EndTime > @Now;",
                new SqlParameter("@Status", "Paid"),
                new SqlParameter("@BookingID", bookingId)
            );

            return affected > 0 ? bookingId : 0;
        }

        private static int TryMarkMostRecentEndedBookingPaidByCourtName(SqlConnection conn, SqlTransaction tran, string courtName)
        {
            string trimmed = (courtName ?? "").Trim();
            if (string.IsNullOrWhiteSpace(trimmed))
                return 0;

            // Pick the most recent booking that has already ended (EndTime <= now).
            // We constrain to a reasonable time window to avoid picking a very old booking.
            object bookingIdObj = DatabaseHelper.ExecuteScalar(
                conn,
                tran,
                @"DECLARE @Now DATETIME = GETDATE();
SELECT TOP (1) b.BookingID
FROM dbo.Bookings b
INNER JOIN dbo.Courts c ON c.CourtID = b.CourtID
WHERE c.Name = @CourtName
  AND b.Status <> 'Cancelled'
    AND b.Status <> 'Maintenance'
  AND b.StartTime <= @Now
  AND b.EndTime <= @Now
  AND b.EndTime >= DATEADD(HOUR, -12, @Now)
ORDER BY b.EndTime DESC, b.BookingID DESC;",
                new SqlParameter("@CourtName", trimmed)
            );

            if (bookingIdObj == null || bookingIdObj == DBNull.Value)
                return 0;

            int bookingId = Convert.ToInt32(bookingIdObj);

            // Mark it as paid if not already.
            DatabaseHelper.ExecuteNonQuery(
                conn,
                tran,
                "UPDATE dbo.Bookings SET Status = @Status WHERE BookingID = @BookingID AND Status <> 'Cancelled' AND Status <> 'Maintenance'",
                new SqlParameter("@Status", "Paid"),
                new SqlParameter("@BookingID", bookingId)
            );

            return bookingId;
        }
    }
}
