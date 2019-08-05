using System.ComponentModel.DataAnnotations;

namespace FinancialAnalysis.Models
{
    public enum TimeBookingType
    {
        [Display(Name = "Kommen")] Login,
        [Display(Name = "Gehen")] Logout,
        [Display(Name = "Pause Start")] StartBreak,
        [Display(Name = "Pause Ende")] EndBreak,
    }
}