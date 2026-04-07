/*
0001__Create_MigrationProof.sql

Mục đích:
- Migration mẫu để chứng minh flow apply + checksum.
- Tạo 1 bảng nhỏ và insert 1 dòng đánh dấu migration đã chạy.

Lưu ý:
- Migrations chỉ chạy 1 lần (được track trong dbo.__Migrations).
- Không nên sửa nội dung migration sau khi đã apply; hãy tạo file migration mới.
*/

IF OBJECT_ID('dbo.MigrationProof','U') IS NULL
BEGIN
    CREATE TABLE dbo.MigrationProof (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_MigrationProof PRIMARY KEY,
        Note NVARCHAR(200) NOT NULL,
        CreatedAt DATETIME NOT NULL CONSTRAINT DF_MigrationProof_CreatedAt DEFAULT(GETDATE())
    );
END
GO

INSERT INTO dbo.MigrationProof (Note)
VALUES (N'Migration 0001 applied');
GO
