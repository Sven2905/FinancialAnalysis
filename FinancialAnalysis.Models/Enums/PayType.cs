using System.ComponentModel.DataAnnotations;

namespace FinancialAnalysis.Models
{
    public enum PayType
    {
        [Display(Name = "Intervall")] Intervall,
        [Display(Name = "Aktueller Monat")] ThisMonth,
        [Display(Name = "Nachfolgender Monat")] NextMonth
        //[Display(Name = "Bestimmter Tag")] NextSpecificDay
    }
}