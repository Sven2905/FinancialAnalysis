using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.TimeManagement
{
    public class TimeEmployeeAbsence : BindableBase
    {
        public int TimeEmployeeAbsenceId { get; set; }
        public int RefEmployeeId { get; set; }
        public DateTime FirstDay { get; set; }
        public DateTime LastDay { get; set; }
        public bool OnlyWorkingDays { get; set; }
        public int RefTimeAbsentReasonId { get; set; }
    }
}
