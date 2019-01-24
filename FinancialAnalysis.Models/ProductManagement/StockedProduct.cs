using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.ProductManagement
{
    public class StockedProduct : BindableBase
    {
        public int StockedProductId { get; set; }
        public int RefProductId { get; set; }
        public int RefStockyardId { get; set; }
        public int Quantity { get; set; }
    }
}
