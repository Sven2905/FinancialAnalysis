using DevExpress.Mvvm;
using FinancialAnalysis.Models.ProductManagement;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.SalesManagement
{
    /// <summary>
    /// Auftragspositionen
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class SalesOrderPosition : BindableBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public int SalesOrderPositionId { get; set; }

        /// <summary>
        /// Referenz-Id des Auftrags
        /// </summary>
        public int RefSalesOrderId { get; set; }

        /// <summary>
        /// Ist komplett versandt
        /// </summary>
        public bool IsShipped { get; set; }

        /// <summary>
        /// Rabatt
        /// </summary>
        public decimal DiscountPercentage { get; set; }

        /// <summary>
        /// Rabatt / 100
        /// </summary>
        public decimal DiscountPercentageForLabel => DiscountPercentage / 100;

        /// <summary>
        /// Referenz-Id Produkt
        /// </summary>
        public int RefProductId { get; set; }

        /// <summary>
        /// Produkt
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// Menge des Produkts
        /// </summary>
        public decimal Quantity { get; set; } = 1;

        /// <summary>
        /// Preis des Produkts
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Beschreibung
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Brutto / Netto
        /// </summary>
        public GrossNetType GrossNetType { get; set; } = GrossNetType.Netto;

        /// <summary>
        /// Zwischenbetrag ohne Steuer
        /// </summary>
        public decimal Subtotal => SubtotalWithoutDiscount - DiscountAmount;

        /// <summary>
        /// Zwischenbetrag ohne Steuer pro Stück
        /// </summary>
        public decimal SubtotalPerUnit => (SubtotalWithoutDiscount - DiscountAmount) / Quantity;

        /// <summary>
        /// Endbetrag
        /// </summary>
        public decimal Total => Subtotal + TaxAmount;

        /// <summary>
        /// Endbetrag pro Stück
        /// </summary>
        public decimal TotalPerUnit => (Subtotal + TaxAmount) / Quantity;

        /// <summary>
        /// Rabatt (%)
        /// </summary>
        public decimal DiscountAmount => SubtotalWithoutDiscount * (DiscountPercentage / 100);

        /// <summary>
        /// Rabatt (%) pro Stück
        /// </summary>
        public decimal DiscountAmountPerUnit => (SubtotalWithoutDiscount * (DiscountPercentage / 100)) / Quantity;

        /// <summary>
        /// Zwischenbetrag ohne Rabatt
        /// </summary>
        public decimal SubtotalWithoutDiscount => PriceWithoutTax() * Quantity;

        /// <summary>
        /// Zwischenbetrag ohne Rabatt pro Stück
        /// </summary>
        public decimal SubtotalWithoutDiscountPerUnit => SubtotalWithoutDiscount / Quantity;

        /// <summary>
        /// Höhe der zu zahlenden Steuer
        /// </summary>
        public decimal TaxAmount => Subtotal * (Product.TaxType.AmountOfTax / 100);

        /// <summary>
        /// Ist storniert
        /// </summary>
        public bool IsCanceled { get; set; }

        /// <summary>
        /// Betrag ohne Steuern
        /// </summary>
        protected virtual decimal PriceWithoutTax()
        {
            if (GrossNetType == GrossNetType.Brutto)
            {
                return Price / (100 + Product.TaxType.AmountOfTax) * 100;
            }

            return Price;
        }
    }
}