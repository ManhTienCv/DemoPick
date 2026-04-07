/*
0002__StaffAccounts_Lockout.sql

Mục đích:
- Thêm cơ chế lockout/backoff cơ bản cho đăng nhập StaffAccounts.
- Tránh brute-force online đơn giản.

Lưu ý:
- Script idempotent: chỉ thêm cột nếu chưa tồn tại.
*/

IF COL_LENGTH('dbo.StaffAccounts', 'FailedLoginCount') IS NULL
BEGIN
    ALTER TABLE dbo.StaffAccounts
    ADD FailedLoginCount INT NOT NULL
        CONSTRAINT DF_StaffAccounts_FailedLoginCount DEFAULT(0);
END
GO

IF COL_LENGTH('dbo.StaffAccounts', 'LockoutUntil') IS NULL
BEGIN
    ALTER TABLE dbo.StaffAccounts
    ADD LockoutUntil DATETIME NULL;
END
GO

IF COL_LENGTH('dbo.StaffAccounts', 'LastFailedLoginAt') IS NULL
BEGIN
    ALTER TABLE dbo.StaffAccounts
    ADD LastFailedLoginAt DATETIME NULL;
END
GO
