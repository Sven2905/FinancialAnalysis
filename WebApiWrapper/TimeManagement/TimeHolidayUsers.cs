using FinancialAnalysis.Models.TimeManagement;
using System.Collections.Generic;

namespace WebApiWrapper.TimeManagement
{
    public static class TimeHolidayUsers
    {
        private const string controllerName = "TimeHolidayUsers";

        public static List<TimeHolidayUser> GetAll()
        {
            return WebApi<List<TimeHolidayUser>>.GetData(controllerName);
        }

        public static TimeHolidayUser GetById(int id)
        {
            return WebApi<TimeHolidayUser>.GetDataById(controllerName, id);
        }

        public static int Insert(TimeHolidayUser TimeHolidayUser)
        {
            return WebApi<int>.PostAsync(controllerName, TimeHolidayUser, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<TimeHolidayUser> TimeHolidayUsers)
        {
            return WebApi<int>.PostAsync(controllerName, TimeHolidayUsers, "MultiPost").Result;
        }

        public static bool Update(TimeHolidayUser TimeHolidayUser)
        {
            return WebApi<bool>.PutAsync(controllerName, TimeHolidayUser, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}