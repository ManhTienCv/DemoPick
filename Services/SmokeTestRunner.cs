using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DemoPick.Controllers;
using DemoPick.Models;

namespace DemoPick.Services
{
    internal static class SmokeTestRunner
    {
        internal static int Run(string[] args)
        {
            EnsureConsole();

            var startedAt = DateTime.Now;
            var swTotal = Stopwatch.StartNew();
            var steps = new List<SmokeTestStepResult>();

            string identifier = GetArg(args, "--id") ?? GetArg(args, "-id");
            string password = GetArg(args, "--pw") ?? GetArg(args, "-pw");

            string credentialSource = (!string.IsNullOrWhiteSpace(identifier) && !string.IsNullOrWhiteSpace(password))
                ? "args"
                : null;

            bool createdTempAccount = false;
            string tempUsername = null;

            try
            {
                // Step 1: DB init
                steps.Add(SmokeStepRunner.RunStep("DB init (schema + migrations)", () =>
                {
                    SchemaInstaller.EnsureDatabaseAndSchema();
                    MigrationsRunner.ApplyPendingMigrations();
                    return "OK";
                }));

                // Step 2: Obtain credentials (prefer user args)
                if (string.IsNullOrWhiteSpace(identifier) || string.IsNullOrWhiteSpace(password))
                {
                    // If DB is empty, seed admin (DEBUG only inside SchemaInstaller); we avoid UI there.
                    // Otherwise, register a unique temp user we can always login with.
                    steps.Add(SmokeStepRunner.RunStep("Obtain test credentials", () =>
                    {
                        // If there are zero accounts, try seeding admin here (works in both DEBUG/RELEASE).
                        if (AuthService.TrySeedAdminIfEmpty(out var seededUser, out var seededPass))
                        {
                            identifier = seededUser;
                            password = seededPass;
                            credentialSource = "seeded-admin";
                            return $"Seeded admin: {seededUser}";
                        }

                        string uniqueEmail = $"smoke_{Guid.NewGuid():N}@local";
                        string pw = "Smoke#" + Guid.NewGuid().ToString("N").Substring(0, 10) + "!";
                        if (!AuthService.TryRegister(
                                fullName: "Smoke Test",
                                email: uniqueEmail,
                                phone: null,
                                password: pw,
                                confirmPassword: pw,
                                out var err))
                        {
                            throw new InvalidOperationException(err ?? "Register failed");
                        }

                        identifier = uniqueEmail;
                        password = pw;
                        createdTempAccount = true;
                        tempUsername = uniqueEmail;
                        credentialSource = "temp-user";
                        return $"Registered temp user: {uniqueEmail}";
                    }));
                }

                // Step 3: Login
                steps.Add(SmokeStepRunner.RunStep("Login", () =>
                {
                    if (!AuthService.TryLogin(identifier, password, out var user, out var err))
                        throw new InvalidOperationException(err ?? "Login failed");

                    AppSession.SignIn(user);
                    if (AppSession.CurrentUser == null)
                        throw new InvalidOperationException("Session not set");

                    return $"Signed in as {AppSession.CurrentUser.Username} ({AppSession.CurrentUser.Role})";
                }));

                // Step 4: Load courts
                List<Models.CourtModel> courts = null;
                steps.Add(SmokeStepRunner.RunStep("Load courts", () =>
                {
                    var controller = new BookingController();
                    courts = controller.GetCourts();
                    if (courts == null || courts.Count == 0)
                        throw new InvalidOperationException("No active courts found");
                    return $"Courts: {courts.Count}";
                }));

                // Step 5: Create & cancel a booking (best-effort cleanup)
                steps.Add(SmokeStepRunner.RunStep("Create + cleanup test booking", () =>
                {
                    var controller = new BookingController();

                    int courtId = courts[0].CourtID;
                    string guest = $"SMOKE_{Guid.NewGuid():N}";
                    string note = "SMOKE TEST (auto)";

                    DateTime startBase = DateTime.Now.AddDays(3).Date.AddHours(17);
                    int[] durations = { 90, 60, 120 };

                    Exception last = null;
                    for (int dayOffset = 0; dayOffset <= 14; dayOffset++)
                    {
                        foreach (var dur in durations)
                        {
                            for (int hour = 6; hour <= 22; hour++)
                            {
                                DateTime start = startBase.AddDays(dayOffset).Date.AddHours(hour);
                                DateTime end = start.AddMinutes(dur);
                                if (start <= DateTime.Now) continue;

                                try
                                {
                                    controller.SubmitBooking(courtId, guest, note, start, end, status: AppConstants.BookingStatus.Confirmed);
                                    int bookingId = TryFindBookingId(courtId, guest, start, end);
                                    bool removed = TryDeleteBookingById(bookingId);
                                    return $"Created booking on CourtID={courtId} {start:yyyy-MM-dd HH:mm} ({dur}m). Removed={removed}";
                                }
                                catch (Exception ex)
                                {
                                    last = ex;
                                    // Conflict messages come from RAISERROR.
                                    if ((ex.Message ?? "").IndexOf("already booked", StringComparison.OrdinalIgnoreCase) >= 0)
                                        continue;
                                }
                            }
                        }
                    }

                    throw new InvalidOperationException("Unable to create a non-conflicting booking", last);
                }));

                // Step 6: Logout
                steps.Add(SmokeStepRunner.RunStep("Logout", () =>
                {
                    AppSession.SignOut();
                    if (AppSession.CurrentUser != null)
                        throw new InvalidOperationException("Session not cleared");
                    return "OK";
                }));

                // Step 7: Cleanup temp account (best-effort)
                if (createdTempAccount && !string.IsNullOrWhiteSpace(tempUsername))
                {
                    steps.Add(SmokeStepRunner.RunStep("Cleanup temp account", () =>
                    {
                        int rows = DatabaseHelper.ExecuteNonQuery(
                            "DELETE FROM dbo.StaffAccounts WHERE Username = @U OR (Email IS NOT NULL AND Email = @U)",
                            new SqlParameter("@U", tempUsername));
                        return $"Deleted rows: {rows}";
                    }));
                }

                // Step 7b: Cleanup legacy smoke artifacts from older test runs.
                steps.Add(SmokeStepRunner.RunStep("Cleanup legacy SMOKE artifacts", () =>
                {
                    return SmokeMaintenanceHelper.CleanupLegacySmokeArtifacts();
                }));

                // Step 8: Logic tests
                steps.Add(SmokeStepRunner.RunStep("Logic tests (PriceCalculator + PendingOrders)", () =>
                {
                    RunLogicTests();
                    return "All logic assertions passed";
                }));

                // Step 9: UI refactor tests
                steps.Add(SmokeStepRunner.RunStep("UI refactor tests (UCThanhToan + FrmXoaSP)", () =>
                {
                    return RunUiRefactorTests();
                }));

                // Step 10: Performance tests
                steps.Add(SmokeStepRunner.RunStep("Performance tests (micro-benchmark)", () =>
                {
                    return RunPerformanceTests();
                }));

                swTotal.Stop();

                string reportPath = SmokeReportWriter.WriteMarkdownReport(FindWorkspaceRoot(), startedAt, swTotal.Elapsed, steps, identifier, credentialSource);
                Console.WriteLine("\nSMOKE TEST: SUCCESS");
                Console.WriteLine("Report: " + reportPath);
                return 0;
            }
            catch (Exception fatal)
            {
                swTotal.Stop();
                steps.Add(new SmokeTestStepResult
                {
                    Name = "Fatal",
                    Success = false,
                    Duration = TimeSpan.Zero,
                    Details = fatal.Message,
                    Exception = fatal
                });

                string reportPath = SmokeReportWriter.WriteMarkdownReport(FindWorkspaceRoot(), startedAt, swTotal.Elapsed, steps, identifier, credentialSource);
                Console.WriteLine("\nSMOKE TEST: FAILED");
                Console.WriteLine(fatal.ToString());
                Console.WriteLine("Report: " + reportPath);
                return 1;
            }
        }

