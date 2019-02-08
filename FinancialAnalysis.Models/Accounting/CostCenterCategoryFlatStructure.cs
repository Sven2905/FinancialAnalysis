using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Accounting
{
    public class CostCenterCategoryFlatStructure : BindableBase
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public CostCenterCategory CostCenterCategory { get; set; }
        public CostCenter CostCenter { get; set; }
    }
}
