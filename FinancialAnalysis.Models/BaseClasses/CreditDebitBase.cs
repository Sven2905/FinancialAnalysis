using DevExpress.Mvvm;
using FinancialAnalysis.Models.Accounting;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.BaseClasses
{
    /// <summary>
    /// Basisklasse für Soll- und Haben-Positionen
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class CreditDebitBase : BaseClass
    {
        /// <summary>
        /// Betrag
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Referenz-Id Kontenrahmen
        /// </summary>
        public int RefCostAccountId { get; set; } = 1;

        /// <summary>
        /// Kontenrahmen
        /// </summary>
        public CostAccount CostAccount { get; set; }

        /// <summary>
        /// Referenz-Id Buchung
        /// </summary>
        public int RefBookingId { get; set; }

        /// <summary>
        /// Beschreibung / Bezeichnung zur Buchung
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gibt an, ob es sich dabei um einen Steuerbetrag handelt
        /// </summary>
        public bool IsTax { get; set; }
    }
}