        private static int TryFindBookingId(int courtId, string guest, DateTime start, DateTime end)
        {
            try
            {
                object obj = DatabaseHelper.ExecuteScalar(
                    "SELECT TOP 1 BookingID FROM dbo.Bookings WHERE CourtID=@C AND GuestName=@G AND StartTime=@S AND EndTime=@E ORDER BY BookingID DESC",
                    new SqlParameter("@C", courtId),
                    new SqlParameter("@G", guest),
                    new SqlParameter("@S", start),
                    new SqlParameter("@E", end));

                if (obj == null || obj == DBNull.Value) return 0;
                return Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                DatabaseHelper.TryLogThrottled(
                    throttleKey: "SmokeTestRunner.TryFindBookingId",
                    eventDesc: "Smoke DB Lookup Error",
                    ex: ex,
                    context: $"TryFindBookingId courtId={courtId}, guest={guest}, start={start:O}, end={end:O}",
                    minSeconds: 300);
                return 0;
            }
        }

        private static bool TryDeleteBookingById(int bookingId)
        {
            if (bookingId <= 0) return false;

            try
            {
                int rows = DatabaseHelper.ExecuteNonQuery(
                    "DELETE FROM dbo.Bookings WHERE BookingID = @Id",
                    new SqlParameter("@Id", bookingId));
                return rows > 0;
            }
            catch (Exception ex)
            {
                DatabaseHelper.TryLogThrottled(
                    throttleKey: "SmokeTestRunner.TryDeleteBookingById",
                    eventDesc: "Smoke DB Cleanup Error",
                    ex: ex,
                    context: $"TryDeleteBookingById bookingId={bookingId}",
                    minSeconds: 300);
                return false;
            }
        }

