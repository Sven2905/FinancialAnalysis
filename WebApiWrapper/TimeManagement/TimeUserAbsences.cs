using FinancialAnalysis.Models.TimeManagement;
using System.Collections.Generic;

namespace WebApiWrapper.TimeManagement
{
    public static class TimeUserAbsences
    {
        private const string controllerName = "TimeUserAbsences";

        public static List<TimeUserAbsence> GetAll()
        {
            return WebApi<List<TimeUserAbsence>>.GetData(controllerName);
        }

        public static TimeUserAbsence GetById(int id)
        {
            return WebApi<TimeUserAbsence>.GetDataById(controllerName, id);
        }

        public static int Insert(TimeUserAbsence TimeUserAbsence)
        {
            return WebApi<int>.PostAsync(controllerName, TimeUserAbsence, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<TimeUserAbsence> TimeUserAbsences)
        {
            return WebApi<int>.PostAsync(controllerName, TimeUserAbsences, "MultiPost").Result;
        }

        public static bool Update(TimeUserAbsence TimeUserAbsence)
        {
            return WebApi<bool>.PutAsync(controllerName, TimeUserAbsence, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}