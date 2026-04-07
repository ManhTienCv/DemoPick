using System;
using System.Collections.Generic;

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
        public DateTime CreatedAt { get; set; }
    }

    public class InventoryItemModel
    {
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Stock { get; set; }
        public string Status { get; set; }
        public string Price { get; set; }
    }

    public class TransactionModel
    {
        public string EventDesc { get; set; }
        public string SubDesc { get; set; }
        public string Time { get; set; }
    }

    public class TopCourtModel
    {
        public string CourtId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Occupancy { get; set; }
        public string Revenue { get; set; }
    }
    public class CourtModel
    {
        public int CourtID { get; set; }
        public string Name { get; set; }
        public string CourtType { get; set; }
        public decimal HourlyRate { get; set; }
    }

    public class BookingModel
    {
        public int BookingID { get; set; }
        public int CourtID { get; set; }
        public string GuestName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } 
    }
}