        private static void RunLogicTests()
        {
            // Case 1: invalid range -> zero totals
            var p1 = PriceCalculator.CalculateTotal(
                new DateTime(2026, 4, 7, 10, 0, 0),
                new DateTime(2026, 4, 7, 10, 0, 0),
                isFixedCustomer: false,
                services: null,
                courtRateMultiplier: 1m);

            AssertDecimal("Case1.SubtotalCourts", 0m, p1.SubtotalCourts);
            AssertDecimal("Case1.Discount", 0m, p1.DiscountAmount);
            AssertDecimal("Case1.GrandTotal", 0m, p1.GrandTotal);

            // Case 2: weekday, fixed customer, 17:00-18:30
            var p2 = PriceCalculator.CalculateTotal(
                new DateTime(2026, 4, 6, 17, 0, 0), // Monday
                new DateTime(2026, 4, 6, 18, 30, 0),
                isFixedCustomer: true,
                services: null,
                courtRateMultiplier: 1m);

            AssertDecimal("Case2.SubtotalCourts", 300000m, p2.SubtotalCourts);
            AssertDecimal("Case2.Discount", 30000m, p2.DiscountAmount);
            AssertDecimal("Case2.GrandTotal", 270000m, p2.GrandTotal);

            // Case 3: weekend + per-hour service
            var services = new List<ServiceCharge>
            {
                new ServiceCharge
                {
                    ProductID = 1,
                    ServiceName = "May ban bong",
                    Unit = "Gio",
                    Quantity = 1,
                    UnitPrice = 50000m
                }
            };

            var p3 = PriceCalculator.CalculateTotal(
                new DateTime(2026, 4, 11, 10, 0, 0), // Saturday
                new DateTime(2026, 4, 11, 12, 0, 0),
                isFixedCustomer: false,
                services: services,
                courtRateMultiplier: 1m);

            AssertDecimal("Case3.SubtotalCourts", 300000m, p3.SubtotalCourts);
            AssertDecimal("Case3.SubtotalServices", 50000m, p3.SubtotalServices);
            AssertDecimal("Case3.Discount", 0m, p3.DiscountAmount);
            AssertDecimal("Case3.GrandTotal", 350000m, p3.GrandTotal);

            // Case 4: split across two blocks 08:30-09:30
            var p4 = PriceCalculator.CalculateTotal(
                new DateTime(2026, 4, 7, 8, 30, 0),
                new DateTime(2026, 4, 7, 9, 30, 0),
                isFixedCustomer: false,
                services: null,
                courtRateMultiplier: 1m);

            AssertDecimal("Case4.SubtotalCourts", 130000m, p4.SubtotalCourts);
            if (p4.TimeSlots == null || p4.TimeSlots.Count < 2)
                throw new InvalidOperationException("Case4 expected at least 2 split time slots");

            // Case 5: court multiplier
            var p5 = PriceCalculator.CalculateTotal(
                new DateTime(2026, 4, 6, 17, 0, 0),
                new DateTime(2026, 4, 6, 18, 30, 0),
                isFixedCustomer: true,
                services: null,
                courtRateMultiplier: 0.5m);

            AssertDecimal("Case5.SubtotalCourts", 150000m, p5.SubtotalCourts);
            AssertDecimal("Case5.Discount", 15000m, p5.DiscountAmount);
            AssertDecimal("Case5.GrandTotal", 135000m, p5.GrandTotal);

            // PendingOrders in-memory state
            PosService.ClearPendingOrder("San Test");
            var lines = new List<CartLine>
            {
                new CartLine(1, "Nuoc", 2, 10000m)
            };
            PosService.SavePendingOrder("San Test", lines);
            var readBack = PosService.GetPendingOrder("San Test");
            if (readBack.Count != 1 || readBack[0].Quantity != 2)
                throw new InvalidOperationException("PendingOrders save/get logic failed");
            PosService.ClearPendingOrder("San Test");

            // Case 6: DB integration - auto create member when checkout with guest phone
            RunAutoCreateMemberIntegrationTest();
        }

