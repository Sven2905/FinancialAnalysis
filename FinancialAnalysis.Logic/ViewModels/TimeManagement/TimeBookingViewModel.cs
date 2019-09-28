using DevExpress.Mvvm;
using FinancialAnalysis.Logic.Manager;
using FinancialAnalysis.Models.ProjectManagement;
using FinancialAnalysis.Models.TimeManagement;
using System;
using System.Collections.Generic;
using Utilities;
using WebApiWrapper.Administration;
using WebApiWrapper.ProjectManagement;
using WebApiWrapper.TimeManagement;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class TimeBookingViewModel : ViewModelBase
    {
        #region Constructor

        public TimeBookingViewModel()
        {
            GetData();
            CreateNewBookingCommand = new DelegateCommand(SaveNewBooking, () => bookingManager.ValidateBooking(SelectedTimeBooking));
        }

        #endregion Constructor

        #region Fields

        private DateTime bookingTime = DateTime.Now;
        private readonly TimeBookingManager bookingManager = new TimeBookingManager();
        private int refUserId;
        private TimeBooking selectedTimeBooking = new TimeBooking();

        #endregion Fields

        #region Methods

        private void SaveNewBooking()
        {
            SelectedTimeBooking.TimeStamp = DateTime.Now;
            bookingManager.SaveTimeBooking(SelectedTimeBooking);
        }

        private void GetData()
        {
            ProjectList = Projects.GetAll().ToSvenTechCollection();
        }

        private void LoadTimeBookingsForDay()
        {
            BookingsForSelectedDay = TimeBookings.GetDataForDay(bookingTime, RefUserId);
        }

        #endregion Methods

        #region Properties

        public DateTime BookingTime
        {
            get { return bookingTime; }
            set { bookingTime = value; RaisePropertyChanged(); LoadTimeBookingsForDay(); }
        }

        public int RefUserId
        {
            get { return refUserId; }
            set
            {
                refUserId = value;
                SelectedTimeBooking.RefUserId = value;
                RaisePropertyChanged();
                LoadTimeBookingsForDay();
            }
        }

        public TimeBooking SelectedTimeBooking
        {
            get { return selectedTimeBooking; }
            set { selectedTimeBooking = value; RaisePropertyChanged(); }
        }

        public List<TimeBooking> BookingsForSelectedDay { get; set; } = new List<TimeBooking>();
        public SvenTechCollection<Project> ProjectList { get; private set; }
        public DelegateCommand CreateNewBookingCommand { get; set; }

        #endregion Properties
    }
}