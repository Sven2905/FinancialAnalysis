using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.Accounting
{
    public static class BookingCostCenterMappings
    {
        private const string controllerName = "BookingCostCenterMappings";

        public static List<BookingCostCenterMapping> GetAll()
        {
            return WebApi<List<BookingCostCenterMapping>>.GetData(controllerName);
        }

        public static BookingCostCenterMapping GetById(int id)
        {
            return WebApi<BookingCostCenterMapping>.GetDataById(controllerName, id);
        }

        public static int Insert(BookingCostCenterMapping BookingCostCenterMapping)
        {
            return WebApi<int>.PostAsync(controllerName, BookingCostCenterMapping, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<BookingCostCenterMapping> BookingCostCenterMappings)
        {
            return WebApi<int>.PostAsync(controllerName, BookingCostCenterMappings, "MultiPost").Result;
        }

        public static bool Update(BookingCostCenterMapping BookingCostCenterMapping)
        {
            return WebApi<bool>.PutAsync(controllerName, BookingCostCenterMapping, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
