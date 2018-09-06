using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Models.Accounting
{
    /// <summary>
    /// Class that categorize account centers to find, filter and order them
    /// </summary>
    public class CostAccountCategory
    {
        public int CostAccountCategoryId { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public virtual CostAccountType CostAccountType { get; set; }
        public int? CostAccountTypeId { get; set; }

        public List<CostAccount> CostAccounts { get; set; }
    }
}
