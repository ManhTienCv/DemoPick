# DemoPick Smoke Test Report

- Started: 2026-04-18 12:40:59
- Duration: 2,18s
- Machine: MANHTIEN
- User: Admin
- App: DemoPick.exe

## Login Used

- Identifier: smoke_ec9e220f1ee3416cadce7f0ad6d39858@local
- Source: temp-user

## Steps

| Step | Result | Duration | Details |
|---|---|---:|---|
| DB init (schema + migrations) | SUCCESS | 16ms | OK |
| Obtain test credentials | SUCCESS | 841ms | Registered temp user: smoke_ec9e220f1ee3416cadce7f0ad6d39858@local |
| Login | SUCCESS | 820ms | Signed in as smoke_ec9e220f1ee3416cadce7f0ad6d39858@local (Staff) |
| Load courts | SUCCESS | 13ms | Courts: 19 |
| Create + cleanup test booking | SUCCESS | 51ms | Created booking on CourtID=1 2026-04-21 06:00 (90m). Removed=True |
| Logout | SUCCESS | 0ms | OK |
| Cleanup temp account | SUCCESS | 4ms | Deleted rows: 1 |
| Cleanup legacy SMOKE artifacts | SUCCESS | 24ms | Bookings=0, Members=0, StaffAccounts=0 |
| Logic tests (PriceCalculator + PendingOrders) | SUCCESS | 123ms | All logic assertions passed |
| UI refactor tests (UCThanhToan + FrmXoaSP) | SUCCESS | 250ms | ReprintPanel=OK; PaymentHistoryPanel=OK; UCThanhToanEmbed=OK; FrmXoaSPDesigner=OK |
| Performance tests (micro-benchmark) | SUCCESS | 38ms | PriceCalc: 20000 loops in 21ms (922714 ops/s, min 861679), PendingOrders: 20000 loops in 6ms (3219886 ops/s, min 2374874), checksum=6.900.000.000, report=D:\vstudio\BTL\DemoPick\DemoPick\Docs\Perf\PER… |

## Failures

No failures.

