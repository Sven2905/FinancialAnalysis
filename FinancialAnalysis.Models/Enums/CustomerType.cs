using System.ComponentModel.DataAnnotations;

namespace FinancialAnalysis.Models
{
    public enum CustomerType
    {
        [Display(Name = "Kreditor")] Creditor,
        Debitor
    }
}