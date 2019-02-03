using System.ComponentModel.DataAnnotations;

namespace FinancialAnalysis.Models
{
    public enum CivilStatus
    {
        [Display(Name = "Ledig")] Single,
        [Display(Name = "Verheiratet")] Married,
        [Display(Name = "Geschieden")] Divorced,
        [Display(Name = "Verwitwet")] Widowed
    }
}