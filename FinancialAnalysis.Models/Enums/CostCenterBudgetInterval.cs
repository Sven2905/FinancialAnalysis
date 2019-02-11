using System.ComponentModel.DataAnnotations;

namespace FinancialAnalysis.Models.Enums
{
    public enum CostCenterBudgetInterval
    {
        [Display(Name = "Jährlich")] Annually = 1,
        [Display(Name = "Quartalsweise")] Quarterly = 4,
        [Display(Name = "Monatlich")] Monthly = 12
    }
}