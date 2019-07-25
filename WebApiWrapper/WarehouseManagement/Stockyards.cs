using FinancialAnalysis.Models.WarehouseManagement;
using System.Collections.Generic;

namespace WebApiWrapper.WarehouseManagement
{
    public static class Stockyards
    {
        private const string controllerName = "Stockyards";

        public static List<Stockyard> GetAll()
        {
            return WebApi<List<Stockyard>>.GetData(controllerName);
        }

        public static Stockyard GetById(int id)
        {
            return WebApi<Stockyard>.GetDataById(controllerName, id);
        }

        public static Stockyard GetStockById(int id)
        {
            return WebApi<Stockyard>.GetDataById(controllerName, id, "GetStockById");
        }

        public static List<Stockyard> GetByRefWarehouseId(int RefWarehouseId)
        {
            return WebApi<List<Stockyard>>.GetDataById(controllerName, RefWarehouseId, "GetByRefWarehouseId");
        }

        public static int Insert(Stockyard Stockyard)
        {
            return WebApi<bool>.PostAsync(controllerName, Stockyard, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<Stockyard> Stockyards)
        {
            return WebApi<bool>.PostAsync(controllerName, Stockyards, "MultiPost").Result;
        }

        public static bool Update(Stockyard Stockyard)
        {
            return WebApi<int>.PutAsync(controllerName, Stockyard, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<int>.DeleteAsync(controllerName, id);
        }
    }
}