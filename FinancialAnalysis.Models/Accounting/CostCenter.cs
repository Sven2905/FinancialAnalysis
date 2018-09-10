using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Class that represents a cost center for matching costs and bills
    /// </summary>
    public class CostCenter
    {
        public int CostCenterId { get; set; }
        public string Name { get; set; }
    }
}
