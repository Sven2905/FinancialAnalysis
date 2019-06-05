using DevExpress.Mvvm;
using FinancialAnalysis.Logic.Manager;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.TimeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WebApiWrapper.TimeManagement;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class TimeFastBookingViewModel : ViewModelBase
    {
        public TimeFastBookingViewModel()
        {
            if (Globals.ActiveUser.RefEmployeeId != 0)
            {
                Validate();
            }
            LoginCommand = new DelegateCommand(() => CreateNewBooking(TimeBookingType.Login), () => AllowLogin);
            StartBreakCommand = new DelegateCommand(() => CreateNewBooking(TimeBookingType.StartBreak), () => AllowStartBreak);
            EndBreakCommand = new DelegateCommand(() => CreateNewBooking(TimeBookingType.EndBreak), () => AllowEndBreak);
            LogoutCommand = new DelegateCommand(() => CreateNewBooking(TimeBookingType.Logout), () => AllowLogout);
        }

        private void Validate()
        {
            var allowedActions = bookingManager.GetAllowedActionsList(DateTime.Now, Globals.ActiveUser.RefEmployeeId);

            AllowLogin = allowedActions.Contains(TimeBookingType.Login);
            AllowStartBreak = allowedActions.Contains(TimeBookingType.StartBreak);
            AllowEndBreak = allowedActions.Contains(TimeBookingType.EndBreak);
            AllowLogout = allowedActions.Contains(TimeBookingType.Logout);
        }

        public bool AllowLogin { get; set; }
        public bool AllowStartBreak { get; set; }
        public bool AllowEndBreak { get; set; }
        public bool AllowLogout { get; set; }
        BookingManager bookingManager = new BookingManager();
        Timer timer = new Timer();

        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand StartBreakCommand { get; set; }
        public DelegateCommand EndBreakCommand { get; set; }
        public DelegateCommand LogoutCommand { get; set; }

        private void CreateNewBooking(TimeBookingType timeBookingType)
        {
            TimeBooking timeBooking = new TimeBooking();
            timeBooking.BookingTime = DateTime.Now;
            timeBooking.TimeStamp = DateTime.Now;
            timeBooking.TimeBookingType = timeBookingType;
            timeBooking.RefEmployeeId = Globals.ActiveUser.RefEmployeeId;
            TimeBookings.Insert(timeBooking);
            Validate();
        }
    }
}
