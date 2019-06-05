using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class TimeManagementViewModel : ViewModelBase
    {
        public bool ShowTimeBookingOverview => Globals.ActiveUser.IsAdministrator || UserManager.Instance.IsUserRightGranted(Globals.ActiveUser, Permission.TimeBooking);
        public bool ShowTimeHolidayRequest => Globals.ActiveUser.IsAdministrator || UserManager.Instance.IsUserRightGranted(Globals.ActiveUser, Permission.TimeHolidayRequest);
    }
}
