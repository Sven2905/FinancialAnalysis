using DevExpress.Mvvm;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.TimeManagement
{
    [JsonObject(MemberSerialization.OptOut)]
    public class TimeHolidayType : BindableBase
    {
        public int TimeHolidayTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}