using System;
using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.ProductManagement;

namespace FinancialAnalysis.Models.WarehouseManagement
{
    public class WarehouseStockingHistory : BindableBase
    {
        public WarehouseStockingHistory()
        {

        }

        public WarehouseStockingHistory(Product product, Stockyard stockyard, int quantity, User user)
        {
            if (product != null)
            {
                RefProductId = product.ProductId;
                ProductName = product.Name;
            }
            if (stockyard != null)
            {
                RefStockyardId = stockyard.StockyardId;
                StockyardName = stockyard.Name;
            }
            Quantity = quantity;
            RefUserId = user.UserId;
            UserName = user.Name;
        }

        public int WarehouseStockingHistoryId { get; set; }
        public int Quantity { get; set; }
        public int RefProductId { get; set; }
        public Product Product { get; set; }
        public int RefStockyardId { get; set; }
        public string ProductName { get; set; }
        public Stockyard Stockyard { get; set; }
        public string StockyardName { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int RefUserId { get; set; }
        public string UserName { get; set; }
        public User User { get; set; }
    }
}