using DevExpress.Mvvm;
using FinancialAnalysis.Models.ProductManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.WarehouseManagement
{
    public class StockedProduct : BindableBase
    {
        public int StockedProductId { get; set; }
        public int RefProductId { get; set; }
        public Product Product { get; set; }
        public int RefStockyardId { get; set; }
        public int Quantity { get; set; }
    }
}
