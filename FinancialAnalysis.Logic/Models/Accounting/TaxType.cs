using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.Models.Accounting
{
    /// <summary>
    /// Categories of taxes
    /// </summary>
    public class TaxType
    {
        public int TaxTypeId { get; set; }
        public string Name { get; set; }
        public decimal AmountOfTax { get; set; }

        public virtual List<CostAccount> CostAccounts { get; set; }
    }
}
