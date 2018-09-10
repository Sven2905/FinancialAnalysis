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
    public class CostAccountCategory
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int ParentCategoryId { get; set; }
    }
}
