using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models
{
    public enum DepreciationType
    {
        [Display(Name = "Arithmetisch-degressiv")] SumOfTheYearsDigitMethod,
        [Display(Name = "Geometrisch-degressiv")] DecliningBalanceMethod,
        [Display(Name = "Linear")] Linear
    }
}
