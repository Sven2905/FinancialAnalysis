using DevExpress.Mvvm;

namespace FinancialAnalysis.Models.SalesManagement
{
    /// <summary>
    /// Auftragstyp
    /// </summary>
    public class SalesType : BindableBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public int SalesTypeId { get; set; }

        /// <summary>
        /// Name des Typs
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Beschreibung
        /// </summary>
        public string Description { get; set; }
    }
}