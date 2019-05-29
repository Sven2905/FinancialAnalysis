using FinancialAnalysis.Models.TimeManagement;
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
