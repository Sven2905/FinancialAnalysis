using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.Models.Accounting
{
    public class CostAccountType
    {
        public int CostAccountTypeId { get; set; }
        public string Name { get; set; }

        public virtual List<CostAccountCategory> CostAccountCategories { get; set; }
    }
}
