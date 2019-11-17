using DevExpress.Mvvm;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Steuertyp
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class TaxType : BaseClass
    {
        /// <summary>
        /// Id
        /// </summary>
        public int TaxTypeId { get; set; }

        /// <summary>
        /// Beschreibung
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Kurzbeschreibung
        /// </summary>
        public string DescriptionShort { get; set; }

        /// <summary>
        /// Höhe der Steuer
        /// </summary>
        public decimal AmountOfTax { get; set; }

        /// <summary>
        /// Höhe der Steuer / 100
        /// </summary>
        public decimal AmountOfTaxDecimal => AmountOfTax / 100;

        /// <summary>
        /// Steuerart
        /// </summary>
        public TaxCategory TaxCategory { get; set; } // Steuerart

        /// <summary>
        /// Referenz-Id des zugehörigen Kontenrahmens
        /// </summary>
        public int RefCostAccount { get; set; }

        /// <summary>
        /// Zugehöriger Kontenrahmen
        /// </summary>
        public CostAccount CostAccount { get; set; }
    }
}