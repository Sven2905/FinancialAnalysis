using System.ComponentModel.DataAnnotations;

namespace FinancialAnalysis.Models
{
    public enum DepreciationType
    {
        [Display(Name = "Arithmetisch-degressiv")] SumOfTheYearsDigitMethod,
        [Display(Name = "Geometrisch-degressiv")] DecliningBalanceMethod,
        [Display(Name = "Linear")] Linear,
        [Display(Name = "Leistungsabhängig")] PerfomanceBased,
    }
}
