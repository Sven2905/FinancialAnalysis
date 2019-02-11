using FinancialAnalysis.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Accounting.CostCenterManagement
{
    public class CostCenterCurrentCosts
    {
        public Months MonthIndex { get; set; }
        public decimal Amount { get; set; }
        public int RefCostCenterId { get; set; }
    }
}
