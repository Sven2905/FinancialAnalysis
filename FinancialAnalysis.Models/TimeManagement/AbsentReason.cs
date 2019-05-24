using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.TimeManagement
{
    public class AbsentReason : BindableBase
    {
        public int AbsentReasonId { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public bool DeleteAble { get; set; } = true;
    }
}
