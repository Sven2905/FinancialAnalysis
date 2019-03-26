using FinancialAnalysis.Models.WarehouseManagement;
using System.Collections.Generic;

namespace WebApiWrapper.WarehouseManagement
{
    public static class WarehouseStockingHistories
    {
        private const string controllerName = "WarehouseStockingHistories";

        public static IEnumerable<WarehouseStockingHistory> GetAll()
        {
            return WebApi.GetData<IEnumerable<WarehouseStockingHistory>>(controllerName);
        }

        public static WarehouseStockingHistory GetById(int id)
        {
            return WebApi.GetDataById<WarehouseStockingHistory>(controllerName, id);
        }

        public static IEnumerable<WarehouseStockingHistory> GetLast10(int RefProductId, int RefStockyardId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("RefProductId", RefProductId);
            parameters.Add("RefStockyardId", RefStockyardId);
            return WebApi.GetData<IEnumerable<WarehouseStockingHistory>>(controllerName, "GetLast10", parameters);
        }

        public static int Insert(WarehouseStockingHistory WarehouseStockingHistory)
        {
            return WebApi.PostAsync(controllerName, WarehouseStockingHistory).Result;
        }

        public static int Insert(IEnumerable<WarehouseStockingHistory> WarehouseStockingHistories)
        {
            return WebApi.PostAsync(controllerName, WarehouseStockingHistories).Result;
        }

        public static bool Update(WarehouseStockingHistory WarehouseStockingHistory)
        {
            return WebApi.PutAsync(controllerName, WarehouseStockingHistory, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
