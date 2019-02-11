using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Accounting.CostCenterManagement
{
    public class CostCenterFlatStructure
    {
        public int Key { get; set; }
        public int ParentKey { get; set; }
        public CostCenterCategory CostCenterCategory { get; set; }
        public CostCenter CostCenter { get; set; }
    }
}
