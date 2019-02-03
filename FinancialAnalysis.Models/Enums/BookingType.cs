using System.ComponentModel.DataAnnotations;

namespace FinancialAnalysis.Models
{
    public enum BookingType
    {
        [Display(Name = "Rechnung")] Invoice,
        [Display(Name = "Gutschrift")] CreditAdvice
    }
}