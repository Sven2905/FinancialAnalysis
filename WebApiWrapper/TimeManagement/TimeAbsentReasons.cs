using FinancialAnalysis.Models.TimeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.Accounting
{
    public static class TimeAbsentReasons
    {
        private const string controllerName = "TimeAbsentReasons";

        public static List<TimeAbsentReason> GetAll()
        {
            return WebApi<List<TimeAbsentReason>>.GetData(controllerName);
        }

        public static TimeAbsentReason GetById(int id)
        {
            return WebApi<TimeAbsentReason>.GetDataById(controllerName, id);
        }

        public static int Insert(TimeAbsentReason TimeAbsentReason)
        {
            return WebApi<int>.PostAsync(controllerName, TimeAbsentReason, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<TimeAbsentReason> TimeAbsentReasons)
        {
            return WebApi<int>.PostAsync(controllerName, TimeAbsentReasons, "MultiPost").Result;
        }

        public static bool Update(TimeAbsentReason TimeAbsentReason)
        {
            return WebApi<bool>.PutAsync(controllerName, TimeAbsentReason, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
