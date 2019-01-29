using FinancialAnalysis.Models.ClientManagement;
using FinancialAnalysis.Models.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.SalesManagement
{
    public class SalesOrderReportData
    {
        public SalesOrder SalesOrder { get; set; }
        public Client MyCompany { get; set; }
        public Employee Employee { get; set; }
    }
}
