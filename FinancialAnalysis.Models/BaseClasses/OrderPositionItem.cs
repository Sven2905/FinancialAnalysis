using DevExpress.Mvvm;
using FinancialAnalysis.Models.ProductManagement;

namespace FinancialAnalysis.Models.BaseClasses
{
    public class OrderPositionItem : BindableBase, IOrderPositionItem
    {
        protected decimal _DiscountPercentage;

        public decimal DiscountPercentage
        {
            get => _DiscountPercentage / 100;
            set => _DiscountPercentage = value;
        }

        public int RefProductId { get; set; }
        public Product Product { get; set; }
        public decimal Quantity { get; set; } = 1;
        public decimal Price { get; set; }
        public string Description { get; set; }
        public GrossNetType GrossNetType { get; set; } = GrossNetType.Netto;
        public decimal Subtotal // w/o Tax
            => SubtotalWithoutDiscount - DiscountAmount;

        public decimal Total => Subtotal + TaxAmount;
        public decimal DiscountAmount => SubtotalWithoutDiscount * DiscountPercentage;
        public decimal SubtotalWithoutDiscount => PriceWithoutTax() * Quantity;
        public decimal TaxAmount => Subtotal * (Product.TaxType.AmountOfTax / 100);
        public bool IsCanceled { get; set; }

        protected virtual decimal PriceWithoutTax()
        {
            if (GrossNetType == GrossNetType.Brutto)
                return Price / (100 + Product.TaxType.AmountOfTax) * 100;
            return Price;
        }
    }
}