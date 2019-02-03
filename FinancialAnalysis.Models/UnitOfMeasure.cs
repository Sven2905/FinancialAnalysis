using DevExpress.Mvvm;

namespace FinancialAnalysis.Models
{
    public class UnitOfMeasure : BindableBase
    {
        public int UnitOfMeasureId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}