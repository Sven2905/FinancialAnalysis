using System.ComponentModel.DataAnnotations;

namespace FinancialAnalysis.Models
{
    public enum DepreciationType
    {
        [Display(Name = "Linear")] Linear = 1,
        [Display(Name = "Leistungsabhängig")] PerfomanceBased = 2,
        [Display(Name = "Arithmetisch-degressiv")] SumOfTheYearsDigitMethod = 3,
        [Display(Name = "Geometrisch-degressiv")] DecliningBalanceMethod = 4,
    }
}