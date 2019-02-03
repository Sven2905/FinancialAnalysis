using System.ComponentModel.DataAnnotations;

namespace FinancialAnalysis.Models
{
    public enum ClientType
    {
        [Display(Name = "Geschäftskunde")] Business,
        [Display(Name = "Privatkunde")] Private
    }
}