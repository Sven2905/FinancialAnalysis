using FinancialAnalysis.Models;
using FinancialAnalysis.Models.TimeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiWrapper.TimeManagement;

namespace FinancialAnalysis.Logic.Manager
{
    public class BookingManager
    {
        public bool ValidateBooking(TimeBooking timeBooking, int refEmployeeId)
        {
            if (refEmployeeId == 0)
                return false;

            List<TimeBooking> bookingsForSelectedDay = TimeBookings.GetDataForDay(timeBooking.BookingTime, refEmployeeId);

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

    }
}
