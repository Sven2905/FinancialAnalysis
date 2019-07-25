using FinancialAnalysis.Models.TimeManagement;
using System.Collections.Generic;

namespace WebApiWrapper.TimeManagement
{
    public static class TimeEmployeeAbsences
    {
        private const string controllerName = "TimeEmployeeAbsences";

        public static List<TimeEmployeeAbsence> GetAll()
        {
            return WebApi<List<TimeEmployeeAbsence>>.GetData(controllerName);
        }

        public static TimeEmployeeAbsence GetById(int id)
        {
            return WebApi<TimeEmployeeAbsence>.GetDataById(controllerName, id);
        }

        public static int Insert(TimeEmployeeAbsence TimeEmployeeAbsence)
        {
            return WebApi<int>.PostAsync(controllerName, TimeEmployeeAbsence, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<TimeEmployeeAbsence> TimeEmployeeAbsences)
        {
            return WebApi<int>.PostAsync(controllerName, TimeEmployeeAbsences, "MultiPost").Result;
        }

        public static bool Update(TimeEmployeeAbsence TimeEmployeeAbsence)
        {
            return WebApi<bool>.PutAsync(controllerName, TimeEmployeeAbsence, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}