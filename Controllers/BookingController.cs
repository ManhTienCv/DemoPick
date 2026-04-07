using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DemoPick.Models;
using DemoPick.Services;

namespace DemoPick.Controllers
{
    public class BookingController
    {
        public List<CourtModel> GetCourts()
        {
            var list = new List<CourtModel>();
            string query = @"
SELECT CourtID, Name, CourtType, HourlyRate
FROM Courts
WHERE Status = 'Active'
ORDER BY
    CASE
        WHEN CourtType IN (N'Trong nhà', N'Trong Nha', N'Indoor') OR Name LIKE N'%(Trong nhà)%' THEN 1
        WHEN CourtType IN (N'Ngoài trời', N'Ngoai Troi', N'Outdoor') OR Name LIKE N'%(Ngoài trời)%' THEN 2
        WHEN CourtType IN (N'Sân tập', N'San tap', N'Practice', N'Training') OR Name LIKE N'Sân Tập%' OR Name LIKE N'San Tap%' THEN 3
        ELSE 9
    END,
    CASE
        WHEN PATINDEX('%[0-9]%', Name) > 0
        THEN TRY_CONVERT(int,
            SUBSTRING(
                Name,
                PATINDEX('%[0-9]%', Name),
                PATINDEX('%[^0-9]%', SUBSTRING(Name, PATINDEX('%[0-9]%', Name), 100) + 'X') - 1
            )
        )
        ELSE 9999
    END,
    Name,
    CourtID";
            var dt = DatabaseHelper.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new CourtModel
                {
                    CourtID = Convert.ToInt32(row["CourtID"]),
                    Name = row["Name"].ToString(),
                    CourtType = row["CourtType"].ToString(),
                    HourlyRate = Convert.ToDecimal(row["HourlyRate"])
                });
            }
            return list;
        }

        public List<BookingModel> GetBookingsByDate(DateTime date)
        {
            var list = new List<BookingModel>();
            string query = @"
                SELECT BookingID, CourtID, GuestName, StartTime, EndTime, Status 
                FROM Bookings 
                WHERE CAST(StartTime AS DATE) = @Date 
                  AND Status != 'Cancelled'";
                  
            var dt = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@Date", date.Date));
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new BookingModel
                {
                    BookingID = Convert.ToInt32(row["BookingID"]),
                    CourtID = Convert.ToInt32(row["CourtID"]),
                    GuestName = row["GuestName"] != DBNull.Value ? row["GuestName"].ToString() : "Unknown",
                    StartTime = Convert.ToDateTime(row["StartTime"]),
                    EndTime = Convert.ToDateTime(row["EndTime"]),
                    Status = row["Status"].ToString()
                });
            }
            return list;
        }

        public void SubmitBooking(int courtId, string guestName, DateTime startTime, DateTime endTime)
        {
            SubmitBooking(courtId, guestName, startTime, endTime, status: "Confirmed");
        }

        public void SubmitBooking(int courtId, string guestName, DateTime startTime, DateTime endTime, string status)
        {
            using (var conn = DatabaseHelper.GetConnection())
            using (var cmd = new SqlCommand("sp_CreateBooking", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CourtID", courtId);
                cmd.Parameters.AddWithValue("@GuestName", (object)(guestName ?? "") ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@StartTime", startTime);
                cmd.Parameters.AddWithValue("@EndTime", endTime);
                cmd.Parameters.AddWithValue("@Status", string.IsNullOrWhiteSpace(status) ? "Confirmed" : status);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateBookingTime(int bookingId, DateTime newStartTime, DateTime newEndTime)
        {
            if (bookingId <= 0) throw new ArgumentException("BookingID không hợp lệ.");
            if (newEndTime <= newStartTime) throw new ArgumentException("Giờ kết thúc phải sau giờ bắt đầu.");
            if (newStartTime < DateTime.Now) throw new ArgumentException("Giờ bắt đầu đã qua, vui lòng chọn lại.");

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                var dt = DatabaseHelper.ExecuteQuery(
                    "SELECT CourtID, Status FROM dbo.Bookings WHERE BookingID = @Id",
                    new SqlParameter("@Id", bookingId)
                );

                if (dt.Rows.Count == 0)
                    throw new InvalidOperationException("Không tìm thấy booking để đổi ca.");

                int courtId = Convert.ToInt32(dt.Rows[0]["CourtID"]);
                string status = dt.Rows[0]["Status"] == DBNull.Value ? "" : dt.Rows[0]["Status"].ToString();

                if (string.Equals(status, "Cancelled", StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("Booking đã bị hủy.");
                if (string.Equals(status, "Paid", StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("Booking đã thanh toán, không thể đổi ca.");

                object conflictObj = DatabaseHelper.ExecuteScalar(
                    "SELECT COUNT(*) FROM dbo.Bookings WHERE CourtID = @CourtID AND BookingID <> @BookingID AND Status <> 'Cancelled' AND StartTime < @NewEnd AND EndTime > @NewStart",
                    new SqlParameter("@CourtID", courtId),
                    new SqlParameter("@BookingID", bookingId),
                    new SqlParameter("@NewStart", newStartTime),
                    new SqlParameter("@NewEnd", newEndTime)
                );

                int conflicts = conflictObj == null || conflictObj == DBNull.Value ? 0 : Convert.ToInt32(conflictObj);
                if (conflicts > 0)
                    throw new InvalidOperationException("Khung giờ mới bị trùng lịch. Vui lòng chọn ca khác.");

                DatabaseHelper.ExecuteNonQuery(
                    "UPDATE dbo.Bookings SET StartTime = @Start, EndTime = @End WHERE BookingID = @Id AND Status <> 'Cancelled'",
                    new SqlParameter("@Start", newStartTime),
                    new SqlParameter("@End", newEndTime),
                    new SqlParameter("@Id", bookingId)
                );
            }
        }
    }
}