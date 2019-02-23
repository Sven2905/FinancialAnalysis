using System.ComponentModel.DataAnnotations;

namespace FinancialAnalysis.Models.Enums
{
    public enum AccountType
    {
        [Display(Name = "Aktivkonto")] Active,
        [Display(Name = "Passivkonto")] Passive
    }
}