using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.Accounting
{
    public static class Bookings
    {
        private const string controllerName = "Bookings";

        public static List<Booking> GetAll()
        {
            return WebApi<List<Booking>>.GetData(controllerName);
        }

        public static Booking GetById(int id)
        {
            return WebApi<Booking>.GetDataById(controllerName, id);
        }

        public static List<Booking> GetByParameter(DateTime startDate, DateTime endDate, int? creditId = null, int? debitId = null)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("startDate", startDate);
            parameters.Add("endDate", endDate);
            parameters.Add("creditId", creditId);
            parameters.Add("debitId", debitId);
            return WebApi<List<Booking>>.GetData(controllerName, "GetByParameter", parameters);
        }

        public static int Insert(Booking Booking)
        {
            return WebApi<int>.PostAsync(controllerName, Booking).Result;
        }

        public static int Insert(IEnumerable<Booking> Bookings)
        {
            return WebApi<int>.PostAsync(controllerName, Bookings).Result;
        }

        public static bool Update(Booking Booking)
        {
            return WebApi<bool>.PutAsync(controllerName, Booking, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
