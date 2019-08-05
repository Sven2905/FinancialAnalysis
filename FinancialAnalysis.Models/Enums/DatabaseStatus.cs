using System.ComponentModel.DataAnnotations;

namespace FinancialAnalysis.Models.Enums
{
    public enum DatabaseStatus
    {
        [Display(Name = "Getrennt")] Disconnected,
        [Display(Name = "Wartend")] Pending,
        [Display(Name = "Verbunden")] Connected
    }
}