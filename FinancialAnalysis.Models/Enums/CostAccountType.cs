using System.ComponentModel.DataAnnotations;

namespace FinancialAnalysis.Models
{
    public enum CostAccountType
    {
        [Display(Name = "Neutral")] Neutral,
        [Display(Name = "Einnahmen")] Gain,
        [Display(Name = "Ausgaben")] Loss
    }
}