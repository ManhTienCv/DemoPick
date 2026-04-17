# DemoPick Smoke Test Report

- Started: 2026-04-18 01:15:47
- Duration: 2,03s
- Machine: MANHTIEN
- User: Admin
- App: DemoPick.exe

## Login Used

- Identifier: smoke_507cd16a87954d3aabf34f74972d693f@local
- Source: temp-user

## Steps

| Step | Result | Duration | Details |
|---|---|---:|---|
| DB init (schema + migrations) | SUCCESS | 25ms | OK |
| Obtain test credentials | SUCCESS | 795ms | Registered temp user: smoke_507cd16a87954d3aabf34f74972d693f@local |
| Login | SUCCESS | 721ms | Signed in as smoke_507cd16a87954d3aabf34f74972d693f@local (Staff) |
| Load courts | SUCCESS | 8ms | Courts: 19 |
| Create + cleanup test booking | SUCCESS | 73ms | Created booking on CourtID=1 2026-04-21 06:00 (90m). Removed=True |
| Logout | SUCCESS | 0ms | OK |
| Cleanup temp account | SUCCESS | 5ms | Deleted rows: 1 |
| Cleanup legacy SMOKE artifacts | SUCCESS | 27ms | Bookings=0, Members=0, StaffAccounts=0 |
| Logic tests (PriceCalculator + PendingOrders) | SUCCESS | 156ms | All logic assertions passed |
| UI refactor tests (UCThanhToan + FrmXoaSP) | SUCCESS | 198ms | ReprintPanel=OK; PaymentHistoryPanel=OK; UCThanhToanEmbed=OK; FrmXoaSPDesigner=OK |
| Performance tests (micro-benchmark) | SUCCESS | 26ms | PriceCalc: 20000 loops in 12ms (1605716 ops/s, min 861679), PendingOrders: 20000 loops in 6ms (2931305 ops/s, min 2374874), checksum=6.900.000.000, report=D:\vstudio\BTL\DemoPick\DemoPick\Docs\Perf\PE… |

## Failures

No failures.

