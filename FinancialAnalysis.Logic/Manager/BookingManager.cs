using FinancialAnalysis.Models;
using FinancialAnalysis.Models.TimeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApiWrapper.TimeManagement;

namespace FinancialAnalysis.Logic.Manager
{
    public class BookingManager
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
                nettoWorkingTime = CalculateNettoWorkingTime(hours, breakTime);
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

    }
}
