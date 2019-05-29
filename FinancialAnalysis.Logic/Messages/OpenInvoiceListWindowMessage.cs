using FinancialAnalysis.Models.SalesManagement;
using Utilities;

namespace FinancialAnalysis.Logic.Messages
{
    public class OpenInvoiceListWindowMessage
    {
        public OpenInvoiceListWindowMessage(SvenTechCollection<Invoice> Invoices)
        {
            this.Invoices = Invoices;
        }

        public SvenTechCollection<Invoice> Invoices { get; set; }
    }
}
