using DemoPick.Helpers;
using DemoPick.Data;
// ==========================================================
// File: CustomerModels.cs
// Role: Model (MVC)
// Description: Contains classes representing customer data, 
// tiers, revenues, and checkout information.
// ==========================================================
using System;

namespace DemoPick.Models
{
    public class CustomerModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string CustomerType { get; set; }
        public decimal TotalHours { get; set; }
        public string TotalSpent { get; set; }
        public string Tier { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CustomerTierCountsModel
    {
        public int FixedCount { get; set; }
        public int WalkinCount { get; set; }
    }

    public class CustomerRevenueSummaryModel
    {
        public int MemberCount { get; set; }
        public decimal Revenue { get; set; }
    }

    public class MembershipSummaryModel
    {
        public int BasicCount { get; set; }
        public int SilverCount { get; set; }
        public int GoldCount { get; set; }
        public int NearSilverCount { get; set; }
        public int NearGoldCount { get; set; }
    }

    public class CheckoutCustomerModel
    {
        public int MemberId { get; set; }
        public string FullName { get; set; }
        public string Tier { get; set; }
        public bool IsFixed { get; set; }
    }
}

