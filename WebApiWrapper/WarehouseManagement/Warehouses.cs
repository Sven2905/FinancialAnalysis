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

        public static IEnumerable<Warehouse> GetAll()
        {
            return WebApi.GetData<IEnumerable<Warehouse>>(controllerName);
        }

        public static Warehouse GetById(int id)
        {
            return WebApi.GetDataById<Warehouse>(controllerName, id);
        }

        public static IEnumerable<Warehouse> GetByProductId(int refProductId)
        {
            return WebApi.GetDataById<IEnumerable<Warehouse>>(controllerName, refProductId, "GetByProductId");
        }

        public static IEnumerable<Warehouse> GetAllWithoutStock()
        {
            return WebApi.GetData<IEnumerable<Warehouse>>(controllerName, "GetAllWithoutStock");
        }

        public static int Insert(Warehouse Warehouse)
        {
            return WebApi.PostAsync(controllerName, Warehouse).Result;
        }

        public static int Insert(IEnumerable<Warehouse> Warehouses)
        {
            return WebApi.PostAsync(controllerName, Warehouses).Result;
        }

        public static bool Update(Warehouse Warehouse)
        {
            return WebApi.PutAsync(controllerName, Warehouse, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
