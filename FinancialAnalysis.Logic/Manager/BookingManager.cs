using FinancialAnalysis.Models;
using FinancialAnalysis.Models.TimeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApiWrapper.TimeManagement;

namespace FinancialAnalysis.Logic.Manager
{
    public class TimeBookingManager
    {
        public bool ValidateBooking(TimeBooking timeBooking)
        {
            if (timeBooking.RefEmployeeId == 0)
                return false;

            List<TimeBooking> bookingsForSelectedDay = TimeBookings.GetDataForDay(timeBooking.BookingTime, timeBooking.RefEmployeeId);

            if (bookingsForSelectedDay?.Count > 0)
            {
                if (bookingsForSelectedDay.Last().TimeBookingType == timeBooking.TimeBookingType)
                    return false;
                if (bookingsForSelectedDay.Last().TimeBookingType == TimeBookingType.Login && (timeBooking.TimeBookingType != TimeBookingType.StartBreak && timeBooking.TimeBookingType != TimeBookingType.Logout))
                    return false;
                if (bookingsForSelectedDay.Last().TimeBookingType == TimeBookingType.StartBreak && timeBooking.TimeBookingType != TimeBookingType.EndBreak)
                    return false;
                if (bookingsForSelectedDay.Last().TimeBookingType == TimeBookingType.EndBreak && (timeBooking.TimeBookingType != TimeBookingType.StartBreak && timeBooking.TimeBookingType != TimeBookingType.Logout))
                    return false;
            }
            else
            {
                if (timeBooking.TimeBookingType != TimeBookingType.Login)
                    return false;
            }

            return true;
        }

        public List<TimeBookingType> GetAllowedActionsList(DateTime dateTime, int refEmployeeId)
        {
            List<TimeBookingType> allowedActions = new List<TimeBookingType>();
            List<TimeBooking> bookingsForSelectedDay = TimeBookings.GetDataForDay(dateTime, refEmployeeId);

            if (bookingsForSelectedDay.Count > 0)
            {
                if (bookingsForSelectedDay.Last().TimeBookingType == TimeBookingType.Login)
                {
                    allowedActions.Add(TimeBookingType.StartBreak);
                    allowedActions.Add(TimeBookingType.Logout);
                }
                if (bookingsForSelectedDay.Last().TimeBookingType == TimeBookingType.StartBreak)
                {
                    allowedActions.Add(TimeBookingType.EndBreak);
                }
                if (bookingsForSelectedDay.Last().TimeBookingType == TimeBookingType.EndBreak)
                {
                    allowedActions.Add(TimeBookingType.StartBreak);
                    allowedActions.Add(TimeBookingType.Logout);
                }
                if (bookingsForSelectedDay.Last().TimeBookingType == TimeBookingType.Logout)
                {
                    allowedActions.Add(TimeBookingType.Login);
                }
            }
            else
                allowedActions.Add(TimeBookingType.Login);

            return allowedActions;
        }

        public bool SaveTimeBooking(TimeBooking timeBooking)
        {
            if (!ValidateBooking(timeBooking))
                return false;

            TimeBookings.Insert(timeBooking);

            if (timeBooking.TimeBookingType == TimeBookingType.Logout)
                SaveBalance(timeBooking);

            if (timeBooking.BookingTime < DateTime.Now)
            {
                var allBookings = TimeBookings.GetDataSinceDayForRefEmployeeId(timeBooking.BookingTime, timeBooking.RefEmployeeId);

                foreach (TimeBooking item in allBookings)
                {
                    if (item.TimeBookingType == TimeBookingType.Logout)
                        SaveBalance(item);
                }
            }

            return true;
        }

        private void SaveBalance(TimeBooking timeBooking)
        {
            TimeSpan nettoWorkingTime;

            var bookings = TimeBookings.GetDataForDay(timeBooking.BookingTime, timeBooking.RefEmployeeId);
            var hours = timeBooking.BookingTime - bookings.First().BookingTime;

            if (bookings.Any(x => x.TimeBookingType == TimeBookingType.StartBreak))
            {
                var breakTime = bookings.First(x => x.TimeBookingType == TimeBookingType.StartBreak).BookingTime - bookings.First(x => x.TimeBookingType == TimeBookingType.EndBreak).BookingTime;
                nettoWorkingTime = CalculateNettoWorkingTime(hours, breakTime); // TODO Change to new method
            }
            else
            {
                nettoWorkingTime = CalculateNettoWorkingTime(hours, new TimeSpan(0));
            }

            var obligatoryHours = TimeObligatoryHours.GetByRefEmployeeId(timeBooking.RefEmployeeId);
            if (obligatoryHours.Count > 0)
            {
                var day = obligatoryHours.SingleOrDefault(x => x.DayOfWeek == timeBooking.BookingTime.DayOfWeek);
                if (day != null)
                {
                    var balanceValue = nettoWorkingTime.TotalHours - day.HoursPerDay;

                    var lastItem = TimeBalances.GetLastByDateAndRefEmployeeId(timeBooking.BookingTime, timeBooking.RefEmployeeId);

                    if (lastItem != null)
                        balanceValue += lastItem.Balance;

                    var existingItem = TimeBalances.GetByDateAndRefEmployeeId(timeBooking.BookingTime, timeBooking.RefEmployeeId);

                    if (existingItem != null)
                    {
                        existingItem.Balance = balanceValue;
                        TimeBalances.Update(existingItem);
                    }
                    else
                    {
                        TimeBalance timeBalance = new TimeBalance(timeBooking.RefEmployeeId, timeBooking.BookingTime, balanceValue);
                        TimeBalances.Insert(timeBalance);
                    }
                }
            }
        }