        private static string RunUiRefactorTests()
        {
            var summary = new StringBuilder();

            using (var reprintPanel = new DemoPick.UCInvoiceReprintPanel())
            {
                bool byIdRaised = false;
                bool lastRaised = false;

                reprintPanel.ReprintByIdRequested += (s, e) => byIdRaised = true;
                reprintPanel.ReprintLastRequested += (s, e) => lastRaised = true;

                reprintPanel.InvoiceIdText = "12345";
                if (!string.Equals(reprintPanel.InvoiceIdText, "12345", StringComparison.Ordinal))
                    throw new InvalidOperationException("UCInvoiceReprintPanel InvoiceIdText get/set failed");

                reprintPanel.SetLastInvoiceEnabled(true);
                var reprintLastBtn = SmokeUiReflectionHelper.GetPrivateField<Sunny.UI.UIButton>(reprintPanel, "btnReprintLast");
                if (reprintLastBtn == null)
                    throw new InvalidOperationException("UCInvoiceReprintPanel missing btnReprintLast");
                if (!reprintLastBtn.Enabled)
                    throw new InvalidOperationException("UCInvoiceReprintPanel SetLastInvoiceEnabled(true) did not enable button");

                SmokeUiReflectionHelper.InvokePrivateMethod(reprintPanel, "BtnReprintById_Click", reprintPanel, EventArgs.Empty);
                SmokeUiReflectionHelper.InvokePrivateMethod(reprintPanel, "BtnReprintLast_Click", reprintPanel, EventArgs.Empty);

                if (!byIdRaised)
                    throw new InvalidOperationException("UCInvoiceReprintPanel did not raise ReprintByIdRequested");
                if (!lastRaised)
                    throw new InvalidOperationException("UCInvoiceReprintPanel did not raise ReprintLastRequested");

                summary.Append("ReprintPanel=OK; ");
            }

            using (var historyPanel = new DemoPick.UCPaymentHistoryPanel())
            {
                bool searchRaised = false;
                bool openRaised = false;

                historyPanel.SearchRequested += (s, e) => searchRaised = true;
                historyPanel.OpenRequested += (s, e) => openRaised = true;

                historyPanel.SearchKeyword = "SMOKE";
                if (!string.Equals(historyPanel.SearchKeyword, "SMOKE", StringComparison.Ordinal))
                    throw new InvalidOperationException("UCPaymentHistoryPanel SearchKeyword get/set failed");

                var rows = new List<DemoPick.UCPaymentHistoryPanel.HistoryRow>
                {
                    new DemoPick.UCPaymentHistoryPanel.HistoryRow
                    {
                        InvoiceCode = "1",
                        TimeText = "18/04 10:00",
                        CustomerText = "A",
                        TotalText = "10.000đ",
                        ToolTipText = "Sân: A",
                        Tag = "A"
                    },
                    new DemoPick.UCPaymentHistoryPanel.HistoryRow
                    {
                        InvoiceCode = "2",
                        TimeText = "18/04 11:00",
                        CustomerText = "B",
                        TotalText = "20.000đ",
                        ToolTipText = "Sân: B",
                        IsHighlighted = true,
                        Tag = "B"
                    }
                };

                historyPanel.BindRows(rows);

                var list = SmokeUiReflectionHelper.GetPrivateField<ListView>(historyPanel, "lstHistory");
                if (list == null)
                    throw new InvalidOperationException("UCPaymentHistoryPanel missing lstHistory");
                if (list.Items.Count != 2)
                    throw new InvalidOperationException("UCPaymentHistoryPanel BindRows did not render expected row count");

                var selectedTag = list.Items[1].Tag as string;
                if (!string.Equals(selectedTag, "B", StringComparison.Ordinal))
                    throw new InvalidOperationException("UCPaymentHistoryPanel BindRows did not preserve row Tag correctly");

                if (list.Items[1].BackColor != System.Drawing.Color.FromArgb(239, 246, 255))
                    throw new InvalidOperationException("UCPaymentHistoryPanel highlighted row style was not applied");

                SmokeUiReflectionHelper.InvokePrivateMethod(historyPanel, "BtnSearch_Click", historyPanel, EventArgs.Empty);
                SmokeUiReflectionHelper.InvokePrivateMethod(historyPanel, "BtnOpen_Click", historyPanel, EventArgs.Empty);

                if (!searchRaised)
                    throw new InvalidOperationException("UCPaymentHistoryPanel did not raise SearchRequested");
                if (!openRaised)
                    throw new InvalidOperationException("UCPaymentHistoryPanel did not raise OpenRequested");

                summary.Append("PaymentHistoryPanel=OK; ");
            }

            using (var checkout = new DemoPick.UCThanhToan())
            {
                if (checkout.ucInvoiceReprintPanel == null || checkout.ucInvoiceReprintPanel.Parent != checkout.pnlTotals)
                    throw new InvalidOperationException("UCThanhToan reprint panel not attached to pnlTotals");

                if (checkout.ucPaymentHistoryPanel == null || checkout.ucPaymentHistoryPanel.Parent != checkout.pnlRight)
                    throw new InvalidOperationException("UCThanhToan payment history panel not attached to pnlRight");

                summary.Append("UCThanhToanEmbed=OK; ");
            }

            using (var frm = new DemoPick.FrmXoaSP())
            {
                var list = SmokeUiReflectionHelper.GetPrivateField<ListView>(frm, "_lstProducts");
                var pnlBottom = SmokeUiReflectionHelper.GetPrivateField<Panel>(frm, "_pnlBottom");
                var btnClose = SmokeUiReflectionHelper.GetPrivateField<Sunny.UI.UIButton>(frm, "_btnClose");
                var btnDelete = SmokeUiReflectionHelper.GetPrivateField<Sunny.UI.UIButton>(frm, "_btnDelete");

                if (list == null || list.Columns.Count != 5)
                    throw new InvalidOperationException("FrmXoaSP designer ListView columns not initialized as expected");

                if (pnlBottom == null || btnClose == null || btnDelete == null)
                    throw new InvalidOperationException("FrmXoaSP designer controls missing");

                pnlBottom.Width = 1000;
                SmokeUiReflectionHelper.InvokePrivateMethod(frm, "PnlBottom_Resize", pnlBottom, EventArgs.Empty);

                if (!(btnDelete.Left < btnClose.Left))
                    throw new InvalidOperationException("FrmXoaSP button layout rule violated (_btnDelete should be left of _btnClose)");

                summary.Append("FrmXoaSPDesigner=OK");
            }

            return summary.ToString();
        }

