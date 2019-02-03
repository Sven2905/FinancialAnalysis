using System.ComponentModel.DataAnnotations;

namespace FinancialAnalysis.Models
{
    public enum Gender
    {
        [Display(Name = "Männlich")] male,
        [Display(Name = "Weiblich")] female
    }
}