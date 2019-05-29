using DevExpress.Mvvm;

namespace FinancialAnalysis.Models.TimeManagement
{
    public class TimeHolidayType : BindableBase
    {
        public int TimeHolidayTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
