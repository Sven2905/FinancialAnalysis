using DevExpress.Mvvm;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.ClientManagement;

namespace FinancialAnalysis.Models.BaseClasses
{
    /// <summary>
    /// Basisklasse für Kreditoren und Debitoren
    /// </summary>
    public class CreditorDebitorBase : BindableBase
    {
        /// <summary>
        /// Referenz-Id Klient
        /// </summary>
        public int RefClientId { get; set; }

        /// <summary>
        /// Klient
        /// </summary>
        public Client Client { get; set; }

        /// <summary>
        /// Referenz-Id Kontenrahmen
        /// </summary>
        public int RefCostAccountId { get; set; }

        /// <summary>
        /// Kontenrahmen
        /// </summary>
        public CostAccount CostAccount { get; set; }
    }
}