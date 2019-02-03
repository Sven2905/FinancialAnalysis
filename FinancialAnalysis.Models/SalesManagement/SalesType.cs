using DevExpress.Mvvm;

namespace FinancialAnalysis.Models.SalesManagement
{
    public class SalesType : BindableBase
    {
        public int SalesTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}