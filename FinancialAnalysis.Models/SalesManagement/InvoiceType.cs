using DevExpress.Mvvm;

namespace FinancialAnalysis.Models.SalesManagement
{
    /// <summary>
    /// Rechnungstyp
    /// </summary>
    public class InvoiceType : BindableBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public int InvoiceTypeId { get; set; }

        /// <summary>
        /// Name des Rechnungstyps
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Beschreibung
        /// </summary>
        public string Description { get; set; }
    }
}