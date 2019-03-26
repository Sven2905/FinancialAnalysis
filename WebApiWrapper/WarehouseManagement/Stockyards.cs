using FinancialAnalysis.Models.WarehouseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.WarehouseManagement
{
    public static class Stockyards
    {
        private const string controllerName = "Stockyards";

        public static IEnumerable<Stockyard> GetAll()
        {
            return WebApi.GetData<IEnumerable<Stockyard>>(controllerName);
        }

        public static Stockyard GetById(int id)
        {
            return WebApi.GetDataById<Stockyard>(controllerName, id);
        }

        public static Stockyard GetStockById(int id)
        {
            return WebApi.GetDataById<Stockyard>(controllerName, id, "GetStockById");
        }

        public static IEnumerable<Stockyard> GetByRefWarehouseId(int RefWarehouseId)
        {
            return WebApi.GetDataById<IEnumerable<Stockyard>>(controllerName, RefWarehouseId, "GetByRefWarehouseId");
        }

        public static int Insert(Stockyard Stockyard)
        {
            return WebApi.PostAsync(controllerName, Stockyard).Result;
        }

        public static int Insert(IEnumerable<Stockyard> Stockyards)
        {
            return WebApi.PostAsync(controllerName, Stockyards).Result;
        }

        public static bool Update(Stockyard Stockyard)
        {
            return WebApi.PutAsync(controllerName, Stockyard, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
