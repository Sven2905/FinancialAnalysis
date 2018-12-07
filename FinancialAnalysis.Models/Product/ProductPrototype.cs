namespace FinancialAnalysis.Models.Product
{
    public class ProductPrototype
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public bool IsStackable { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public double DimensionX { get; set; }
        public double DimensionY { get; set; }
        public double DimensionZ { get; set; }
        public int RefProductCategory { get; set; } = 1;
        public ProductCategory ProductCategory { get; set; }
    }
}