using DevExpress.Mvvm;

namespace FinancialAnalysis.Models.SalesManagement
{
    public class InvoicePosition : BindableBase
    {
        public int InvoicePositionId { get; set; }
        public int RefInvoiceId { get; set; }
        public int RefSalesOrderPositionId { get; set; }
        public int Quantity { get; set; }
    }
}