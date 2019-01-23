using System.Collections.Generic;
using DevExpress.Mvvm;
using FinancialAnalysis.Models.ProductManagement;

namespace FinancialAnalysis.Models.Warehouse
{
    public class Stockyard : BindableBase
    {
        public int StockyardId { get; set; }
        public string Name { get; set; }
        public int RefWarehouseId { get; set; }
        public List<Product> Products { get; set; }
    }
}