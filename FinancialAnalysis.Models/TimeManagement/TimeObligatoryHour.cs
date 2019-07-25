using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;

namespace FinancialAnalysis.Models.TimeManagement
{
    [JsonObject(MemberSerialization.OptOut)]
    public class TimeObligatoryHour : BindableBase
    {
        public TimeObligatoryHour()
        {
        }

        public TimeObligatoryHour(int RefEmployeeId, double HoursPerDay, DayOfWeek DayOfWeek)
        {
            this.RefEmployeeId = RefEmployeeId;
            this.HoursPerDay = HoursPerDay;
            this.DayOfWeek = DayOfWeek;
        }

        public int TimeObligatoryHourId { get; set; }
        public int RefEmployeeId { get; set; }
        public double HoursPerDay { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }
}