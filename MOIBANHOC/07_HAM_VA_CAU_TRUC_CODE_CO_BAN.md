# 07 - Các hàm và cấu trúc code cơ bản trong dự án (để học)

## 1) Mục tiêu
File này trích các mẫu code cơ bản đang được dùng thật trong DemoPick để bạn học:
- Khai báo class/OOP
- Hàm (method)
- Vòng lặp `for`, `foreach`, `while`
- `if/else`, `switch/case` (dạng tương đương)
- `try/catch`
- `using` (quản lý tài nguyên)
- `async/await`
- Collection (`List`, `Dictionary`)

Mỗi mục đều có ví dụ thật từ dự án.

---

## 2) OOP cơ bản: Class và Object
Ví dụ class trong `Services/PriceCalculator.cs`:
```csharp
public class ServiceCharge
{
    public int ProductID { get; set; }
    public string ServiceName { get; set; }
    public string Unit { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Total { get; set; }
}
```

Ý nghĩa:
- `class` mô tả một kiểu dữ liệu có nhiều thuộc tính liên quan.
- Mỗi object `ServiceCharge` đại diện cho 1 dòng dịch vụ trong tính tiền.

---

## 3) Hàm (method) và tham số
Ví dụ hàm có tham số + trả về trong `Services/PriceCalculator.cs`:
```csharp
public static decimal GetCourtRateMultiplier(string courtType, string courtName)
{
    string t = (courtType ?? string.Empty).Trim();
    string n = (courtName ?? string.Empty).Trim();

    if (Contains(t, "tập") || Contains(t, "practice") || Contains(n, "tập"))
        return 0.5m;

    return 1m;
}
```

Bạn học được:
- Hàm nhận input (`courtType`, `courtName`), xử lý và trả output (`decimal`).
- Dùng `return` để trả kết quả.

---

## 4) Method overload (nạp chồng hàm)
Trong `Controllers/BookingController.cs` có nhiều phiên bản `SubmitBooking`:
```csharp
public void SubmitBooking(int courtId, string guestName, DateTime startTime, DateTime endTime)
public void SubmitBooking(int courtId, string guestName, DateTime startTime, DateTime endTime, string status)
public void SubmitBooking(int courtId, string guestName, string note, DateTime startTime, DateTime endTime, string status)
public void SubmitBooking(int courtId, int? memberId, string guestName, string note, DateTime startTime, DateTime endTime, string status)
```

Ý nghĩa:
- Cùng tên hàm, khác tham số.
- Cho phép gọi linh hoạt theo nhu cầu.

---

## 5) Vòng lặp `foreach`
Ví dụ trong `Services/ReportService.cs`:
```csharp
foreach (DataRow r in dtTrend.Rows)
{
    list.Add(new TrendPointModel
    {
        Label = r["Label"].ToString(),
        Revenue = r["Revenue"] == DBNull.Value ? 0m : Convert.ToDecimal(r["Revenue"])
    });
}
```

Khi nào dùng:
- Duyệt từng phần tử trong tập hợp (DataRow, List, array).

---

## 6) Vòng lặp `for`
Ví dụ trong `Services/PosService.cs`:
```csharp
for (int i = 0; i < lines.Count; i++)
{
    var line = lines[i];
    if (line == null) continue;
    if (line.ProductId <= 0) continue;
    if (line.Quantity <= 0) continue;
    // xử lý
}
```

Khi nào dùng:
- Khi cần index `i` để truy cập phần tử theo vị trí.

---

## 7) Vòng lặp `while`
Ví dụ trong `Services/PriceCalculator.cs`:
```csharp
DateTime current = start;
while (current < end)
{
    DateTime nextMidnight = current.Date.AddDays(1);
    DateTime blockEnd = end < nextMidnight ? end : nextMidnight;
    // xử lý theo block giờ
    current = blockEnd;
}
```

Khi nào dùng:
- Khi chưa biết trước số lần lặp, chỉ biết điều kiện dừng.

---

