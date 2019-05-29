using System.ComponentModel.DataAnnotations;

namespace FinancialAnalysis.Models.Enums
{
    public enum Months
    {
        [Display(Name = "Januar")] January = 1,
        [Display(Name = "Februar")] February,
        [Display(Name = "März")] March,
        [Display(Name = "April")] April,
        [Display(Name = "Mai")] May,
        [Display(Name = "Juni")] June,
        [Display(Name = "Juli")] July,
        [Display(Name = "August")] August,
        [Display(Name = "September")] September,
        [Display(Name = "Oktober")] October,
        [Display(Name = "November")] November,
        [Display(Name = "Dezember")] December
    }
}
