using DevExpress.Mvvm;
using FinancialAnalysis.Models.Accounting;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.ProductManagement
{
    /// <summary>
    /// Produkt
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class Product : BaseClass
    {
        /// <summary>
        /// Id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Name des Produkts
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Barcode
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// Artikelnummer
        /// </summary>
        public int ItemNumber { get; set; }

        /// <summary>
        /// Beschreibung
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gewicht
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Breite
        /// </summary>
        public double DimensionX { get; set; }

        /// <summary>
        /// Tiefe
        /// </summary>
        public double DimensionY { get; set; }

        /// <summary>
        /// Höhe
        /// </summary>
        public double DimensionZ { get; set; }

        /// <summary>
        /// Daten des Produktbilds
        /// </summary>
        public byte[] Picture { get; set; }

        /// <summary>
        /// Referenz-Id des Steuertyps
        /// </summary>
        public int RefTaxTypeId { get; set; } = 11;

        /// <summary>
        /// Steuertyp
        /// </summary>
        public TaxType TaxType { get; set; }

        /// <summary>
        /// Verpackungseinheit
        /// </summary>
        public int PackageUnit { get; set; }

        /// <summary>
        /// Referenz-Id der Produktkategorie
        /// </summary>
        public int RefProductCategoryId { get; set; } = 1;

        /// <summary>
        /// Produktkategorie
        /// </summary>
        public ProductCategory ProductCategory { get; set; }

        /// <summary>
        /// Basiseinkaufspreis
        /// </summary>
        public decimal DefaultBuyingPrice { get; set; }

        /// <summary>
        /// Basisverkaufspreis
        /// </summary>
        public decimal DefaultSellingPrice { get; set; }
    }
}