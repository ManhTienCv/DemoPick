using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DemoPick.Models;

namespace DemoPick.Services
{
    public class ReportService
    {
        public async Task<List<TopCourtModel>> GetTopCourtsAsync()
        {
            // Backward-compatible: default to all time.
            return await GetTopCourtsAsync(null, null);
        }

        public async Task<List<TopCourtModel>> GetTopCourtsAsync(DateTime? fromDateInclusive, DateTime? toDateExclusive)
        {
            var list = new List<TopCourtModel>();
            string query = @"
                SELECT 
                    c.Name as CourtName, 
                    c.CourtType as CourtType,
                    ISNULL(SUM(DATEDIFF(minute, b.StartTime, b.EndTime)), 0) as BookedMinutes,
                    ISNULL(SUM(DATEDIFF(minute, b.StartTime, b.EndTime) / 60.0 * c.HourlyRate), 0) as Revenue
                FROM Courts c
                LEFT JOIN Bookings b 
                    ON c.CourtID = b.CourtID
                    AND b.Status != 'Cancelled'
                    AND b.Status != 'Maintenance'
                    AND (@From IS NULL OR b.StartTime >= @From)
                    AND (@To IS NULL OR b.StartTime < @To)
                GROUP BY c.CourtID, c.Name, c.CourtType
                ORDER BY Revenue DESC";

            await Task.Run(() => {
                var dt = DatabaseHelper.ExecuteQuery(
                    query,
                    new System.Data.SqlClient.SqlParameter("@From", (object)fromDateInclusive ?? DBNull.Value),
                    new System.Data.SqlClient.SqlParameter("@To", (object)toDateExclusive ?? DBNull.Value)
                );
                
                // Find max booked minutes for relative scaling up to 100%
                decimal maxMins = 0;
                foreach (DataRow row in dt.Rows)
                {
                    decimal mins = Convert.ToDecimal(row["BookedMinutes"]);
                    if (mins > maxMins) maxMins = mins;
                }

                int rank = 1;
                foreach (DataRow row in dt.Rows)
                {
                    decimal rev = Convert.ToDecimal(row["Revenue"]);
                    decimal mins = Convert.ToDecimal(row["BookedMinutes"]);
                    string name = row["CourtName"].ToString();
                    
                    // Allow fallback to generic string if Type column isn't properly returned by some schemas
                    string type = dt.Columns.Contains("CourtType") ? row["CourtType"].ToString() : "Sân Pickleball";

                    // Relative occupancy: 0 to 100% logic
                    int occPct = maxMins > 0 ? (int)Math.Round((mins / maxMins) * 100) : 0;
                    int charCount = (int)Math.Round((occPct / 100.0) * 20); // Scale to 20 "="
                    if (charCount == 0 && occPct > 0) charCount = 1; // at least 1 bar if there's >0 mins
                    
                    string visualBar = occPct + "%  " + new String('=', charCount);

                    list.Add(new TopCourtModel
                    {
                        CourtId = "T" + rank,
                        Name = name,
                        Type = type,
                        Occupancy = visualBar,
                        Revenue = rev == 0 ? "0đ" : rev.ToString("N0") + "đ"
                    });
                    rank++;
                }
            });
            return list;
        }
    }
}
