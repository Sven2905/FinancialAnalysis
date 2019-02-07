using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Accounting
{
    public class FixedCostAllocation
    {
        public int FixedCostAllocationId { get; set; }
        public int RefCostCenterId { get; set; }
        public CostCenter CostCenter { get; set; }
        public double Shares { get; set; }
    }
}
