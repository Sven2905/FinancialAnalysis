using DevExpress.Mvvm;
using System;

namespace FinancialAnalysis.Models.TimeManagement
{
    public class TimeBalance : BindableBase
    {
        public int TimeBalanceId { get; set; }
        public int RefEmployeeId { get; set; }
        public DateTime Date { get; set; }
        public decimal Balance { get; set; }
    }
}
