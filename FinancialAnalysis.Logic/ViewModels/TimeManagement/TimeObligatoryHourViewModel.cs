using DevExpress.Mvvm;
using FinancialAnalysis.Models.TimeManagement;
using System;
using System.Collections.Generic;
using WebApiWrapper.TimeManagement;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class TimeObligatoryHourViewModel : ViewModelBase
    {
        #region Constructor

        public TimeObligatoryHourViewModel()
        {
            SaveCommand = new DelegateCommand(SaveData, () => EmployeeId > 0);
        }

        #endregion Constructor

        #region Fields

        private int _EmployeeId;
        private List<TimeObligatoryHour> _TimeObligatoryHours = new List<TimeObligatoryHour>();

        #endregion Fields

        #region Methods

        private void LoadData()
        {
            if (EmployeeId == 0)
                return;

            _TimeObligatoryHours = TimeObligatoryHours.GetByRefEmployeeId(EmployeeId);

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
                    TimeObligatoryHours.Update(item);
                }
            }
            else
            {
                List<TimeObligatoryHour> itemsToSave = new List<TimeObligatoryHour>()
                {
                    new TimeObligatoryHour(EmployeeId, HoursMonday, DayOfWeek.Monday),
                    new TimeObligatoryHour(EmployeeId, HoursTuesday, DayOfWeek.Tuesday),
                    new TimeObligatoryHour(EmployeeId, HoursWednesday, DayOfWeek.Wednesday),
                    new TimeObligatoryHour(EmployeeId, HoursThursday, DayOfWeek.Thursday),
                    new TimeObligatoryHour(EmployeeId, HoursFriday, DayOfWeek.Friday),
                    new TimeObligatoryHour(EmployeeId, HoursSaturday, DayOfWeek.Saturday),
                    new TimeObligatoryHour(EmployeeId, HoursSunday, DayOfWeek.Sunday),
                };

                TimeObligatoryHours.Insert(itemsToSave);

                LoadData();
            }
        }

        #endregion Methods

        #region Properties

        public int EmployeeId
        {
            get { return _EmployeeId; }
            set { _EmployeeId = value; RaisePropertyChanged(); LoadData(); }
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
