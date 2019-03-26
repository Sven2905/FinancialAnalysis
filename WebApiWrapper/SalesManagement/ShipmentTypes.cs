using FinancialAnalysis.Models.SalesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.SalesManagement
{
    public static class ShipmentTypes
    {
        private const string controllerName = "ShipmentTypes";

        public static IEnumerable<ShipmentType> GetAll()
        {
            return WebApi.GetData<IEnumerable<ShipmentType>>(controllerName);
        }

        public static ShipmentType GetById(int id)
        {
            return WebApi.GetDataById<ShipmentType>(controllerName, id);
        }

        public static int Insert(ShipmentType ShipmentType)
        {
            return WebApi.PostAsync(controllerName, ShipmentType).Result;
        }

        public static int Insert(IEnumerable<ShipmentType> ShipmentTypes)
        {
            return WebApi.PostAsync(controllerName, ShipmentTypes).Result;
        }

        public static bool Update(ShipmentType ShipmentType)
        {
            return WebApi.PutAsync(controllerName, ShipmentType, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
