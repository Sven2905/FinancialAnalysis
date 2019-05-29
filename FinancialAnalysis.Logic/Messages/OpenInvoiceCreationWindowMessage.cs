using FinancialAnalysis.Models.SalesManagement;

namespace FinancialAnalysis.Logic.Messages
{
    public class OpenInvoiceCreationWindowMessage
    {
        public OpenInvoiceCreationWindowMessage(SalesOrder SalesOrder)
        {
            this.SalesOrder = SalesOrder;
        }

        public SalesOrder SalesOrder { get; set; }
    }
}
