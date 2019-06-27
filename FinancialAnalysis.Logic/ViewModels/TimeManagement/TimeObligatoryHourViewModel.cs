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
        private List<Models.TimeManagement.TimeObligatoryHour> _TimeObligatoryHours = new List<Models.TimeManagement.TimeObligatoryHour>();

        #endregion Fields

        #region Methods

        private void LoadData()
        {
            if (EmployeeId == 0)
                return;

            _TimeObligatoryHours = WebApiWrapper.TimeManagement.TimeObligatoryHours.GetByRefEmployeeId(EmployeeId);

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
                    new Models.TimeManagement.TimeObligatoryHour(EmployeeId, HoursMonday, DayOfWeek.Monday),
                    new Models.TimeManagement.TimeObligatoryHour(EmployeeId, HoursTuesday, DayOfWeek.Tuesday),
                    new Models.TimeManagement.TimeObligatoryHour(EmployeeId, HoursWednesday, DayOfWeek.Wednesday),
                    new Models.TimeManagement.TimeObligatoryHour(EmployeeId, HoursThursday, DayOfWeek.Thursday),
                    new Models.TimeManagement.TimeObligatoryHour(EmployeeId, HoursFriday, DayOfWeek.Friday),
                    new Models.TimeManagement.TimeObligatoryHour(EmployeeId, HoursSaturday, DayOfWeek.Saturday),
                    new Models.TimeManagement.TimeObligatoryHour(EmployeeId, HoursSunday, DayOfWeek.Sunday),
                };

                WebApiWrapper.TimeManagement.TimeObligatoryHours.Insert(itemsToSave);

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
