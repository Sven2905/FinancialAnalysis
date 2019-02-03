using DevExpress.Mvvm;

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