        private static string RunPerformanceTests()
        {
            const int loops = 20000;
            string root = FindWorkspaceRoot();

#if DEBUG
            bool enforceBaseline = false;
            bool persistBaselineIfMissing = false;
#else
            bool enforceBaseline = true;
            bool persistBaselineIfMissing = true;
#endif

            var start = new DateTime(2026, 4, 6, 17, 0, 0);
            var end = new DateTime(2026, 4, 6, 18, 30, 0);
            var services = new List<ServiceCharge>
            {
                new ServiceCharge
                {
                    ProductID = 2,
                    ServiceName = "Bong",
                    Unit = "Cai",
                    Quantity = 4,
                    UnitPrice = 15000m
                }
            };

            var swPrice = Stopwatch.StartNew();
            decimal checksum = 0m;
            for (int i = 0; i < loops; i++)
            {
                var b = PriceCalculator.CalculateTotal(start, end, isFixedCustomer: (i % 2 == 0), services: services, courtRateMultiplier: 1m);
                checksum += b.GrandTotal;
            }
            swPrice.Stop();

            var swPending = Stopwatch.StartNew();
            for (int i = 0; i < loops; i++)
            {
                string key = "perf-" + (i % 50).ToString();
                PosService.SavePendingOrder(key, new List<CartLine>
                {
                    new CartLine(1, "Nuoc", 1, 10000m)
                });
                var _ = PosService.GetPendingOrder(key);
            }
            swPending.Stop();

            double priceOps = loops / Math.Max(0.001, swPrice.Elapsed.TotalSeconds);
            double pendingOps = loops / Math.Max(0.001, swPending.Elapsed.TotalSeconds);

            var baseline = SmokePerformancePersistence.LoadOrCreatePerfBaseline(root, Environment.MachineName, priceOps, pendingOps, persistBaselineIfMissing);

            string mode = enforceBaseline ? "baseline-guard" : "debug-no-guard";

            var results = new List<SmokeModulePerfResult>
            {
                new SmokeModulePerfResult
                {
                    Module = "PriceCalculator",
                    Iterations = loops,
                    ElapsedMs = swPrice.ElapsedMilliseconds,
                    OpsPerSec = priceOps,
                    ThresholdOpsPerSec = baseline.PriceCalcMinOps,
                    Passed = !enforceBaseline || priceOps >= baseline.PriceCalcMinOps,
                    Mode = mode
                },
                new SmokeModulePerfResult
                {
                    Module = "PendingOrders",
                    Iterations = loops,
                    ElapsedMs = swPending.ElapsedMilliseconds,
                    OpsPerSec = pendingOps,
                    ThresholdOpsPerSec = baseline.PendingOrdersMinOps,
                    Passed = !enforceBaseline || pendingOps >= baseline.PendingOrdersMinOps,
                    Mode = mode
                }
            };

            string perfReportPath = SmokePerformancePersistence.WritePerformanceModuleReport(root, results, checksum);
            SmokePerformancePersistence.AppendPerformanceHistory(root, results);

            foreach (var r in results)
            {
                if (enforceBaseline && !r.Passed)
                {
                    throw new InvalidOperationException(
                        $"Performance regression: {r.Module} {r.OpsPerSec:F0} ops/s < baseline {r.ThresholdOpsPerSec:F0} ops/s. See {perfReportPath}"
                    );
                }
            }

            return $"PriceCalc: {loops} loops in {swPrice.ElapsedMilliseconds}ms ({priceOps:F0} ops/s, min {baseline.PriceCalcMinOps:F0}), PendingOrders: {loops} loops in {swPending.ElapsedMilliseconds}ms ({pendingOps:F0} ops/s, min {baseline.PendingOrdersMinOps:F0}), checksum={checksum:N0}, report={perfReportPath}";
        }

