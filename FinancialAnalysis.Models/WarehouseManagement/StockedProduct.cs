using DevExpress.Mvvm;
using FinancialAnalysis.Models.ProductManagement;

namespace FinancialAnalysis.Models.WarehouseManagement
{
    public class StockedProduct : BindableBase
    {
        public StockedProduct()
        {
        }

        public StockedProduct(Product product, int refStockyardId, int quantity)
        {
            RefProductId = product.ProductId;
            Product = product;
            RefStockyardId = refStockyardId;
            Quantity = quantity;
        }

        public int StockedProductId { get; set; }
        public int RefProductId { get; set; }
        public Product Product { get; set; }
        public int RefStockyardId { get; set; }
        public int Quantity { get; set; }
    }
}