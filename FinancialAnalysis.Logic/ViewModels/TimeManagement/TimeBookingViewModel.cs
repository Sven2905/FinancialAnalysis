using DevExpress.Mvvm;
using FinancialAnalysis.Logic.Manager;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.ProjectManagement;
using FinancialAnalysis.Models.TimeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;
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
            CreateNewBookingCommand = new DelegateCommand(CreateNewBooking, () => bookingManager.ValidateBooking(SelectedTimeBooking, RefEmployeeId)); 
        }

        #endregion Constructor

        #region Fields

        private DateTime _BookingTime = DateTime.Now;
        private BookingManager bookingManager = new BookingManager();

        #endregion Fields

        #region Methods

        private void CreateNewBooking()
        {
            SelectedTimeBooking.TimeStamp = DateTime.Now;
            SelectedTimeBooking.RefEmployeeId = RefEmployeeId;
            TimeBookings.Insert(SelectedTimeBooking);
        }

        private void GetData()
        {
            ProjectList = Projects.GetAll().ToSvenTechCollection();
        }

        private void LoadTimeBookingsForDay()
        {
            BookingsForSelectedDay = TimeBookings.GetDataForDay(_BookingTime, RefEmployeeId);
        }

        #endregion Methods

        #region Properties

        public DateTime BookingTime
        {
            get { return _BookingTime; }
            set { _BookingTime = value; RaisePropertyChanged(); LoadTimeBookingsForDay(); }
        }

        private int _RefEmployeeId;

        public int RefEmployeeId
        {
            get { return _RefEmployeeId; }
            set { _RefEmployeeId = value; RaisePropertyChanged(); LoadTimeBookingsForDay(); }
        }
          
        public TimeBooking SelectedTimeBooking { get; set; } = new TimeBooking();
        public List<TimeBooking> BookingsForSelectedDay { get; set; } = new List<TimeBooking>();
        public SvenTechCollection<Project> ProjectList { get; private set; }
        public SvenTechCollection<TimeBooking> TimeBookingList { get; set; }
        public DelegateCommand CreateNewBookingCommand { get; set; }

        #endregion Properties
    }
}
