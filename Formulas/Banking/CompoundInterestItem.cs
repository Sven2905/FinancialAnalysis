using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas.Banking
{
    public class CompoundInterestItem
    {
        /// <summary>
        /// Jahre
        /// </summary>
        public double Years { get; set; }
        
        /// <summary>
        /// Startkapital
        /// </summary>
        public decimal SeedCapital { get; set; }
        
        /// <summary>
        /// Endkapital
        /// </summary>
        public decimal FinalCapital { get; set; }
        
        /// <summary>
        /// Zinssatz
        /// </summary>
        public double Rate { get; set; }
        
        /// <summary>
        /// Intervall
        /// </summary>
        public CompoundInterestIntervall CompoundInterestIntervall { get; set; } = CompoundInterestIntervall.Yearly;
    }
}
