using DevExpress.Mvvm;

namespace FinancialAnalysis.Models.SalesManagement
{
    public class InvoiceType : BindableBase
    {
        public int InvoiceTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}