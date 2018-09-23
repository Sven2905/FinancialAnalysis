using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Class that categorize account centers to find, filter and order them
    /// </summary>
    public class CostAccountCategory : BindableBase
    {
        public int CostAccountCategoryId { get; set; }
        public string Description { get; set; }
        public int ParentCategoryId { get; set; }

        public IEnumerable<CostAccountCategory> SubCategories { get; set; }
    }
}
