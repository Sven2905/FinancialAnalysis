using FinancialAnalysis.Models.SalesManagement;
using System.Collections.Generic;

namespace WebApiWrapper.SalesManagement
{
    public static class SalesOrderPositions
    {
        private const string controllerName = "SalesOrderPositions";

        public static List<SalesOrderPosition> GetAll()
        {
            return WebApi<List<SalesOrderPosition>>.GetData(controllerName);
        }

        public static SalesOrderPosition GetById(int id)
        {
            return WebApi<SalesOrderPosition>.GetDataById(controllerName, id);
        }

        public static int Insert(SalesOrderPosition SalesOrderPosition)
        {
            return WebApi<int>.PostAsync(controllerName, SalesOrderPosition, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<SalesOrderPosition> SalesOrderPositions)
        {
            return WebApi<int>.PostAsync(controllerName, SalesOrderPositions, "MultiPost").Result;
        }

        public static bool Update(SalesOrderPosition SalesOrderPosition)
        {
            return WebApi<bool>.PutAsync(controllerName, SalesOrderPosition, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
