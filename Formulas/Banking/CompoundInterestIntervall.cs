using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
    public enum CompoundInterestIntervall
    {
        [Display(Name = "Jährlich")] Yearly = 1,
        [Display(Name = "Quartalsweise")] Quarterly = 4,
        [Display(Name = "Monatlich")] Monthly = 12,
        [Display(Name = "Wächentlich")] Weekly = 52,
        [Display(Name = "Täglich")] Daily = 360
    }
}
