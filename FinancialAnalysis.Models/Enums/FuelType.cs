using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Enums
{
    public enum FuelType
    {
        [Display(Name = "Benzin")] Petrol,
        [Display(Name = "Diesel")] Diesel,
        [Display(Name = "Autogas")] LiquefiedPetroleumGas,
        [Display(Name = "Erdgas")] NaturalGas,
        [Display(Name = "Hybrid")]  Hybrid,
        [Display(Name = "Elektro")]  Electric,
    }
}
