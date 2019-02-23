using FinancialAnalysis.Models.Enums;

namespace FinancialAnalysis.Models.Accounting.CostCenterManagement
{
    /// <summary>
    /// Aktuelle Kosten für Kostenstelle
    /// </summary>
    public class CostCenterCurrentCosts
    {
        /// <summary>
        /// Index des Monats
        /// </summary>
        public Months MonthIndex { get; set; }

        /// <summary>
        /// Höhe der Kosten
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Referenz-Id der Kostenstelle
        /// </summary>
        public int RefCostCenterId { get; set; }
    }
}