        [Obsolete("Use new method", false)]
        private TimeSpan CalculateNettoWorkingTime(TimeSpan WorkingTime, TimeSpan BreakTime)
        {
            if (WorkingTime.TotalHours >= 9.25 && BreakTime.TotalMinutes >= 45)
            {
                return WorkingTime - BreakTime;
            }
            else if (WorkingTime.TotalHours >= 9.25 && BreakTime.TotalMinutes < 45)
            {
                return WorkingTime - new TimeSpan(0, 45, 0);
            }
            else if (WorkingTime.TotalHours >= 9 && WorkingTime.TotalHours < 9.25 && BreakTime.TotalMinutes < 45)
            {
                var temp = WorkingTime - new TimeSpan(9, 0, 0);
                if (temp + new TimeSpan(0, 30, 0) < BreakTime)
                {
                    return WorkingTime - BreakTime;
                }
                return WorkingTime - (temp + new TimeSpan(0, 30, 0));
            }
            else if (WorkingTime.TotalHours >= 6.5 && BreakTime.TotalMinutes >= 30)
            {
                return WorkingTime - BreakTime;
            }
            else if (WorkingTime.TotalHours >= 6.5 && BreakTime.Minutes < 30)
            {
                return WorkingTime - new TimeSpan(0, 30, 0);
            }
            else if (WorkingTime.TotalHours >= 6)
            {
                var temp = WorkingTime - new TimeSpan(6, 0, 0);
                if (temp < BreakTime)
                {
                    return WorkingTime - BreakTime;
                }
                return WorkingTime - temp;
            }
            else
            {
                return WorkingTime;
            }
        }

        public IEnumerable<TimeBookingDayItem> GetBookingItemsForMonth(DateTime dateTime, int refEmployeeId)
        {
            List<TimeBooking> bookingsForMonth = TimeBookings.GetDataForMonth(dateTime, refEmployeeId);

            return CreateDayItems(bookingsForMonth);
        }

        private IEnumerable<TimeBookingDayItem> CreateDayItems(IEnumerable<TimeBooking> timeBookings)
        {
            if (!timeBookings.Any())
                return new List<TimeBookingDayItem>();

            List<TimeBookingDayItem> timeBookingDayItems = new List<TimeBookingDayItem>();

            var timeObligatoryHours = TimeObligatoryHours.GetByRefEmployeeId(timeBookings.ToList()[0].RefEmployeeId);

            var itemsPerDay = timeBookings.GroupBy(x => x.BookingTime.Date);
            foreach (var item in itemsPerDay)
            {
                var timeObgligatoryHoursDay = timeObligatoryHours.SingleOrDefault(x => x.DayOfWeek == item.ToList()[0].BookingTime.DayOfWeek);
                var newDayItem = CreateDayItem(item, timeObgligatoryHoursDay);
                if (newDayItem != null)
                    timeBookingDayItems.Add(newDayItem);
            }

            return timeBookingDayItems;
        }

        private TimeBookingDayItem CreateDayItem(IEnumerable<TimeBooking> timeBookings, TimeObligatoryHour timeObligatoryHour)
        {
            List<TimeBooking> timeBookingList = timeBookings.ToList();
            if (!timeBookings.Any(x => x.TimeBookingType == TimeBookingType.Logout))
                return null;

            DateTime login = DateTime.MinValue;
            DateTime logout = DateTime.MinValue;
            DateTime startBreak = DateTime.MinValue;
            DateTime endBreak = DateTime.MinValue;
            double sumWorkingTime = 0;
            double sumBreakTime = 0;

            for (int i = 0; i < timeBookingList.Count; i++)
            {
                switch (timeBookingList[i].TimeBookingType)
                {
                    case TimeBookingType.Login:
                        login = timeBookingList[i].BookingTime;
                        break;
                    case TimeBookingType.Logout:
                        logout = timeBookingList[i].BookingTime;
                        if (logout > login)
                            sumWorkingTime += logout.Subtract(login).TotalHours;
                        break;
                    case TimeBookingType.StartBreak:
                        startBreak = timeBookingList[i].BookingTime;
                        break;
                    case TimeBookingType.EndBreak:
                        endBreak = timeBookingList[i].BookingTime;
                        if (logout > login)
                            sumBreakTime += logout.Subtract(login).TotalHours;
                        break;
                }
            }

            TimeBookingDayItem timeBookingDayItem = CheckBreaktime(sumWorkingTime, sumBreakTime);
            timeBookingDayItem.BookingDate = timeBookingList[0].BookingTime.Date;
            timeBookingDayItem.ObligatoryHours = timeObligatoryHour.HoursPerDay;

            var timeBalance = TimeBalances.GetByDateAndRefEmployeeId(timeBookingList[0].BookingTime, timeBookingList[0].RefEmployeeId);
            if (timeBalance != null)
                timeBookingDayItem.Balance = timeBalance.Balance;

            return timeBookingDayItem;
        }

