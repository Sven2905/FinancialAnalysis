using FinancialAnalysis.Models.SalesManagement;
using System.Collections.Generic;

namespace WebApiWrapper.SalesManagement
{
    public static class ShipmentTypes
    {
        private const string controllerName = "ShipmentTypes";

        public static List<ShipmentType> GetAll()
        {
            return WebApi<List<ShipmentType>>.GetData(controllerName);
        }

        public static ShipmentType GetById(int id)
        {
            return WebApi<ShipmentType>.GetDataById(controllerName, id);
        }

        public static int Insert(ShipmentType ShipmentType)
        {
            return WebApi<int>.PostAsync(controllerName, ShipmentType, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<ShipmentType> ShipmentTypes)
        {
            return WebApi<int>.PostAsync(controllerName, ShipmentTypes, "MultiPost").Result;
        }

        public static bool Update(ShipmentType ShipmentType)
        {
            return WebApi<bool>.PutAsync(controllerName, ShipmentType, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
