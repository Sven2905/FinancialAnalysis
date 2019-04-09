using DevExpress.Mvvm;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Kostenstellenaufteilungdetails
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class FixedCostAllocationDetail : BindableBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public int FixedCostAllocationDetailId { get; set; }

        /// <summary>
        /// Referenz auf die Hauptklasse für die Kostenstellenaufteilung
        /// </summary>
        public int RefFixedCostAllocationId { get; set; }

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