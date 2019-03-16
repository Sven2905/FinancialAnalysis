using DevExpress.Mvvm;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Aufteilungsanteile auf Kostenstelle
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class FixedCostAllocation : BindableBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public int FixedCostAllocationId { get; set; }

        /// <summary>
        /// Referenz-Id Kostenstelle
        /// </summary>
        public int RefCostCenterId { get; set; }

        /// <summary>
        /// Kostenstelle
        /// </summary>
        public CostCenter CostCenter { get; set; } = new CostCenter();

        /// <summary>
        /// Anteile
        /// </summary>
        public double Shares { get; set; } = 0;
    }
}