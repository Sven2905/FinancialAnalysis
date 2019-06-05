using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;

namespace FinancialAnalysis.Models.TimeManagement
{
    [JsonObject(MemberSerialization.OptOut)]
    public class TimeObligatoryHour : BindableBase
    {
        public int TimeObligatoryHourId { get; set; }
        public int RefEmployeeId { get; set; }
        public decimal HoursPerDay { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }
}
