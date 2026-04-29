using DemoPick.Helpers;
using DemoPick.Data;
// ==========================================================
// File: BookingModels.cs
// Role: Model (MVC)
// Description: Contains classes for representing bookings, 
// courts, and transactions.
// ==========================================================
using System;

namespace DemoPick.Models
{
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
        public string Note { get; set; }
        public string PaymentState { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } 
    }

    public class TopCourtModel
    {
        public string CourtId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Occupancy { get; set; }
        public string Revenue { get; set; }
        public string PeakSlot { get; set; }
        public string CancelRate { get; set; }
    }

    public class TransactionModel
    {
        public string EventDesc { get; set; }
        public string SubDesc { get; set; }
        public string Time { get; set; }
    }
}

