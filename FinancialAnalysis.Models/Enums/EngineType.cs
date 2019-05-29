using System.ComponentModel.DataAnnotations;

namespace FinancialAnalysis.Models.Enums
{
    public enum EngineType
    {
        [Display(Name = "Benzin")] Gasoline = 1,
        [Display(Name = "Diesel")] Diesel,
        [Display(Name = "Autogas")] Gas,
        [Display(Name = "Hybrid")] Hybrid,
        [Display(Name = "Elektro")] Electro,
    }
}
