using DevExpress.Mvvm;

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