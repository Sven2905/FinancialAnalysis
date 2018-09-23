using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
