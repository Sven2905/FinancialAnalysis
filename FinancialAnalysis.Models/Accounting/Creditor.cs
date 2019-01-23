using DevExpress.Mvvm;
using FinancialAnalysis.Models.CompanyManagement;

namespace FinancialAnalysis.Models.Accounting
{
    public class Creditor : BindableBase
    {
        public Creditor()
        {
            Company = new Company();
            CostAccount = new CostAccount();
        }

        public int CreditorId { get; set; }
        public int RefCompanyId { get; set; }
        public Company Company { get; set; }
        public int RefCostAccountId { get; set; }
        public CostAccount CostAccount { get; set; }
    }
}