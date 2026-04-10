# DemoPick Smoke Test Report

- Started: 2026-04-10 22:28:44
- Duration: 2,66s
- Machine: MANHTIEN
- User: Admin
- App: DemoPick.exe

## Login Used

- Identifier: smoke_d7e818a589954db3b69cb62f920a9af2@local
- Source: temp-user

## Steps

| Step | Result | Duration | Details |
|---|---|---:|---|
| DB init (schema + migrations) | SUCCESS | 27ms | OK |
| Obtain test credentials | SUCCESS | 982ms | Registered temp user: smoke_d7e818a589954db3b69cb62f920a9af2@local |
| Login | SUCCESS | 1173ms | Signed in as smoke_d7e818a589954db3b69cb62f920a9af2@local (Staff) |
| Load courts | SUCCESS | 18ms | Courts: 19 |
| Create + cleanup test booking | SUCCESS | 134ms | Created booking on CourtID=1 2026-04-13 06:00 (90m). Removed=True |
| Logout | SUCCESS | 0ms | OK |
| Cleanup temp account | SUCCESS | 8ms | Deleted rows: 1 |
| Cleanup legacy SMOKE artifacts | SUCCESS | 33ms | Bookings=0, Members=0, StaffAccounts=0 |
| Logic tests (PriceCalculator + PendingOrders) | SUCCESS | 229ms | All logic assertions passed |
| Performance tests (micro-benchmark) | FAIL | 53ms | Performance regression: PriceCalculator 720570 ops/s < baseline 861679 ops/s. See D:\vstudio\BTL\DemoPick\DemoPick\Docs\Perf\PERF_LAST_RUN.md |

## Failures

### Performance tests (micro-benchmark)

```text
System.InvalidOperationException: Performance regression: PriceCalculator 720570 ops/s < baseline 861679 ops/s. See D:\vstudio\BTL\DemoPick\DemoPick\Docs\Perf\PERF_LAST_RUN.md
   at DemoPick.Services.SmokeTestRunner.RunPerformanceTests() in D:\vstudio\BTL\DemoPick\DemoPick\Services\SmokeTestRunner.cs:line 464
   at DemoPick.Services.SmokeTestRunner.<>c.<Run>b__3_9() in D:\vstudio\BTL\DemoPick\DemoPick\Services\SmokeTestRunner.cs:line 214
   at DemoPick.Services.SmokeTestRunner.RunStep(String name, Func`1 action) in D:\vstudio\BTL\DemoPick\DemoPick\Services\SmokeTestRunner.cs:line 249
```

