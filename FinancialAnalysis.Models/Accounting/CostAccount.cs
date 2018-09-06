namespace FinancialAnalysis.Models.Models.Accounting
{
    /// <summary>
    /// Account centers for Skr03
    /// </summary>
    public class CostAccount
    {
        public int CostAccountId { get; set; }
        public int AccountNumber { get; set; }
        public string Name { get; set; }
        public string GainsOutputAllocation { get; set; }
        public int SalesTaxAllocation { get; set; }
        public virtual int TaxTypeId { get; set; }
        public virtual TaxType TaxType { get; set; }
        public virtual CostAccountCategory CostAccountCategory { get; set; }
        public virtual int CostAccountCategoryId { get; set; }
    }
}