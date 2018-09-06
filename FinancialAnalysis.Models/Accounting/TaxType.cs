using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Models.Accounting
{
    /// <summary>
    /// Categories of taxes
    /// </summary>
    public class TaxType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal AmountOfTax { get; set; }
    }
}
