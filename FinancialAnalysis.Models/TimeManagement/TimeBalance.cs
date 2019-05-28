using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
