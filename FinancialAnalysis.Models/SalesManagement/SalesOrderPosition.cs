using FinancialAnalysis.Models.BaseClasses;

namespace FinancialAnalysis.Models.SalesManagement
{
    public class SalesOrderPosition : OrderPositionItem
    {
        public int SalesOrderPositionId { get; set; }
        public int RefSalesOrderId { get; set; }
        public bool IsShipped { get; set; }
    }
}