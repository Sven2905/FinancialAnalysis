using DevExpress.Mvvm;
using Newtonsoft.Json;
using System.Collections.Generic;
using Utilities;

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
        /// Name
        /// </summary>
        public string Name { get; set; }

        public List<FixedCostAllocationDetail> FixedCostAllocationDetails { get; set; } = new List<FixedCostAllocationDetail>();
    }
}