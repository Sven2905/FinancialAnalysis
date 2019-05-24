using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.TimeManagement
{
    public class ObligatoryHours : BindableBase
    {
        public int ObligatoryHoursId { get; set; }
        public int RefEmployeeId { get; set; }
        public decimal HoursPerDay { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }
}
