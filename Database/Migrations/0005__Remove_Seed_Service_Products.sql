/*
Remove legacy service-rental products that were seeded from base SQL.
Only delete products that have never been used in InvoiceDetails to keep sales history safe.
*/

DELETE p
FROM dbo.Products p
WHERE p.SKU IN (N'SVC-RACKET', N'SVC-BALL-BASKET', N'SVC-BALL-MACHINE', N'SVC-BALL-PICK')
  AND NOT EXISTS (
      SELECT 1
      FROM dbo.InvoiceDetails d
      WHERE d.ProductID = p.ProductID
  );
GO
