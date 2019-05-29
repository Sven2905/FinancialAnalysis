using FinancialAnalysis.Models.TimeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.TimeManagement
{
    public static class TimeObligatoryHours
    {
        private const string controllerName = "TimeObligatoryHours";

        public static List<TimeObligatoryHour> GetAll()
        {
            return WebApi<List<TimeObligatoryHour>>.GetData(controllerName);
        }

        public static TimeObligatoryHour GetById(int id)
        {
            return WebApi<TimeObligatoryHour>.GetDataById(controllerName, id);
        }

        public static int Insert(TimeObligatoryHour TimeObligatoryHour)
        {
            return WebApi<int>.PostAsync(controllerName, TimeObligatoryHour, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<TimeObligatoryHour> TimeObligatoryHours)
        {
            return WebApi<int>.PostAsync(controllerName, TimeObligatoryHours, "MultiPost").Result;
        }

        public static bool Update(TimeObligatoryHour TimeObligatoryHour)
        {
            return WebApi<bool>.PutAsync(controllerName, TimeObligatoryHour, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
