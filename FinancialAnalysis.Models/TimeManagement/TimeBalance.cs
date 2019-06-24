using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;

namespace FinancialAnalysis.Models.TimeManagement
{
    [JsonObject(MemberSerialization.OptOut)]
    public class TimeBalance : BindableBase
    {
        public TimeBalance()
        {

        }

        public TimeBalance(int RefEmployeeId, DateTime Date, double Balance)
        {
            this.RefEmployeeId = RefEmployeeId;
            this.Date = Date;
            this.Balance = Balance;
        }

        public int TimeBalanceId { get; set; }
        public int RefEmployeeId { get; set; }
        public DateTime Date { get; set; }
        public double Balance { get; set; }
    }
}
