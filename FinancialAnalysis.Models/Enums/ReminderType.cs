using System.ComponentModel.DataAnnotations;

namespace FinancialAnalysis.Models.Enums
{
    public enum ReminderType
    {
        [Display(Name = "Postalisch")] Potal,
        [Display(Name = "Mail")] Mail,
        [Display(Name = "Postalisch und Mail")] PostalAndMail
    }
}
