using FinancialAnalysis.Models.SalesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.SalesManagement
{
    public static class SalesOrders
    {
        private const string controllerName = "SalesOrders";

        public static IEnumerable<SalesOrder> GetAll()
        {
            return WebApi.GetData<IEnumerable<SalesOrder>>(controllerName);
        }

        public static SalesOrder GetById(int id)
        {
            return WebApi.GetDataById<SalesOrder>(controllerName, id);
        }

        public static IEnumerable<SalesOrder> GetOpenedSalesOrders()
        {
            return WebApi.GetData<IEnumerable<SalesOrder>>(controllerName, "GetOpenedSalesOrders");
        }

        public static int Insert(SalesOrder SalesOrder)
        {
            return WebApi.PostAsync(controllerName, SalesOrder).Result;
        }

        public static int Insert(IEnumerable<SalesOrder> SalesOrders)
        {
            return WebApi.PostAsync(controllerName, SalesOrders).Result;
        }

        public static bool Update(SalesOrder SalesOrder)
        {
            return WebApi.PutAsync(controllerName, SalesOrder, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
