using DevExpress.Mvvm;

namespace FinancialAnalysis.Models.Accounting
{
    public class CostAccount : BindableBase
    {
        public CostAccount()
        {
            IsEditable = true;
        }

        public int CostAccountId { get; set; }
        public int AccountNumber { get; set; }
        public string Description { get; set; }
        public int RefTaxTypeId { get; set; } = 1;
        public TaxType TaxType { get; set; }
        public CostAccountCategory CostAccountCategory { get; set; }
        public int RefCostAccountCategoryId { get; set; }
        public bool IsVisible { get; set; }
        public bool IsEditable { get; set; }
        public string DisplayName => $"{AccountNumber} - {Description}";
        public int RefIncomeSummaryAndBalanceAccountId { get; set; }
        public int RefActiveBalanceAccountId { get; set; }
        public int RefPassiveBalanceAccountId { get; set; }
        public int RefGainAndLossAccountId { get; set; }
    }
}