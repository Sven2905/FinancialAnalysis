using DevExpress.Mvvm;

namespace FinancialAnalysis.Models.Accounting
{
    public class CostAccount : BindableBase
    {
        public CostAccount()
        {
            RefTaxTypeId = 1;
            IsEditable = true;
        }

        public int CostAccountId { get; set; }
        public int AccountNumber { get; set; }
        public string Description { get; set; }
        public int RefTaxTypeId { get; set; }
        public TaxType TaxType { get; set; }
        public CostAccountCategory CostAccountCategory { get; set; }
        public int RefCostAccountCategoryId { get; set; }
        public bool IsVisible { get; set; }
        public bool IsEditable { get; set; }
        public string DisplayName { get { return $"{AccountNumber} - {Description}"; } }
    }
}