using DevExpress.Mvvm;
using FinancialAnalysis.Models.ProductManagement;

namespace FinancialAnalysis.Models.BaseClasses
{
    public class OrderPositionItem : BindableBase, IOrderPositionItem
    {
        protected decimal _DiscountPercentage = 0;

        public decimal DiscountPercentage
        {
            get { return _DiscountPercentage / 100; }
            set { _DiscountPercentage = value; }
        }

        public int RefProductId { get; set; }
        public Product Product { get; set; }
        public decimal Quantity { get; set; } = 1;
        public decimal Price { get; set; }
        public string Description { get; set; }
        public GrossNetType GrossNetType { get; set; } = GrossNetType.Netto;
        public decimal Subtotal { get => SubtotalWithoutDiscount - DiscountAmount; } // w/o Tax
        public decimal Total { get => Subtotal + TaxAmount; }
        public decimal DiscountAmount { get => SubtotalWithoutDiscount * DiscountPercentage; }
        public decimal SubtotalWithoutDiscount { get => PriceWithoutTax() * Quantity; }
        public decimal TaxAmount { get => Subtotal * (Product.TaxType.AmountOfTax / 100); }
        public bool IsCanceled { get; set; }

        protected virtual decimal PriceWithoutTax()
        {
            if (GrossNetType == GrossNetType.Brutto)
                return ((Price / (100 + Product.TaxType.AmountOfTax)) * 100);
            else
                return Price;
        }
    }
}
