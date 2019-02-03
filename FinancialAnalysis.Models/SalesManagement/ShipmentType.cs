using DevExpress.Mvvm;

namespace FinancialAnalysis.Models.SalesManagement
{
    public class ShipmentType : BindableBase
    {
        public int ShipmentTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}