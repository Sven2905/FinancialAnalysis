using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.ProjectManagement
{
    public class Debitor
    {
        public int Id { get; set; }
        public int RefCompanyId { get; set; }
        public Company Company { get; set; }
        public int RefCostAccountId { get; set; }
        public CostAccount CostAccount { get; set; }
    }
}
