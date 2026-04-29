// ==========================================================
// File: InventoryController.cs
// Role: Controller (MVC)
// Description: Manages inventory data operations, providing 
// KPI metrics and item lists to the UI.
// ==========================================================
using System.Collections.Generic;
using System.Threading.Tasks;
using DemoPick.Models;
using DemoPick.Services;
using DemoPick.Data;
using DemoPick.Helpers;

namespace DemoPick.Controllers
{
    public class InventoryController
    {
        private readonly InventoryService _inventoryService;

        public InventoryController()
        {
            _inventoryService = new InventoryService();
        }

        public Task<InventoryKpiModel> GetInventoryKpisAsync()
        {
            return _inventoryService.GetInventoryKpisAsync();
        }



        public Task<List<InventoryItemModel>> GetInventoryItemsAsync()
        {
            return _inventoryService.GetInventoryItemsAsync();
        }

        public Task<List<TransactionModel>> GetRecentTransactionsAsync()
        {
            return _inventoryService.GetRecentTransactionsAsync();
        }

        public InventorySmartInsightsModel BuildSmartInsights(IReadOnlyCollection<InventoryItemModel> items)
        {
            return _inventoryService.BuildSmartInsights(items);
        }
    }
}