        private static void RunAutoCreateMemberIntegrationTest()
        {
            var controller = new BookingController();
            var courts = controller.GetCourts();
            var bookingsToday = controller.GetBookingsByDate(DateTime.Now);
            var now = DateTime.Now;

            Models.CourtModel targetCourt = null;
            foreach (var c in courts)
            {
                bool occupied = false;
                foreach (var b in bookingsToday)
                {
                    if (b.CourtID != c.CourtID) continue;
                    if (string.Equals(b.Status, AppConstants.BookingStatus.Cancelled, StringComparison.OrdinalIgnoreCase)) continue;
                    if (string.Equals(b.Status, AppConstants.BookingStatus.Maintenance, StringComparison.OrdinalIgnoreCase)) continue;
                    if (b.StartTime <= now && b.EndTime > now)
                    {
                        occupied = true;
                        break;
                    }
                }

                if (!occupied)
                {
                    targetCourt = c;
                    break;
                }
            }

            if (targetCourt == null)
                throw new InvalidOperationException("No available court for auto-member integration test");

            string phone = SmokeMaintenanceHelper.BuildUniquePhone();
            string guest = "SMOKE AUTO " + Guid.NewGuid().ToString("N").Substring(0, 6) + " - " + phone;
            DateTime start = now.AddMinutes(-20);
            DateTime end = now.AddMinutes(40);

            bool hadMemberBefore = SmokeMaintenanceHelper.HasMemberByPhone(phone);
            if (hadMemberBefore)
                throw new InvalidOperationException("Generated phone already exists; cannot run isolated auto-member test");

            int bookingId = 0;
            int invoiceId = 0;
            int memberId = 0;

            try
            {
                controller.SubmitBooking(targetCourt.CourtID, guest, "SMOKE AUTO MEMBER", start, end, status: AppConstants.BookingStatus.Confirmed);
                bookingId = TryFindBookingId(targetCourt.CourtID, guest, start, end);
                if (bookingId <= 0)
                    throw new InvalidOperationException("Cannot find booking created for auto-member test");

                var lines = new List<CartLine>
                {
                    new CartLine(-1, "Gio san smoke", 1, 120000m)
                };

                var pos = new PosService();
                invoiceId = pos.Checkout(0, lines, 120000m, 0m, 120000m, "Cash", targetCourt.Name);
                if (invoiceId <= 0)
                    throw new InvalidOperationException("Checkout did not return a valid InvoiceID");

                object memberObj = DatabaseHelper.ExecuteScalar(
                    "SELECT MemberID FROM dbo.Invoices WHERE InvoiceID = @Id",
                    new SqlParameter("@Id", invoiceId)
                );
                if (memberObj == null || memberObj == DBNull.Value)
                    throw new InvalidOperationException("Invoice.MemberID is null; auto-create member failed");

                memberId = Convert.ToInt32(memberObj);
                if (memberId <= 0)
                    throw new InvalidOperationException("Invoice.MemberID invalid; auto-create member failed");

                var memberDt = DatabaseHelper.ExecuteQuery(
                    "SELECT Phone, IsFixed, TotalSpent, TotalHoursPurchased FROM dbo.Members WHERE MemberID = @Id",
                    new SqlParameter("@Id", memberId)
                );
                if (memberDt.Rows.Count == 0)
                    throw new InvalidOperationException("Created member not found in Members table");

                string dbPhone = Convert.ToString(memberDt.Rows[0]["Phone"]);
                if (!string.Equals(dbPhone, phone, StringComparison.Ordinal))
                    throw new InvalidOperationException($"Phone mismatch after auto-create member. expected={phone}, actual={dbPhone}");

                decimal totalSpent = Convert.ToDecimal(memberDt.Rows[0]["TotalSpent"]);
                if (totalSpent < 120000m)
                    throw new InvalidOperationException($"TotalSpent not accumulated. expected>=120000, actual={totalSpent}");

                decimal totalHours = Convert.ToDecimal(memberDt.Rows[0]["TotalHoursPurchased"]);
                if (totalHours <= 0m)
                    throw new InvalidOperationException($"TotalHoursPurchased not accumulated. actual={totalHours}");

                var bookingDt = DatabaseHelper.ExecuteQuery(
                    "SELECT MemberID, Status FROM dbo.Bookings WHERE BookingID = @Id",
                    new SqlParameter("@Id", bookingId)
                );
                if (bookingDt.Rows.Count == 0)
                    throw new InvalidOperationException("Booking not found after checkout");

                int bookingMemberId = bookingDt.Rows[0]["MemberID"] == DBNull.Value ? 0 : Convert.ToInt32(bookingDt.Rows[0]["MemberID"]);
                if (bookingMemberId != memberId)
                    throw new InvalidOperationException($"Booking.MemberID mismatch. expected={memberId}, actual={bookingMemberId}");

                string status = Convert.ToString(bookingDt.Rows[0]["Status"]);
                if (!string.Equals(status, AppConstants.BookingStatus.Paid, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException($"Booking status should be {AppConstants.BookingStatus.Paid} after checkout, actual={status}");
            }
            finally
            {
                // Best-effort cleanup for smoke artifacts.
                try
                {
                    if (invoiceId > 0)
                    {
                        DatabaseHelper.ExecuteNonQuery("DELETE FROM dbo.InvoiceDetails WHERE InvoiceID = @Id", new SqlParameter("@Id", invoiceId));
                        DatabaseHelper.ExecuteNonQuery("DELETE FROM dbo.Invoices WHERE InvoiceID = @Id", new SqlParameter("@Id", invoiceId));

                        // Also remove POS checkout logs that reference this invoice.
                        try
                        {
                            DatabaseHelper.ExecuteNonQuery(
                                "DELETE FROM dbo.SystemLogs WHERE EventDesc = N'POS Checkout' AND (SubDesc LIKE @Legacy OR SubDesc LIKE @NewFmt)",
                                new SqlParameter("@Legacy", $"InvoiceID={invoiceId}%"),
                                new SqlParameter("@NewFmt", $"HĐ #{invoiceId}%")
                            );
                        }
                        catch { }
                    }
                }
                catch { }

                try
                {
                    if (bookingId > 0)
                        DatabaseHelper.ExecuteNonQuery("DELETE FROM dbo.Bookings WHERE BookingID = @Id", new SqlParameter("@Id", bookingId));
                }
                catch { }

                try
                {
                    if (memberId > 0)
                        DatabaseHelper.ExecuteNonQuery("DELETE FROM dbo.Members WHERE MemberID = @Id", new SqlParameter("@Id", memberId));
                }
                catch { }
            }
        }

        private static void AssertDecimal(string name, decimal expected, decimal actual, decimal tolerance = 0.001m)
        {
            decimal diff = Math.Abs(expected - actual);
            if (diff > tolerance)
                throw new InvalidOperationException($"{name} expected {expected}, actual {actual}, diff {diff}");
        }


        private static string FindWorkspaceRoot()
        {
            try
            {
                string dir = AppDomain.CurrentDomain.BaseDirectory;
                for (int i = 0; i < 8 && !string.IsNullOrWhiteSpace(dir); i++)
                {
                    if (File.Exists(Path.Combine(dir, "DemoPick.sln")) || Directory.Exists(Path.Combine(dir, "Docs")))
                        return dir;

                    var parent = Directory.GetParent(dir);
                    if (parent == null) break;
                    dir = parent.FullName;
                }
            }
            catch
            {
                // ignore
            }

            return Environment.CurrentDirectory;
        }

        private static string GetArg(string[] args, string key)
        {
            if (args == null || args.Length == 0) return null;
            for (int i = 0; i < args.Length; i++)
            {
                if (!string.Equals(args[i], key, StringComparison.OrdinalIgnoreCase))
                    continue;
                if (i + 1 < args.Length)
                    return args[i + 1];
                return null;
            }
            return null;
        }

        private static void EnsureConsole()
        {
            try
            {
                // WinExe doesn't have a console by default. Attach to parent console when launched from cmd.
                if (!AttachConsole(ATTACH_PARENT_PROCESS))
                {
                    AllocConsole();
                }

                try { Console.OutputEncoding = Encoding.UTF8; } catch { }
            }
            catch
            {
                // ignore
            }
        }

        private const int ATTACH_PARENT_PROCESS = -1;

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AttachConsole(int dwProcessId);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AllocConsole();
    }
}
