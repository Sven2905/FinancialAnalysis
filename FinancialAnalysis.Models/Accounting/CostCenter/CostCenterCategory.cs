using System.Collections.Generic;
using DevExpress.Mvvm;

namespace FinancialAnalysis.Models.Accounting
{
    public class CostCenterCategory : BindableBase
    {
        public int CostCenterCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<CostCenter> CostCenters { get; set; } = new List<CostCenter>();
    }
}