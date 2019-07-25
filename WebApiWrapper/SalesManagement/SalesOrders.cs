using FinancialAnalysis.Models.SalesManagement;
using System.Collections.Generic;

namespace WebApiWrapper.SalesManagement
{
    public static class SalesOrders
    {
        private const string controllerName = "SalesOrders";

        public static List<SalesOrder> GetAll()
        {
            return WebApi<List<SalesOrder>>.GetData(controllerName);
        }

        public static SalesOrder GetById(int id)
        {
            return WebApi<SalesOrder>.GetDataById(controllerName, id);
        }

        public static List<SalesOrder> GetOpenedSalesOrders()
        {
            return WebApi<List<SalesOrder>>.GetData(controllerName, "GetOpenedSalesOrders");
        }

        public static int Insert(SalesOrder SalesOrder)
        {
            return WebApi<int>.PostAsync(controllerName, SalesOrder, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<SalesOrder> SalesOrders)
        {
            return WebApi<int>.PostAsync(controllerName, SalesOrders, "MultiPost").Result;
        }

        public static bool Update(SalesOrder SalesOrder)
        {
            return WebApi<bool>.PutAsync(controllerName, SalesOrder, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}