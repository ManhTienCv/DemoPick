// ==========================================================
// File: CustomerController.cs
// Role: Controller (MVC)
// Description: Manages customer-related data flows between 
// the Views and the CustomerService.
// ==========================================================
using System.Collections.Generic;
using System.Threading.Tasks;
using DemoPick.Models;
using DemoPick.Services;
using DemoPick.Data;
using DemoPick.Helpers;

namespace DemoPick.Controllers
{
    public class CustomerController
    {
        private readonly CustomerService _customerService;

        public CustomerController()
        {
            _customerService = new CustomerService();
        }

        public Task<List<CustomerModel>> GetAllCustomersAsync()
        {
            return _customerService.GetAllCustomersAsync();
        }

        public Task<CustomerTierCountsModel> GetTierCountsAsync()
        {
            return _customerService.GetTierCountsAsync();
        }

        public Task<CustomerRevenueSummaryModel> GetRevenueSummaryAsync()
        {
            return _customerService.GetRevenueSummaryAsync();
        }

        public Task<int> GetTodayOccupancyPctAsync()
        {
            return _customerService.GetTodayOccupancyPctAsync();
        }

        public Task<MembershipSummaryModel> GetMembershipSummaryAsync()
        {
            return _customerService.GetMembershipSummaryAsync();
        }
    }
}


