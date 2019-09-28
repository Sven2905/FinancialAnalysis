using DevExpress.Mvvm;
using System;
using System.Collections.Generic;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class TimeObligatoryHourViewModel : ViewModelBase
    {
        #region Constructor

        public TimeObligatoryHourViewModel()
        {
            SaveCommand = new DelegateCommand(SaveData, () => UserId > 0);
        }

        #endregion Constructor

        #region Fields

        private int _UserId;
        private List<Models.TimeManagement.TimeObligatoryHour> _TimeObligatoryHours = new List<Models.TimeManagement.TimeObligatoryHour>();

        #endregion Fields

        #region Methods

        private void LoadData()
        {
            if (UserId == 0)
                return;

            _TimeObligatoryHours = WebApiWrapper.TimeManagement.TimeObligatoryHours.GetByRefUserId(UserId);

            if (_TimeObligatoryHours.Count > 0)
            {
                foreach (var item in _TimeObligatoryHours)
                {
                    switch (item.DayOfWeek)
                    {
                        case DayOfWeek.Sunday:
                            HoursSunday = item.HoursPerDay;
                            break;

                        case DayOfWeek.Monday:
                            HoursMonday = item.HoursPerDay;
                            break;

                        case DayOfWeek.Tuesday:
                            HoursTuesday = item.HoursPerDay;
                            break;

                        case DayOfWeek.Wednesday:
                            HoursWednesday = item.HoursPerDay;
                            break;

                        case DayOfWeek.Thursday:
                            HoursThursday = item.HoursPerDay;
                            break;

                        case DayOfWeek.Friday:
                            HoursFriday = item.HoursPerDay;
                            break;

                        case DayOfWeek.Saturday:
                            HoursSaturday = item.HoursPerDay;
                            break;
                    }
                }
            }
        }

        public void SaveData()
        {
            if (_TimeObligatoryHours.Count > 0)
            {
                foreach (var item in _TimeObligatoryHours)
                {
                    switch (item.DayOfWeek)
                    {
                        case DayOfWeek.Sunday:
                            item.HoursPerDay = HoursSunday;
                            break;

                        case DayOfWeek.Monday:
                            item.HoursPerDay = HoursMonday;
                            break;

                        case DayOfWeek.Tuesday:
                            item.HoursPerDay = HoursTuesday;
                            break;

                        case DayOfWeek.Wednesday:
                            item.HoursPerDay = HoursWednesday;
                            break;

                        case DayOfWeek.Thursday:
                            HoursThursday = item.HoursPerDay;
                            break;

                        case DayOfWeek.Friday:
                            item.HoursPerDay = HoursFriday;
                            break;

                        case DayOfWeek.Saturday:
                            item.HoursPerDay = HoursSaturday;
                            break;
                    }
                    WebApiWrapper.TimeManagement.TimeObligatoryHours.Update(item);
                }
            }
            else
            {
                List<Models.TimeManagement.TimeObligatoryHour> itemsToSave = new List<Models.TimeManagement.TimeObligatoryHour>()
                {
                    new Models.TimeManagement.TimeObligatoryHour(UserId, HoursMonday, DayOfWeek.Monday),
                    new Models.TimeManagement.TimeObligatoryHour(UserId, HoursTuesday, DayOfWeek.Tuesday),
                    new Models.TimeManagement.TimeObligatoryHour(UserId, HoursWednesday, DayOfWeek.Wednesday),
                    new Models.TimeManagement.TimeObligatoryHour(UserId, HoursThursday, DayOfWeek.Thursday),
                    new Models.TimeManagement.TimeObligatoryHour(UserId, HoursFriday, DayOfWeek.Friday),
                    new Models.TimeManagement.TimeObligatoryHour(UserId, HoursSaturday, DayOfWeek.Saturday),
                    new Models.TimeManagement.TimeObligatoryHour(UserId, HoursSunday, DayOfWeek.Sunday),
                };

                WebApiWrapper.TimeManagement.TimeObligatoryHours.Insert(itemsToSave);

                LoadData();
            }
        }

        #endregion Methods

        #region Properties

        public int UserId
        {
            get { return _UserId; }
            set { _UserId = value; RaisePropertyChanged(); LoadData(); }
        }

        public DelegateCommand SaveCommand { get; set; }
        public double HoursMonday { get; set; }
        public double HoursTuesday { get; set; }
        public double HoursWednesday { get; set; }
        public double HoursThursday { get; set; }
        public double HoursFriday { get; set; }
        public double HoursSaturday { get; set; }
        public double HoursSunday { get; set; }
        public double HoursSum => HoursMonday + HoursTuesday + HoursWednesday + HoursThursday + HoursFriday + HoursSaturday + HoursSunday;

        #endregion Properties
    }
}