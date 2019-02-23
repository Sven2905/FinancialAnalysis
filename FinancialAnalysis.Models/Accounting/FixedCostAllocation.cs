namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Aufteilungsanteile auf Kostenstelle
    /// </summary>
    public class FixedCostAllocation
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