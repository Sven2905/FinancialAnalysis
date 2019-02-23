using FinancialAnalysis.Models.ClientManagement;
using FinancialAnalysis.Models.ProjectManagement;

namespace FinancialAnalysis.Models.SalesManagement
{
    /// <summary>
    /// Auftragsdaten für Report
    /// </summary>
    public class SalesOrderReportData
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
        public Employee Employee { get; set; }
    }
}