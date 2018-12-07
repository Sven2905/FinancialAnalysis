using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ProjectViewModel : ViewModelBase
    {



        public TimeSpan CalculateWorkHours(DateTime StartTime, DateTime EndTime)
        {
            var WorkTime = (StartTime - EndTime);

            if (WorkTime.TotalHours > 6 && WorkTime.TotalHours < new TimeSpan(6, 30, 0).TotalHours)
            {
                var requiredBreakTime = WorkTime - new TimeSpan(6, 0, 0);

                return WorkTime - requiredBreakTime;
            }

            else if (WorkTime.TotalHours > 6 && WorkTime.TotalHours < 9)
            {
                var requiredBreakTime = new TimeSpan(0, 30, 0);

                return WorkTime - requiredBreakTime;
            }

            else if (WorkTime.TotalHours > 9 && WorkTime.TotalHours < new TimeSpan(9, 15, 0).TotalHours)
            {
                var requiredBreakTime = WorkTime - new TimeSpan(6, 0, 0);

                return WorkTime - requiredBreakTime;
            }

            else if(WorkTime.TotalHours > 9)
            {
                var requiredBreakTime = new TimeSpan(0, 45, 0);

                return WorkTime - requiredBreakTime;
            }
            else
            {
                return WorkTime;
            }
        }
    }
}
