using System.ComponentModel.DataAnnotations;

namespace FinancialAnalysis.Models
{
    public enum Status
    {
        [Display(Name = "Nicht gestartet")] NotStarted,
        [Display(Name = "In Bearbeitung")] InProgress,
        [Display(Name = "Wartend")] Pending,
        [Display(Name = "Genehmigt")] Approved,
        [Display(Name = "Fertig")] Done,
        [Display(Name = "Abgeschlossen")] Completed,
        [Display(Name = "Blockiert")] Blocked
    }
}