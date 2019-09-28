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

        public static TimeBalance GetByDateAndRefUserId(DateTime Date, int RefUserId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "Date", Date },
                { "RefUserId", RefUserId },
            };

            return WebApi<TimeBalance>.GetData(controllerName, "GetByDateAndRefUserId", parameters);
        }

        public static TimeBalance GetLastByDateAndRefUserId(DateTime Date, int RefUserId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "Date", Date },
                { "RefUserId", RefUserId },
            };

            return WebApi<TimeBalance>.GetData(controllerName, "GetLastByDateAndRefUserId", parameters);
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