// ==========================================================
// File: DashboardController.cs
// Role: Controller (MVC)
// Description: Coordinates data for the main dashboard view.
// ==========================================================
using System.Collections.Generic;
using System.Threading.Tasks;
using DemoPick.Models;
using DemoPick.Services;
using DemoPick.Data;
using DemoPick.Helpers;

namespace DemoPick.Controllers
{
    public class DashboardController
    {
        private readonly DashboardService _dashboardService;

        public DashboardController()
        {
            _dashboardService = new DashboardService();
        }

        public Task<DashboardMetricsModel> GetMetricsAsync()
        {
            return _dashboardService.GetMetricsAsync();
        }

        public Task<List<TrendPointModel>> GetRevenueTrendLast7DaysAsync()
        {
            return _dashboardService.GetRevenueTrendLast7DaysAsync();
        }

        public Task<List<NamedRevenueModel>> GetTopCourtsRevenueAsync()
        {
            return _dashboardService.GetTopCourtsRevenueAsync();
        }

        public Task<List<DashboardActivityModel>> GetRecentActivityAsync(int limit)
        {
            return _dashboardService.GetRecentActivityAsync(limit);
        }


    }
}


