using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.TimeManagement
{
    public class TimeHolidayEmployee : BindableBase
    {
        public int TimeHolidayEmployeeId { get; set; }
        public int RefEmployeeId { get; set; }
        public DateTime FirstDay { get; set; }
        public bool IsHalfFirstDay { get; set; }
        public DateTime LastDay { get; set; }
        public bool IsHalfLastDay { get; set; }
        public bool OnlyWorkingDays { get; set; }
        public string Reason { get; set; }
        public int RefTimeHolidayTypeId { get; set; }
        public DateTime TimeStamp { get; set; }
        public int RefApprovalEmployeeId { get; set; }

        /// <summary>
        /// Sonderurlaub
        /// </summary>
        public bool IsSpecialLeave { get; set; }
        public bool IsApproved { get; set; }
        public DateTime ApprovalDate { get; set; }
    }
}
