using DemoPick.Helpers;
using DemoPick.Data;
// ==========================================================
// File: ReportModels.cs
// Role: Model (MVC)
// Description: Contains classes for representing data in 
// reports and analytics.
// ==========================================================

namespace DemoPick.Models
{
    public class TrendPointModel
    {
        public string Label { get; set; }
        public decimal Revenue { get; set; }
    }

    public class NamedRevenueModel
    {
        public string Name { get; set; }
        public decimal Revenue { get; set; }
    }

    public class ReportKpiModel
    {
        public decimal CurrRev { get; set; }
        public decimal PrevRev { get; set; }
        public decimal CurrOcc { get; set; }
        public decimal PrevOcc { get; set; }
        public int CurrNewCust { get; set; }
        public int PrevNewCust { get; set; }
    }

    public class ReportHeatmapPointModel
    {
        public int Hour { get; set; }
        public string Label { get; set; }
        public int BookingCount { get; set; }
    }

    public class ReportBookingOpsModel
    {
        public int TotalBookings { get; set; }
        public int CancelledBookings { get; set; }
        public int ShiftedBookings { get; set; }
        public int ActiveBookings { get; set; }
    }
}

