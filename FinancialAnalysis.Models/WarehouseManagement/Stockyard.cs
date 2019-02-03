using DevExpress.Mvvm;
using System.Collections.Generic;

namespace FinancialAnalysis.Models.WarehouseManagement
{
    public class Stockyard : BindableBase
    {
        public int StockyardId { get; set; }
        public string Name { get; set; }
        public int RefWarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        public bool IsEmpty => StockedProducts.Count == 0;
        public List<StockedProduct> StockedProducts { get; set; } = new List<StockedProduct>();
    }
}