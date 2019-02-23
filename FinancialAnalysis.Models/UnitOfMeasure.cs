using DevExpress.Mvvm;

namespace FinancialAnalysis.Models
{
    /// <summary>
    /// Maßeinheit
    /// </summary>
    public class UnitOfMeasure : BindableBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public int UnitOfMeasureId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Beschreibung
        /// </summary>
        public string Description { get; set; }
    }
}