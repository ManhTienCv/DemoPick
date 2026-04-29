using DemoPick.Helpers;
using DemoPick.Data;
// ==========================================================
// File: InventoryModels.cs
// Role: Model (MVC)
// Description: Contains classes representing products, 
// inventory metrics, and catalog items.
// ==========================================================
using System.Collections.Generic;

namespace DemoPick.Models
{
    public class InventoryItemModel
    {
        public int ProductId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Stock { get; set; }
        public string Status { get; set; }
        public string Price { get; set; }
        public int CurrentStock { get; set; }
        public int MinThreshold { get; set; }
        public int SoldLast14Days { get; set; }
        public decimal AvgDailySales { get; set; }
        public int SuggestedReorderQuantity { get; set; }
        public int TargetStockQuantity { get; set; }
        public int? DaysRemaining { get; set; }
        public string Recommendation { get; set; }
    }

    public class InventoryKpiModel
    {
        public decimal TotalValue { get; set; }
        public int CriticalItems { get; set; }
        public int Sales { get; set; }
        public int InvoicesCount { get; set; }
    }

    public class InventoryForecastPointModel
    {
        public string Label { get; set; }
        public int RiskItems { get; set; }
    }

    public class InventorySmartInsightsModel
    {
        public int OutOfStockCount { get; set; }
        public int WarningItemsCount { get; set; }
        public int SuggestedReorderProductCount { get; set; }
        public int SuggestedReorderUnits { get; set; }
        public int SoldLast14Days { get; set; }
        public decimal AvgDailySales { get; set; }
        public List<InventoryForecastPointModel> ForecastPoints { get; set; } = new List<InventoryForecastPointModel>();
    }

    public class ProductCatalogItemModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }

    public class ProductDeleteListItemModel
    {
        public int ProductId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}

