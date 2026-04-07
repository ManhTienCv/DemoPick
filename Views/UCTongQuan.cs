using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DemoPick.Services;

namespace DemoPick
{
    public partial class UCTongQuan : UserControl
    {
        public UCTongQuan()
        {
            InitializeComponent();

            if (DesignModeUtil.IsDesignMode(this))
            {
                return;
            }
            SetupCharts();
            SetupTable();
            AttachBorders();
            LoadRealDataAsync();
        }

        private void AttachBorders()
        {
            PaintEventHandler drawBorder = (s, e) => {
                var pnl = s as Panel;
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(229, 231, 235), 1), 0, 0, pnl.Width - 1, pnl.Height - 1);
            };
            pnlCard1.Paint += drawBorder;
            pnlCard2.Paint += drawBorder;
            pnlCard3.Paint += drawBorder;
            pnlCard4.Paint += drawBorder;
            pnlChartBorder.Paint += drawBorder;
            pnlPieBorder.Paint += drawBorder;
            pnlTable.Paint += drawBorder;
        }

        private void SetupCharts()
        {
            // Trend Area Chart
            chartTrend.ChartAreas.Add(new ChartArea("MainArea"));
            chartTrend.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(243, 244, 246);
            chartTrend.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(243, 244, 246);
            chartTrend.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Gray;
            chartTrend.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Gray;

            Series s = new Series("Revenue");
            s.ChartType = SeriesChartType.Area;
            s.Color = Color.FromArgb(100, 76, 175, 80); // Semi-transparent green
            s.BorderColor = Color.FromArgb(76, 175, 80); // Solid green line
            s.BorderWidth = 2;
            
            // To be populated by LoadRealDataAsync
            chartTrend.Series.Add(s);
            chartTrend.Legends.Clear();

            // Doughnut / Pie Chart
            chartPie.ChartAreas.Add(new ChartArea("PieArea"));
            chartPie.ChartAreas[0].BackColor = Color.White;
            Series sp = new Series("Tỉ trọng");
            sp.ChartType = SeriesChartType.Doughnut;
            sp.BorderWidth = 2;
            sp.BorderColor = Color.White;
            
            // To be populated by LoadRealDataAsync
            chartPie.Series.Add(sp);
            
            Legend l = new Legend("Legend");
            l.Docking = Docking.Bottom;
            l.Alignment = StringAlignment.Center;
            chartPie.Legends.Add(l);
        }

        private void SetupTable()
        {
            lstBookings.Columns.Add("Mã Đặt", 100);
            lstBookings.Columns.Add("Sân", 200);
            lstBookings.Columns.Add("Khách hàng", 250);
            lstBookings.Columns.Add("Thời gian", 200);
            lstBookings.Columns.Add("Trạng thái", 150);
            lstBookings.Columns.Add("Doanh thu", 150);
            lstBookings.Items.Clear();
        }

        private void UCTongQuan_Load(object sender, EventArgs e)
        {
            LoadRealDataAsync();
        }

        private void LoadRealDataAsync()
        {
            try
            {
                // 1. TOP CARDS (Dashboard metrics)
                var dtMetrics = DemoPick.Services.DatabaseHelper.ExecuteQuery(@"
                    DECLARE @posRev DECIMAL(18,2) = ISNULL((SELECT SUM(FinalAmount) FROM Invoices), 0);
                    DECLARE @courtRev DECIMAL(18,2) = ISNULL((
                        SELECT SUM((DATEDIFF(minute, B.StartTime, B.EndTime) / 60.0) * C.HourlyRate)
                        FROM Bookings B
                        JOIN Courts C ON B.CourtID = C.CourtID
                        WHERE B.Status != 'Cancelled'
                          AND B.Status != 'Maintenance'
                    ), 0);
                    DECLARE @totalRev DECIMAL(18,2) = @posRev + @courtRev;
                    DECLARE @totalCust INT = (SELECT COUNT(*) FROM Members);
                    DECLARE @total INT = (SELECT COUNT(*) * 16 FROM Courts WHERE Status='Active');
                    DECLARE @booked INT = (SELECT ISNULL(SUM(DATEDIFF(minute, StartTime, EndTime)/60.0),0) FROM Bookings WHERE Status != 'Cancelled' AND Status != 'Maintenance');
                    DECLARE @occ INT = CASE WHEN @total = 0 THEN 0 ELSE CAST((@booked * 100.0 / @total) AS INT) END;
                    DECLARE @pos INT = (SELECT COUNT(*) FROM Invoices);
                    
                    SELECT @totalRev as Rev, @totalCust as Cust, @occ as Occ, @pos as POS;
                ");
                
                if (dtMetrics.Rows.Count > 0)
                {
                    decimal rev = Convert.ToDecimal(dtMetrics.Rows[0]["Rev"]);
                    int occ = Convert.ToInt32(dtMetrics.Rows[0]["Occ"]);
                    int cust = Convert.ToInt32(dtMetrics.Rows[0]["Cust"]);
                    int pos = Convert.ToInt32(dtMetrics.Rows[0]["POS"]);

                    lblC1V.Text = rev == 0 ? "0 đ" : rev.ToString("N0") + "đ";
                    lblC2V.Text = occ.ToString() + "%";
                    lblC3V.Text = cust.ToString();
                    lblC4V.Text = pos.ToString();

                    lblC1S.Text = "+0% ↗"; // placeholder for delta
                    lblC2S.Text = "+0% ↗";
                    lblC3S.Text = "+0.0% ↗";
                    lblC4S.Text = "+0% ↗";
                }

                // 2. TREND CHART (Last 7 Days)
                chartTrend.Series[0].Points.Clear();
                var dtTrend = DemoPick.Services.DatabaseHelper.ExecuteQuery(@"
                    WITH Last7Days AS (
                        SELECT CAST(GETDATE() - 6 AS DATE) as Dt UNION ALL
                        SELECT CAST(GETDATE() - 5 AS DATE) UNION ALL
                        SELECT CAST(GETDATE() - 4 AS DATE) UNION ALL
                        SELECT CAST(GETDATE() - 3 AS DATE) UNION ALL
                        SELECT CAST(GETDATE() - 2 AS DATE) UNION ALL
                        SELECT CAST(GETDATE() - 1 AS DATE) UNION ALL
                        SELECT CAST(GETDATE() AS DATE)
                    )
                    SELECT 
                        FORMAT(D.Dt, 'dd/MM') as Label,
                        ISNULL(SUM(I.FinalAmount), 0) + ISNULL(SUM(BR.CourtRevenue), 0) as Revenue
                    FROM Last7Days D
                    LEFT JOIN Invoices I ON CAST(I.CreatedAt AS DATE) = D.Dt
                    LEFT JOIN (
                        SELECT
                            CAST(B.StartTime AS DATE) as Dt,
                            SUM((DATEDIFF(minute, B.StartTime, B.EndTime) / 60.0) * C.HourlyRate) as CourtRevenue
                        FROM Bookings B
                        JOIN Courts C ON B.CourtID = C.CourtID
                        WHERE B.Status != 'Cancelled'
                                                    AND B.Status != 'Maintenance'
                        GROUP BY CAST(B.StartTime AS DATE)
                    ) BR ON BR.Dt = D.Dt
                    GROUP BY D.Dt
                    ORDER BY D.Dt
                ");
                foreach (System.Data.DataRow r in dtTrend.Rows)
                {
                    chartTrend.Series[0].Points.AddXY(r["Label"].ToString(), Convert.ToDecimal(r["Revenue"]));
                }

                // 3. PIE CHART (Sân Doanh thu)
                chartPie.Series[0].Points.Clear();
                var dtPie = DemoPick.Services.DatabaseHelper.ExecuteQuery(@"
                    SELECT TOP 4
                        C.Name,
                        ISNULL(SUM(CASE
                            WHEN B.BookingID IS NULL THEN 0
                            ELSE (DATEDIFF(minute, B.StartTime, B.EndTime) / 60.0) * C.HourlyRate
                        END), 0) as Rev
                    FROM Courts C
                    LEFT JOIN Bookings B ON C.CourtID = B.CourtID AND B.Status != 'Cancelled' AND B.Status != 'Maintenance'
                    GROUP BY C.Name
                    ORDER BY Rev DESC
                ");
                
                if (dtPie.Rows.Count == 0 || (dtPie.Rows.Count > 0 && Convert.ToDecimal(dtPie.Rows[0]["Rev"]) == 0))
                {
                    chartPie.Series[0].Points.AddXY("Chưa có D.Thu", 100);
                    chartPie.Series[0].Points[0].Color = Color.LightGray;
                }
                else
                {
                    Color[] colors = { Color.FromArgb(76, 175, 80), Color.FromArgb(129, 199, 132), Color.FromArgb(165, 214, 167), Color.FromArgb(232, 245, 233) };
                    int i = 0;
                    foreach (System.Data.DataRow r in dtPie.Rows)
                    {
                        if (Convert.ToDecimal(r["Rev"]) > 0)
                        {
                            chartPie.Series[0].Points.AddXY(r["Name"].ToString(), Convert.ToDecimal(r["Rev"]));
                            chartPie.Series[0].Points[chartPie.Series[0].Points.Count - 1].Color = colors[i % colors.Length];
                            i++;
                        }
                    }
                }

                // 4. RECENT ACTIVITY
                lstBookings.Items.Clear();
                var dtActivity = DemoPick.Services.DatabaseHelper.ExecuteQuery(@"
                    SELECT TOP 10 
                        '#BK' + CAST(B.BookingID as VARCHAR) as Mã,
                        C.Name as Sân,
                        COALESCE(M.FullName, B.GuestName, N'Khách lẻ') as Khách,
                        FORMAT(B.StartTime, 'dd/MM/yyyy HH:mm') as ThờiGian,
                        B.Status as TTrạng,
                        CAST(CASE WHEN B.Status = 'Maintenance' THEN 0 ELSE ISNULL((DATEDIFF(minute, B.StartTime, B.EndTime) / 60.0) * C.HourlyRate, 0) END AS DECIMAL(18,2)) as Tiền
                    FROM Bookings B
                    JOIN Courts C ON B.CourtID = C.CourtID
                    LEFT JOIN Members M ON B.MemberID = M.MemberID
                    ORDER BY B.StartTime DESC
                ");
                foreach (System.Data.DataRow r in dtActivity.Rows)
                {
                    string amt = Convert.ToDecimal(r["Tiền"]) == 0 ? "0đ" : Convert.ToDecimal(r["Tiền"]).ToString("N0") + "đ";
                    lstBookings.Items.Add(new ListViewItem(new[] { r["Mã"].ToString(), r["Sân"].ToString(), r["Khách"].ToString(), r["ThờiGian"].ToString(), r["TTrạng"].ToString(), amt }));
                }
                if (lstBookings.Items.Count == 0)
                {
                    lstBookings.Items.Add(new ListViewItem(new[] { "-", "-", "Không có hoạt động nào", "-", "-", "0đ" }));
                }
            }
            catch (Exception ex)
            {
                DemoPick.Services.DatabaseHelper.TryLog("Dashboard Load Error", ex, "UCTongQuan.LoadRealDataAsync");
            }
        }
    }
}
