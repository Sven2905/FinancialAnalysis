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

        public TimeBalance(int RefUserId, DateTime Date, double Balance)
        {
            this.RefUserId = RefUserId;
            this.Date = Date;
            this.Balance = Balance;
        }

        public int TimeBalanceId { get; set; }
        public int RefUserId { get; set; }
        public DateTime Date { get; set; }
        public double Balance { get; set; }
    }
}