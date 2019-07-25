using FinancialAnalysis.Models.Enums;
using System;

namespace FinancialAnalysis.Models.General
{
    public class TimerLastRuntime
    {
        public int TimerLastRuntimeId { get; set; }
        public DateTime LastRuntime { get; set; }
        public TimerType TimerType { get; set; }
    }
}