namespace FinancialAnalysis.Models.Product
{
    public class ProductPrototype
    {
        public int ProductPrototypeId { get; set; }
        public string Name { get; set; }
        public bool IsStackable { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public double DimensionX { get; set; }
        public double DimensionY { get; set; }
        public double DimensionZ { get; set; }
        public byte[] Picture { get; set; }
        public int RefProductCategoryId { get; set; } = 1;
        public int PackageUnit { get; set; }
        public decimal BuyingPrice { get; set; }
        public decimal SalePrice { get; set; }
        public ProductCategory ProductCategory { get; set; }
    }
}