using DemoPick.Helpers;
using DemoPick.Data;
// ==========================================================
// File: DashboardModels.cs
// Role: Model (MVC)
// Description: Contains classes used by the Dashboard View 
// for displaying metrics and activities.
// ==========================================================

namespace DemoPick.Models
{
    public class DashboardMetricsModel
    {
        public decimal Revenue { get; set; }
        public int OccupancyPct { get; set; }
        public int CustomerCount { get; set; }
        public int PosCount { get; set; }
    }

    public class DashboardActivityModel
    {
        public string Code { get; set; }
        public string CourtName { get; set; }
        public string CustomerName { get; set; }
        public string TimeText { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
    }
}

