using DevExpress.Mvvm;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.General;
using FinancialAnalysis.Models.ProjectManagement;
using FinancialAnalysis.Models.TimeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using WebApiWrapper.ProjectManagement;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class TimeBookingOverviewViewModel : ViewModelBase
    {
        public TimeBookingOverviewViewModel()
        {
            BalanceInfoBoxViewModel.SetIconData("M256 8C119 8 8 119 8 256s111 248 248 248 248-111 248-248S393 8 256 8zm0 448c-110.5 0-200-89.5-200-200S145.5 56 256 56s200 89.5 200 200-89.5 200-200 200zm61.8-104.4l-84.9-61.7c-3.1-2.3-4.9-5.9-4.9-9.7V116c0-6.6 5.4-12 12-12h32c6.6 0 12 5.4 12 12v141.7l66.8 48.6c5.4 3.9 6.5 11.4 2.6 16.8L334.6 349c-3.9 5.3-11.4 6.5-16.8 2.6z");
            HolidayInfoBoxViewModel.SetIconData("M391.32 128H24.68c-21.95 0-32.94 26.53-17.42 42.05L192 354.79V480h-53.33c-14.73 0-26.67 11.94-26.67 26.67 0 2.95 2.39 5.33 5.33 5.33h181.33c2.95 0 5.33-2.39 5.33-5.33 0-14.73-11.94-26.67-26.67-26.67H224V354.79l107.7-107.7 22.66-22.66 54.37-54.37c15.52-15.53 4.53-42.06-17.41-42.06zM208 325.53L42.47 160h331.06L208 325.53zM432 0c-62.55 0-114.89 40.23-134.61 96h34.31c17.95-37.68 55.83-64 100.3-64 61.76 0 112 50.24 112 112s-50.24 112-112 112c-18.49 0-35.68-4.93-51.06-12.9l-23.52 23.52C379.23 279.92 404.59 288 432 288c79.53 0 144-64.47 144-144S511.53 0 432 0zm0 192c-.04 0-.08-.01-.13-.01-.2.21-.3.48-.51.69L405.04 219c8.46 3.05 17.46 5 26.96 5 44.12 0 80-35.89 80-80s-35.88-80-80-80c-26.05 0-49.01 12.68-63.62 32H432c26.47 0 48 21.53 48 48s-21.53 48-48 48z");
            selectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateString = selectedDate.ToString("MMMM yyyy");
            NextMonthCommand = new DelegateCommand(NextMonth);
            LastMonthCommand = new DelegateCommand(LastMonth);
            NewTimeBookingCommand = new DelegateCommand(NewTimeBooking);
            DeleteTimeBookingCommand = new DelegateCommand(DeleteTimeBooking);

            GetData();
        }

        private void GetData()
        {
            EmployeeList = Employees.GetAll().ToSvenTechCollection();
            SelectedEmployee = Employees.GetAll().FirstOrDefault(x => x.Firstname == Globals.ActiveUser.Firstname && x.Lastname == Globals.ActiveUser.Lastname);
        }

        public InfoBoxViewModel BalanceInfoBoxViewModel { get; set; } = new InfoBoxViewModel()
        {
            Color = SvenTechColors.BrushGreen,
            Description = "Saldo",
            Unit = "h",
            Value = 0,
        };

        public InfoBoxViewModel HolidayInfoBoxViewModel { get; set; } = new InfoBoxViewModel()
        {
            Color = SvenTechColors.BrushBlue,
            Description = "Verf. Urlaubstage",
            Unit = "d",
            Value = 20,
        };

        public void NextMonth()
        {
            selectedDate = selectedDate.AddMonths(1);
            DateString = selectedDate.ToString("MMMM yyyy");
        }

        public void LastMonth()
        {
            selectedDate = selectedDate.AddMonths(-1);
            DateString = selectedDate.ToString("MMMM yyyy");
        }

        public void NewTimeBooking()
        {
            if (SelectedEmployee != null)
                Messenger.Default.Send(new OpenTimeBookingWindowMessage { RefEmployeeId = SelectedEmployee.EmployeeId });
        }

        public void DeleteTimeBooking()
        {
            //if (SelectedTimeBooking.TimeBookingId > 0)
            //{
            //    TimeBookings.Delete(SelectedTimeBooking.TimeBookingId)
            //}
        }

        private DateTime selectedDate;
        public string DateString { get; set; }
        public TimeBooking SelectedTimeBooking { get; set; }
        public TimeHolidayType SelectedTimeHolidayType { get; set; }
        public TimeBookingType TimeBookingType { get; set; }
        public SvenTechCollection<TimeBooking> TimeBookingList { get; set; } = new SvenTechCollection<TimeBooking>();
        public SvenTechCollection<Employee> EmployeeList { get; set; }
        public Employee SelectedEmployee { get; set; }
        public DelegateCommand NextMonthCommand { get; set; }
        public DelegateCommand LastMonthCommand { get; set; }
        public DelegateCommand NewTimeBookingCommand { get; set; }
        public DelegateCommand DeleteTimeBookingCommand { get; set; }
    }
}
