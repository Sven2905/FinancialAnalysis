using System.Collections.Generic;
using DevExpress.Mvvm;
using FinancialAnalysis.Models.ProductManagement;

namespace FinancialAnalysis.Models.WarehouseManagement
{
    public class Stockyard : BindableBase
    {
        public int StockyardId { get; set; }
        public string Name { get; set; }
        public int RefWarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        public List<StockedProduct> StockedProducts { get; set; }
    }
}