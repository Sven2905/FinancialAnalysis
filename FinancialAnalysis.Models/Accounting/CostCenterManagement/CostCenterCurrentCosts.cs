using DevExpress.Mvvm;
using FinancialAnalysis.Models.Enums;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.Accounting.CostCenterManagement
{
    /// <summary>
    /// Aktuelle Kosten für Kostenstelle
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class CostCenterCurrentCosts : BaseClass
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