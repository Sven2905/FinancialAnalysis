using System.ComponentModel.DataAnnotations;

namespace FinancialAnalysis.Models
{
    public enum TaxCategory
    {
        [Display(Name = "Keine")] None,
        [Display(Name = "Netto")] Netto,
        [Display(Name = "Brutto")] Brutto,
        [Display(Name = "i.g.E")] igE,
        [Display(Name = "13b")] thirteenB,
        [Display(Name = "50%")] fiftyPercent
    }
}