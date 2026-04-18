/*
Ensure default rental-service products always exist for POS catalog.
Idempotent: update if SKU exists, insert if missing.
*/

MERGE INTO dbo.Products AS Target
USING (VALUES
    (N'DV_THUE_VOT', N'Thuê vợt', N'Dịch vụ', CAST(40000 AS DECIMAL(18,2)), 9999, 0),
    (N'DV_BONG_RO', N'Bóng tập (rổ)', N'Dịch vụ', CAST(40000 AS DECIMAL(18,2)), 9999, 0),
    (N'DV_MAY_BAN', N'Máy bắn bóng', N'Dịch vụ', CAST(80000 AS DECIMAL(18,2)), 9999, 0),
    (N'DV_NHAT_BONG', N'Nhặt bóng', N'Dịch vụ', CAST(40000 AS DECIMAL(18,2)), 9999, 0)
) AS Source (SKU, Name, Category, Price, StockQuantity, MinThreshold)
ON Target.SKU = Source.SKU
WHEN MATCHED THEN
    UPDATE SET
        Target.Name = Source.Name,
        Target.Category = Source.Category,
        Target.Price = Source.Price,
        Target.StockQuantity = CASE
            WHEN Target.StockQuantity < Source.StockQuantity THEN Source.StockQuantity
            ELSE Target.StockQuantity
        END,
        Target.MinThreshold = Source.MinThreshold
WHEN NOT MATCHED THEN
    INSERT (SKU, Name, Category, Price, StockQuantity, MinThreshold)
    VALUES (Source.SKU, Source.Name, Source.Category, Source.Price, Source.StockQuantity, Source.MinThreshold);
GO
