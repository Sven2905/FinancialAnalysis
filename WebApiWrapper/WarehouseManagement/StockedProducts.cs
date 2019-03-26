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

        public static IEnumerable<StockedProduct> GetAll()
        {
            return WebApi.GetData<IEnumerable<StockedProduct>>(controllerName);
        }

        public static StockedProduct GetById(int id)
        {
            return WebApi.GetDataById<StockedProduct>(controllerName, id);
        }

        public static StockedProduct GetByRefProductIdAndRefStockyardId(int refProductId, int refStockyardId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("refProductId", refProductId);
            parameters.Add("refStockyardId", refStockyardId);
            return WebApi.GetData<StockedProduct>(controllerName, "GetByRefProductIdAndRefStockyardId", parameters);
        }

        public static IEnumerable<StockedProduct> GetByRefStockyardId(int RefStockyardId)
        {
            return WebApi.GetDataById<IEnumerable<StockedProduct>>(controllerName, RefStockyardId, "GetByRefStockyardId");
        }

        public static int Insert(StockedProduct StockedProduct)
        {
            return WebApi.PostAsync(controllerName, StockedProduct).Result;
        }

        public static int Insert(IEnumerable<StockedProduct> StockedProducts)
        {
            return WebApi.PostAsync(controllerName, StockedProducts).Result;
        }

        public static bool Update(StockedProduct StockedProduct)
        {
            return WebApi.PutAsync(controllerName, StockedProduct, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
