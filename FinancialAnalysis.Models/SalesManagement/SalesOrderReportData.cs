using FinancialAnalysis.Models.ClientManagement;
using FinancialAnalysis.Models.ProjectManagement;

namespace FinancialAnalysis.Models.SalesManagement
{
    public class SalesOrderReportData
    {
        public SalesOrder SalesOrder { get; set; }
        public Client MyCompany { get; set; }
        public Employee Employee { get; set; }
    }
}