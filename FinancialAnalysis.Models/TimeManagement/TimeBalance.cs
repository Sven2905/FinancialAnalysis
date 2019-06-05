using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;

namespace FinancialAnalysis.Models.TimeManagement
{
    [JsonObject(MemberSerialization.OptOut)]
    public class TimeBalance : BindableBase
    {
        public int TimeBalanceId { get; set; }
        public int RefEmployeeId { get; set; }
        public DateTime Date { get; set; }
        public decimal Balance { get; set; }
    }
}
