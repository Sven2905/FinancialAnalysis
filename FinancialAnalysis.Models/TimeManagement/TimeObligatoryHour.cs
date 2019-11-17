using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;

namespace FinancialAnalysis.Models.TimeManagement
{
    [JsonObject(MemberSerialization.OptOut)]
    public class TimeObligatoryHour : BaseClass
    {
        public TimeObligatoryHour()
        {
        }

        public TimeObligatoryHour(int RefUserId, double HoursPerDay, DayOfWeek DayOfWeek)
        {
            this.RefUserId = RefUserId;
            this.HoursPerDay = HoursPerDay;
            this.DayOfWeek = DayOfWeek;
        }

        public int TimeObligatoryHourId { get; set; }
        public int RefUserId { get; set; }
        public double HoursPerDay { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }
}