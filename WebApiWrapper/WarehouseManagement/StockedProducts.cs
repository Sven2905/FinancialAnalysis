using FinancialAnalysis.Models.WarehouseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.WarehouseManagement
{
    public static class StockedProducts
    {
        private const string controllerName = "StockedProducts";

        public static List<StockedProduct> GetAll()
        {
            return WebApi<List<StockedProduct>>.GetData(controllerName);
        }

        public static StockedProduct GetById(int id)
        {
            return WebApi<StockedProduct>.GetDataById(controllerName, id);
        }

        public static StockedProduct GetByRefProductIdAndRefStockyardId(int refProductId, int refStockyardId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("refProductId", refProductId);
            parameters.Add("refStockyardId", refStockyardId);
            return WebApi<StockedProduct>.GetData(controllerName, "GetByRefProductIdAndRefStockyardId", parameters);
        }

        public static List<StockedProduct> GetByRefStockyardId(int RefStockyardId)
        {
            return WebApi<List<StockedProduct>>.GetDataById(controllerName, RefStockyardId, "GetByRefStockyardId");
        }

        public static int Insert(StockedProduct StockedProduct)
        {
            return WebApi<int>.PostAsync(controllerName, StockedProduct).Result;
        }

        public static int Insert(IEnumerable<StockedProduct> StockedProducts)
        {
            return WebApi<int>.PostAsync(controllerName, StockedProducts).Result;
        }

        public static bool Update(StockedProduct StockedProduct)
        {
            return WebApi<bool>.PutAsync(controllerName, StockedProduct, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id).Result;
        }
    }
}
