using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.SalesManagement
{
    public class ShippedProduct : BindableBase
    {
        public int ShippedProductId { get; set; }
        public int RefShipmentId { get; set; }
        public int RefSalesOrderPositionId { get; set; }
        public int Quantity { get; set; }
    }
}
