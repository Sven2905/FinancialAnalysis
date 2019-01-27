using FinancialAnalysis.Models.Accounting;

namespace FinancialAnalysis.Models.ProductManagement
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public bool IsStackable { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public double DimensionX { get; set; }
        public double DimensionY { get; set; }
        public double DimensionZ { get; set; }
        public byte[] Picture { get; set; }
        public int RefTaxTypeId { get; set; } = 11;
        public TaxType TaxType { get; set; }
        public int RefProductCategoryId { get; set; } = 1;
        public int PackageUnit { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public decimal DefaultBuyingPrice { get; set; }
        public decimal DefaultSellingPrice { get; set; }
    }
}