## 8) Điều kiện `if/else`
Ví dụ trong `Services/AuthService.cs`:
```csharp
if (string.IsNullOrWhiteSpace(identifier) || string.IsNullOrWhiteSpace(password))
{
    error = "Vui lòng nhập tài khoản và mật khẩu.";
    return false;
}
```

Ý nghĩa:
- Kiểm tra điều kiện trước khi xử lý nghiệp vụ chính.

---

## 9) Toán tử điều kiện rút gọn (ternary)
Ví dụ trong `Services/InvoiceService.cs`:
```csharp
MemberID = r["MemberID"] == DBNull.Value ? (int?)null : Convert.ToInt32(r["MemberID"]),
```

Ý nghĩa:
- Dùng `condition ? valueIfTrue : valueIfFalse` để viết ngắn gọn.

---

## 10) `try/catch` bắt lỗi
Ví dụ trong `Services/DatabaseHelper.cs`:
```csharp
public static void TryLog(string eventDesc, string subDesc)
{
    try
    {
        ExecuteNonQuery(
            "INSERT INTO SystemLogs (EventDesc, SubDesc) VALUES (@EventDesc, @SubDesc)",
            new SqlParameter("@EventDesc", eventDesc ?? ""),
            new SqlParameter("@SubDesc", (object)subDesc ?? DBNull.Value)
        );
    }
    catch
    {
        // không để lỗi log làm crash app
    }
}
```

Ý nghĩa:
- Tránh crash app với các tác vụ phụ như logging.

---

## 11) `using` để tự động giải phóng tài nguyên
Ví dụ trong `Services/DatabaseHelper.cs`:
```csharp
using (var conn = GetConnection())
using (var cmd = new SqlCommand(query, conn))
{
    conn.Open();
    return cmd.ExecuteScalar();
}
```

Ý nghĩa:
- `SqlConnection` và `SqlCommand` được dispose đúng lúc.
- Hạn chế leak kết nối DB.

---

## 12) `async/await` cơ bản
Ví dụ trong `Services/CustomerService.cs`:
```csharp
public async Task<List<CustomerModel>> GetAllCustomersAsync()
{
    var list = new List<CustomerModel>();
    await Task.Run(() => {
        var dt = DatabaseHelper.ExecuteQuery(query);
        // map dữ liệu
    });
    return list;
}
```

Ý nghĩa:
- Tránh block UI khi xử lý tác vụ tốn thời gian.

---

## 13) Collection: `Dictionary` và `List`
Ví dụ trong `Services/PosService.cs`:
```csharp
public static Dictionary<string, List<CartLine>> PendingOrders { get; }
    = new Dictionary<string, List<CartLine>>(StringComparer.OrdinalIgnoreCase);
```

Ý nghĩa:
- `Dictionary`: tra cứu nhanh theo key (`courtName`).
- `List`: lưu danh sách nhiều phần tử cùng kiểu.

---

## 14) Khai báo lớp lồng nhau và `sealed`
Ví dụ trong `Services/InvoiceService.cs`:
```csharp
public sealed class InvoiceHeader
{
    public int InvoiceID { get; set; }
    public DateTime CreatedAt { get; set; }
    // ...
}
```

Ý nghĩa:
- `sealed`: không cho class bị kế thừa.
- Dùng tốt cho DTO/model truyền dữ liệu.

---

## 15) Mẹo học nhanh từ chính dự án này
1. Đọc `PriceCalculator` để học vòng lặp + chia logic.
2. Đọc `BookingController` để học method overload + validate nghiệp vụ.
3. Đọc `DatabaseHelper` để học `using`, query SQL, handling lỗi.
4. Đọc `PosService.Checkout` để học transaction thực chiến.
5. Đọc `ReportService` để học map `DataTable -> Model`.

---

## 16) Kết luận
Bạn có thể coi DemoPick như một bộ ví dụ thực tế để học C# cơ bản đến trung cấp:
- không phải ví dụ sách giáo khoa đơn lẻ,
- mà là code chạy thật với bài toán thật.

Hãy thử tự code lại một vài hàm tương tự để nhớ lâu hơn (đặc biệt là `CalculateTotal` và `Checkout`).
