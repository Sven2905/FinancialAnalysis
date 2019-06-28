using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.TimeManagement
{
    public class TimeBookingDayItem : BindableBase
    {
        public DateTime BookingDate { get; set; }
        public DayOfWeek BookingDay => BookingDate.DayOfWeek;
        public double ObligatoryHours { get; set; }
        public TimeSpan WorkingHours { get; set; }
        public double Balance { get; set; }
        public TimeSpan BreaktimeHours { get; set; }
        public double DailySaldo => WorkingHours.Subtract(TimeSpan.FromHours(ObligatoryHours)).TotalHours;
        public string AbsentReason { get; set; }
    }
}
