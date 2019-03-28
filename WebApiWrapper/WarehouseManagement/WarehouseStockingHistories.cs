using FinancialAnalysis.Models.WarehouseManagement;
using System.Collections.Generic;

namespace WebApiWrapper.WarehouseManagement
{
    public static class WarehouseStockingHistories
    {
        private const string controllerName = "WarehouseStockingHistories";

        public static List<WarehouseStockingHistory> GetAll()
        {
            return WebApi<List<WarehouseStockingHistory>>.GetData(controllerName);
        }

        public static WarehouseStockingHistory GetById(int id)
        {
            return WebApi<WarehouseStockingHistory>.GetDataById(controllerName, id);
        }

        public static List<WarehouseStockingHistory> GetLast10(int RefProductId, int RefStockyardId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("RefProductId", RefProductId);
            parameters.Add("RefStockyardId", RefStockyardId);
            return WebApi<List<WarehouseStockingHistory>>.GetData(controllerName, "GetLast10", parameters);
        }

        public static int Insert(WarehouseStockingHistory WarehouseStockingHistory)
        {
            return WebApi<int>.PostAsync(controllerName, WarehouseStockingHistory).Result;
        }

        public static int Insert(IEnumerable<WarehouseStockingHistory> WarehouseStockingHistories)
        {
            return WebApi<int>.PostAsync(controllerName, WarehouseStockingHistories).Result;
        }

        public static bool Update(WarehouseStockingHistory WarehouseStockingHistory)
        {
            return WebApi<bool>.PutAsync(controllerName, WarehouseStockingHistory, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
