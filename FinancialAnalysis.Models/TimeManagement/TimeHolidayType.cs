using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.TimeManagement
{
    public class TimeHolidayType : BindableBase
    {
        public int TimeHolidayTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
