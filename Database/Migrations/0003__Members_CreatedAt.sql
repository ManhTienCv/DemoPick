/*
0003__Members_CreatedAt.sql

Mục đích:
- Bổ sung cột CreatedAt cho Members để UI/CRM sắp xếp ổn định (newest first).
- Idempotent: chỉ thêm nếu chưa tồn tại.
*/

IF COL_LENGTH('dbo.Members', 'CreatedAt') IS NULL
BEGIN
    ALTER TABLE dbo.Members
    ADD CreatedAt DATETIME NOT NULL
        CONSTRAINT DF_Members_CreatedAt DEFAULT(GETDATE());
END
GO
