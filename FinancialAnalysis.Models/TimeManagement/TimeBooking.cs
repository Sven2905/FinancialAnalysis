using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.TimeManagement
{
    public class TimeBooking : BindableBase
    {
        public int TimeBookingId { get; set; }
        public int RefEmployeeId { get; set; }
        public DateTime BookingTime { get; set; }
        public TimeBookingType TimeBookingType { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
