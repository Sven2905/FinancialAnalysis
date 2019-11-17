using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;

namespace FinancialAnalysis.Models.TimeManagement
{
    [JsonObject(MemberSerialization.OptOut)]
    public class TimeUserAbsence : BaseClass
    {
        public int TimeUserAbsenceId { get; set; }
        public int RefUserId { get; set; }
        public DateTime FirstDay { get; set; }
        public DateTime LastDay { get; set; }
        public bool OnlyWorkingDays { get; set; }
        public int RefTimeAbsentReasonId { get; set; }
    }
}