using FinancialAnalysis.Models.ProjectManagement;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Account centers for Skr03
    /// </summary>
    public class CostAccount
    {
        public int Id { get; set; }
        public int AccountNumber { get; set; }
        public string Description { get; set; }
        public int RefTaxTypeId { get; set; }
        public TaxType TaxType { get; set; }
        public CostAccountCategory CostAccountCategory { get; set; }
        public int RefCostAccountCategoryId { get; set; }
        public bool IsVisible { get; set; }
        public int RefDebitorId { get; set; }
        public Debitor Debitor { get; set; }
        public int RefCreditorId { get; set; }
        public Creditor Creditor { get; set; }
    }
}