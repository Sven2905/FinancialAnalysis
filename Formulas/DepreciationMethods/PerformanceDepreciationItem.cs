using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas.DepreciationMethods
{
    public class PerformanceDepreciationItem
    {
        public PerformanceDepreciationItem()
        {

        }

        public PerformanceDepreciationItem(int Year, decimal Power)
        {
            this.Year = Year;
            this.Power = Power;
        }

        public int Year { get; set; }
        public decimal Power { get; set; }
    }
}
