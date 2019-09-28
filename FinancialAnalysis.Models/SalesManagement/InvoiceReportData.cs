using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.ClientManagement;
using FinancialAnalysis.Models.ProjectManagement;

namespace FinancialAnalysis.Models.SalesManagement
{
    /// <summary>
    /// Rechnungsdaten für Report
    /// </summary>
    public class InvoiceReportData
    {
        /// <summary>
        /// Auftrag
        /// </summary>
        public SalesOrder SalesOrder { get; set; }

        /// <summary>
        /// Absenderdaten
        /// </summary>
        public Client MyCompany { get; set; }

        /// <summary>
        /// Bearbeitender Mitarbeiter
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Rechnung
        /// </summary>
        public Invoice Invoice { get; set; }
    }
}