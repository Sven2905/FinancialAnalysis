using FinancialAnalysis.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.General
{
    public class TimerLastRuntime
    {
        public int TimerLastRuntimeId { get; set; }
        public DateTime LastRuntime { get; set; }
        public TimerType TimerType { get; set; }
    }
}
