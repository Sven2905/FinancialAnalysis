using FinancialAnalysis.Models.TimeManagement;
using System.Collections.Generic;

namespace WebApiWrapper.TimeManagement
{
    public static class TimeHolidayTypes
    {
        private const string controllerName = "TimeHolidayTypes";

        public static List<TimeHolidayType> GetAll()
        {
            return WebApi<List<TimeHolidayType>>.GetData(controllerName);
        }

        public static TimeHolidayType GetById(int id)
        {
            return WebApi<TimeHolidayType>.GetDataById(controllerName, id);
        }

        public static int Insert(TimeHolidayType TimeHolidayType)
        {
            return WebApi<int>.PostAsync(controllerName, TimeHolidayType, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<TimeHolidayType> TimeHolidayTypes)
        {
            return WebApi<int>.PostAsync(controllerName, TimeHolidayTypes, "MultiPost").Result;
        }

        public static bool Update(TimeHolidayType TimeHolidayType)
        {
            return WebApi<bool>.PutAsync(controllerName, TimeHolidayType, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}