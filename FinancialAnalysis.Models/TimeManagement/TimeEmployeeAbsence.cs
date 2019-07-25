using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;

namespace FinancialAnalysis.Models.TimeManagement
{
    [JsonObject(MemberSerialization.OptOut)]
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