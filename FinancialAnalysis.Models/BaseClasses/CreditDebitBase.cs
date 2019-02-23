using DevExpress.Mvvm;
using FinancialAnalysis.Models.Accounting;

namespace FinancialAnalysis.Models.BaseClasses
{
    /// <summary>
    /// Basisklasse für Soll- und Haben-Positionen
    /// </summary>
    public class CreditDebitBase : BindableBase
    {
        /// <summary>
        /// Betrag
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Referenz-Id Kontenrahmen
        /// </summary>
        public int RefCostAccountId { get; set; }

        /// <summary>
        /// Kontenrahmen
        /// </summary>
        public CostAccount CostAccount { get; set; }

        /// <summary>
        /// Referenz-Id Buchung
        /// </summary>
        public int RefBookingId { get; set; }
    }
}