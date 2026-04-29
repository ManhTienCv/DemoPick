// ==========================================================
// File: ReportController.cs
// Role: Controller (MVC)
// Description: Handles report generation, metrics, and KPI 
// formatting for the Report views.
// ==========================================================
using System.Collections.Generic;
using System.Threading.Tasks;
using DemoPick.Models;
using DemoPick.Services;
using DemoPick.Data;
using DemoPick.Helpers;

namespace DemoPick.Controllers
{
    public class ReportController
    {
        private readonly ReportService _reportService;

        public ReportController()
        {
            _reportService = new ReportService();
        }

        public Task<ReportKpiModel> GetKpisAsync(System.DateTime from, System.DateTime to, int days)
        {
            return _reportService.GetKpisAsync(from, to, days);
        }

        public Task<List<ReportHeatmapPointModel>> GetBookingHourHeatmapAsync(System.DateTime from, System.DateTime to)
        {
            return _reportService.GetBookingHourHeatmapAsync(from, to);
        }

        public Task<ReportBookingOpsModel> GetBookingOpsAsync(System.DateTime from, System.DateTime to)
        {
            return _reportService.GetBookingOpsAsync(from, to);
        }

        public Task<List<TopCourtModel>> GetTopCourtsAsync(System.DateTime from, System.DateTime to)
        {
            return _reportService.GetTopCourtsAsync(from, to);
        }

        public Task<List<NamedRevenueModel>> GetTopCourtsRevenueAsync(System.DateTime from, System.DateTime to)
        {
            return _reportService.GetTopCourtsRevenueAsync(from, to);
        }
    }
}


