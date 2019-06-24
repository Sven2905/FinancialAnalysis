using FinancialAnalysis.Models.TimeManagement;
using System;
using System.Collections.Generic;

namespace WebApiWrapper.TimeManagement
{
    public static class TimeBalances
    {
        private const string controllerName = "TimeBalances";

        public static List<TimeBalance> GetAll()
        {
            return WebApi<List<TimeBalance>>.GetData(controllerName);
        }

        public static TimeBalance GetById(int id)
        {
            return WebApi<TimeBalance>.GetDataById(controllerName, id);
        }

        public static TimeBalance GetByDateAndRefEmployeeId(DateTime Date, int RefEmployeeId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "Date", Date },
                { "RefEmployeeId", RefEmployeeId },
            };

            return WebApi<TimeBalance>.GetData(controllerName, "GetByDateAndRefEmployeeId", parameters);
        }

        public static TimeBalance GetLastByDateAndRefEmployeeId(DateTime Date, int RefEmployeeId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "Date", Date },
                { "RefEmployeeId", RefEmployeeId },
            };

            return WebApi<TimeBalance>.GetData(controllerName, "GetLastByDateAndRefEmployeeId", parameters);
        }

        public static int Insert(TimeBalance TimeBalance)
        {
            return WebApi<int>.PostAsync(controllerName, TimeBalance, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<TimeBalance> TimeBalances)
        {
            return WebApi<int>.PostAsync(controllerName, TimeBalances, "MultiPost").Result;
        }

        public static bool Update(TimeBalance TimeBalance)
        {
            return WebApi<bool>.PutAsync(controllerName, TimeBalance, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
