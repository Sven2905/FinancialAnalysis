using System.ComponentModel.DataAnnotations;

namespace FinancialAnalysis.Models
{
    public enum CostCenterType
    {
        [Display(Name = "Hauptkostenstelle")] Main,
        [Display(Name = "Nebenkostenstelle")] Extension,
        [Display(Name = "Hilfskostenstelle")] Indirect,
    }
}
