using FinancialAnalysis.Models.WarehouseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.WarehouseManagement
{
    public static class Warehouses
    {
        private const string controllerName = "Warehouses";

        public static List<Warehouse> GetAll()
        {
            return WebApi<List<Warehouse>>.GetData(controllerName);
        }

        public static Warehouse GetById(int id)
        {
            return WebApi<Warehouse>.GetDataById(controllerName, id);
        }

        public static List<Warehouse> GetByProductId(int refProductId)
        {
            return WebApi<List<Warehouse>>.GetDataById(controllerName, refProductId, "GetByProductId");
        }

        public static List<Warehouse> GetAllWithoutStock()
        {
            return WebApi<List<Warehouse>>.GetData(controllerName, "GetAllWithoutStock");
        }

        public static int Insert(Warehouse Warehouse)
        {
            return WebApi<int>.PostAsync(controllerName, Warehouse).Result;
        }

        public static int Insert(IEnumerable<Warehouse> Warehouses)
        {
            return WebApi<int>.PostAsync(controllerName, Warehouses).Result;
        }

        public static bool Update(Warehouse Warehouse)
        {
            return WebApi<bool>.PutAsync(controllerName, Warehouse, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id).Result;
        }
    }
}
