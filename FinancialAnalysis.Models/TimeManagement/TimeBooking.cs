using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;

namespace FinancialAnalysis.Models.TimeManagement
{
    [JsonObject(MemberSerialization.OptOut)]
    public class TimeBooking : BindableBase
    {
        public int TimeBookingId { get; set; }
        public int RefEmployeeId { get; set; }
        public DateTime BookingTime { get; set; } = DateTime.Now;
        public TimeBookingType TimeBookingType { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