        private TimeBookingDayItem CheckBreaktime(double sumWorkingTime, double sumBreakTime)
        {
            TimeBookingDayItem timeBookingDayItem = new TimeBookingDayItem();

            if (sumWorkingTime >= 9.25 && sumBreakTime >= 0.75)
            {
                timeBookingDayItem.WorkingHours = TimeSpan.FromHours(sumWorkingTime);
                timeBookingDayItem.BreaktimeHours = TimeSpan.FromHours(sumBreakTime);
                return timeBookingDayItem;
            }
            else if (sumWorkingTime >= 9.25 && sumBreakTime < 0.75)
            {
                var breakDifference = TimeSpan.FromHours(0.75).Subtract(timeBookingDayItem.BreaktimeHours);
                timeBookingDayItem.WorkingHours = TimeSpan.FromHours(sumWorkingTime).Subtract(breakDifference);
                timeBookingDayItem.BreaktimeHours = TimeSpan.FromHours(0.75);
                return timeBookingDayItem;
            }
            else if (sumWorkingTime >= 9)
            {
                var differenceWorkingTime = sumWorkingTime - 9;
                if (sumBreakTime >= 0.5 + differenceWorkingTime)
                {
                    timeBookingDayItem.WorkingHours = TimeSpan.FromHours(sumWorkingTime);
                    timeBookingDayItem.BreaktimeHours = TimeSpan.FromHours(sumBreakTime);
                }
                else
                {
                    var breakDifference = TimeSpan.FromHours(0.5 + differenceWorkingTime).Subtract(timeBookingDayItem.BreaktimeHours);
                    timeBookingDayItem.WorkingHours = TimeSpan.FromHours(sumWorkingTime - differenceWorkingTime);
                    timeBookingDayItem.BreaktimeHours = TimeSpan.FromHours(0.5) + breakDifference;
                }
                return timeBookingDayItem;
            }
            else if (sumWorkingTime >= 6.5 && sumBreakTime >= 0.5)
            {
                timeBookingDayItem.WorkingHours = TimeSpan.FromHours(sumWorkingTime);
                timeBookingDayItem.BreaktimeHours = TimeSpan.FromHours(sumBreakTime);
                return timeBookingDayItem;
            }
            else if (sumWorkingTime >= 6.5 && sumBreakTime < 0.5)
            {
                var breakDifference = TimeSpan.FromHours(0.5).Subtract(timeBookingDayItem.BreaktimeHours);
                timeBookingDayItem.WorkingHours = TimeSpan.FromHours(sumWorkingTime).Subtract(breakDifference);
                timeBookingDayItem.BreaktimeHours = TimeSpan.FromHours(0.5);
                return timeBookingDayItem;
            }
            else if (sumWorkingTime >= 6)
            {
                var differenceWorkingTime = sumWorkingTime - 6;
                if (sumBreakTime >= differenceWorkingTime)
                {
                    timeBookingDayItem.WorkingHours = TimeSpan.FromHours(sumWorkingTime);
                    timeBookingDayItem.BreaktimeHours = TimeSpan.FromHours(sumBreakTime);
                }
                else
                {
                    var breakDifference = TimeSpan.FromHours(differenceWorkingTime).Subtract(timeBookingDayItem.BreaktimeHours);
                    timeBookingDayItem.WorkingHours = TimeSpan.FromHours(sumWorkingTime - differenceWorkingTime);
                    timeBookingDayItem.BreaktimeHours = TimeSpan.FromHours(differenceWorkingTime);
                }
                return timeBookingDayItem;
            }
            else
            {
                timeBookingDayItem.WorkingHours = TimeSpan.FromHours(sumWorkingTime);
                timeBookingDayItem.BreaktimeHours = TimeSpan.FromHours(sumBreakTime);
                return timeBookingDayItem;
            }
        }
    }
}
