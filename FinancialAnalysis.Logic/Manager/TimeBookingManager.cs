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

            var date = timeBooking.BookingTime.Date;
            var allBookings = TimeBookings.GetDataSinceDayForRefEmployeeId(timeBooking.BookingTime, timeBooking.RefEmployeeId);

            while (date.Date < DateTime.Now.Date)
            {
                var booking = allBookings.SingleOrDefault(x => x.BookingTime.Date == date.Date && x.TimeBookingType == TimeBookingType.Logout);

                if (booking != null)
                    SaveBalance(booking);
                else
                    SaveBalanceWithoutBookings(date, timeBooking.RefEmployeeId);

                date = date.AddDays(1);
            }

            return true;
        }

        private void SaveBalance(TimeBooking timeBooking)
        {
            var bookings = TimeBookings.GetDataForDay(timeBooking.BookingTime, timeBooking.RefEmployeeId);
            var hours = timeBooking.BookingTime - bookings.First().BookingTime;

            var obligatoryHours = TimeObligatoryHours.GetByRefEmployeeId(timeBooking.RefEmployeeId);
            if (obligatoryHours.Count > 0)
            {
                var day = obligatoryHours.SingleOrDefault(x => x.DayOfWeek == timeBooking.BookingTime.DayOfWeek);
                if (day != null)
                {
                    var dayItem = CreateDayItem(bookings, day);
                    var balanceValue = dayItem.WorkingHours.TotalHours - day.HoursPerDay;

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

        public void SaveBalanceWithoutBookings(DateTime dateTime, int refEmployeeId)
        {
            var lastBalance = TimeBalances.GetLastByDateAndRefEmployeeId(dateTime, refEmployeeId);
            var obligatoryHours = TimeObligatoryHours.GetByRefEmployeeId(refEmployeeId);

            if (lastBalance != null)
            {
                if (obligatoryHours != null)
                {
                    var obligatoryHoursDay = obligatoryHours.Single(x => x.DayOfWeek == dateTime.DayOfWeek).HoursPerDay;
                    var existingItem = TimeBalances.GetByDateAndRefEmployeeId(dateTime, refEmployeeId);

                    if (existingItem != null)
                    {
                        existingItem.Balance = lastBalance.Balance - obligatoryHoursDay;
                        TimeBalances.Update(existingItem);
                    }
                    else
                    {
                    TimeBalances.Insert(new TimeBalance(refEmployeeId, dateTime, lastBalance.Balance - obligatoryHoursDay));
                    }
                }
            }
            else
            {
                if (obligatoryHours != null)
                {
                    var obligatoryHoursDay = obligatoryHours.Single(x => x.DayOfWeek == dateTime.DayOfWeek).HoursPerDay;
                    TimeBalances.Insert(new TimeBalance(refEmployeeId, dateTime, (obligatoryHoursDay * (-1))));
                }
            }
        }


        public IEnumerable<TimeBookingDayItem> GetBookingItemsForMonth(DateTime dateTime, int refEmployeeId)
        {
            List<TimeBooking> bookingsForMonth = TimeBookings.GetDataForMonth(dateTime, refEmployeeId);

            var timeBookingDayItems = CreateDayItems(bookingsForMonth).ToList();
            var startDate = new DateTime(dateTime.Year, dateTime.Month, 1);

            while (startDate.Month == dateTime.Month && startDate.Date <= DateTime.Now.Date)
            {
                if (!timeBookingDayItems.Any(x => x.BookingDate.Date == startDate.Date))
                {
                    TimeBookingDayItem timeBookingDayItem = new TimeBookingDayItem();
                    timeBookingDayItem.BookingDate = startDate;
                    timeBookingDayItem.WorkingHours = new TimeSpan(0);
                    timeBookingDayItem.BreaktimeHours = new TimeSpan(0);
                    timeBookingDayItem.ObligatoryHours = GetObgligaryHoursForDay(startDate, refEmployeeId);

                    var balanceForDay = TimeBalances.GetByDateAndRefEmployeeId(startDate, refEmployeeId);

                    if (balanceForDay != null)
                        timeBookingDayItem.Balance = balanceForDay.Balance;
                    else
                    {
                        var lastBalance = TimeBalances.GetLastByDateAndRefEmployeeId(startDate, refEmployeeId);
                        if (lastBalance != null)
                            timeBookingDayItem.Balance = lastBalance.Balance;
                        else
                            timeBookingDayItem.Balance = 0;
                    }

                    timeBookingDayItems.Add(timeBookingDayItem);
                }

                startDate = startDate.AddDays(1);
            }

            return timeBookingDayItems.OrderBy(x => x.BookingDate);
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
            TimeSpan sumWorkingTime = new TimeSpan(0);
            TimeSpan sumBreakTime = new TimeSpan(0);

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
                            sumWorkingTime += logout.Subtract(login);
                        break;
                    case TimeBookingType.StartBreak:
                        startBreak = timeBookingList[i].BookingTime;
                        break;
                    case TimeBookingType.EndBreak:
                        endBreak = timeBookingList[i].BookingTime;
                        if (logout > login)
                            sumBreakTime += logout.Subtract(login);
                        break;
                }
            }

            TimeBookingDayItem timeBookingDayItem = CorrectWorkingTime(sumWorkingTime, sumBreakTime);
            timeBookingDayItem.BookingDate = timeBookingList[0].BookingTime.Date;
            timeBookingDayItem.ObligatoryHours = timeObligatoryHour.HoursPerDay;

            var timeBalance = TimeBalances.GetByDateAndRefEmployeeId(timeBookingList[0].BookingTime, timeBookingList[0].RefEmployeeId);
            if (timeBalance != null)
                timeBookingDayItem.Balance = timeBalance.Balance;

            return timeBookingDayItem;
        }

        private TimeBookingDayItem CorrectWorkingTime(TimeSpan sumWorkingTime, TimeSpan sumBreakTime)
        {
            TimeBookingDayItem timeBookingDayItem = new TimeBookingDayItem();

            if (sumWorkingTime.TotalHours >= 9.25 && sumBreakTime.TotalHours >= 0.75)
            {
                timeBookingDayItem.WorkingHours = sumWorkingTime;
                timeBookingDayItem.BreaktimeHours = sumBreakTime;
                return timeBookingDayItem;
            }
            else if (sumWorkingTime.TotalHours >= 9.25 && sumBreakTime.TotalHours < 0.75)
            {
                var breakDifference = TimeSpan.FromHours(0.75).Subtract(timeBookingDayItem.BreaktimeHours);
                timeBookingDayItem.WorkingHours = sumWorkingTime.Subtract(breakDifference);
                timeBookingDayItem.BreaktimeHours = TimeSpan.FromHours(0.75);
                return timeBookingDayItem;
            }
            else if (sumWorkingTime.TotalHours >= 9)
            {
                var differenceWorkingTime = sumWorkingTime - TimeSpan.FromHours(9);
                if (sumBreakTime >= TimeSpan.FromHours(0.5) + differenceWorkingTime)
                {
                    timeBookingDayItem.WorkingHours = sumWorkingTime;
                    timeBookingDayItem.BreaktimeHours = sumBreakTime;
                }
                else
                {
                    var breakDifference = (TimeSpan.FromHours(0.5) + differenceWorkingTime).Subtract(timeBookingDayItem.BreaktimeHours);
                    timeBookingDayItem.WorkingHours = sumWorkingTime - differenceWorkingTime;
                    timeBookingDayItem.BreaktimeHours = TimeSpan.FromHours(0.5) + breakDifference;
                }
                return timeBookingDayItem;
            }
            else if (sumWorkingTime.TotalHours >= 6.5 && sumBreakTime.TotalHours >= 0.5)
            {
                timeBookingDayItem.WorkingHours = sumWorkingTime;
                timeBookingDayItem.BreaktimeHours = sumBreakTime;
                return timeBookingDayItem;
            }
            else if (sumWorkingTime.TotalHours >= 6.5 && sumBreakTime.TotalHours < 0.5)
            {
                var breakDifference = TimeSpan.FromHours(0.5).Subtract(timeBookingDayItem.BreaktimeHours);
                timeBookingDayItem.WorkingHours = sumWorkingTime.Subtract(breakDifference);
                timeBookingDayItem.BreaktimeHours = TimeSpan.FromHours(0.5);
                return timeBookingDayItem;
            }
            else if (sumWorkingTime.TotalHours >= 6)
            {
                var differenceWorkingTime = sumWorkingTime - TimeSpan.FromHours(6);
                if (sumBreakTime >= differenceWorkingTime)
                {
                    timeBookingDayItem.WorkingHours = sumWorkingTime;
                    timeBookingDayItem.BreaktimeHours = sumBreakTime;
                }
                else
                {
                    var breakDifference = differenceWorkingTime.Subtract(timeBookingDayItem.BreaktimeHours);
                    timeBookingDayItem.WorkingHours = sumWorkingTime - differenceWorkingTime;
                    timeBookingDayItem.BreaktimeHours = breakDifference;
                }
                return timeBookingDayItem;
            }
            else
            {
                timeBookingDayItem.WorkingHours = sumWorkingTime;
                timeBookingDayItem.BreaktimeHours = sumBreakTime;
                return timeBookingDayItem;
            }
        }

        public double GetObgligaryHoursForDay(DateTime dateTime, int refEmployeeId)
        {
            var obligatoryHours = TimeObligatoryHours.GetByRefEmployeeId(refEmployeeId);
            if (obligatoryHours == null || obligatoryHours.Count == 0 || obligatoryHours.SingleOrDefault(x => x.DayOfWeek == dateTime.DayOfWeek) == null)
                return 0;

            return obligatoryHours.Single(x => x.DayOfWeek == dateTime.DayOfWeek).HoursPerDay;
        }
    }
}
