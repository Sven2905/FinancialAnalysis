using FinancialAnalysis.Models.TimeManagement;
using System;
using System.Collections.Generic;

namespace WebApiWrapper.TimeManagement
{
    public static class TimeBookings
    {
        private const string controllerName = "TimeBookings";

        public static List<TimeBooking> GetAll()
        {
            return WebApi<List<TimeBooking>>.GetData(controllerName);
        }

        public static List<TimeBooking> GetDataForDay(DateTime Date, int RefEmployeeId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "date", Date },
                { "refEmployeeId", RefEmployeeId },
            };

            return WebApi<List<TimeBooking>>.GetData(controllerName, "GetDataForDay", parameters);
        }

        public static List<TimeBooking> GetDataForMonth(DateTime date, int refEmployeeId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "date", date },
                { "refEmployeeId", refEmployeeId },
            };

            return WebApi<List<TimeBooking>>.GetData(controllerName, "GetDataForMonth", parameters);
        }

        public static List<TimeBooking> GetDataSinceDayForRefEmployeeId(DateTime date, int refEmployeeId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "date", date },
                { "refEmployeeId", refEmployeeId },
            };

            return WebApi<List<TimeBooking>>.GetData(controllerName, "GetDataSinceDayForRefEmployeeId", parameters);
        }

        public static TimeBooking GetById(int id)
        {
            return WebApi<TimeBooking>.GetDataById(controllerName, id);
        }

        public static int Insert(TimeBooking TimeBooking)
        {
            return WebApi<int>.PostAsync(controllerName, TimeBooking, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<TimeBooking> TimeBookings)
        {
            return WebApi<int>.PostAsync(controllerName, TimeBookings, "MultiPost").Result;
        }

        public static bool Update(TimeBooking TimeBooking)
        {
            return WebApi<bool>.PutAsync(controllerName, TimeBooking, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
