using FinancialAnalysis.Models.CarPoolManagement;
using System.Collections.Generic;

namespace WebApiWrapper.CarPoolManagement
{
    public static class Vehicles
    {
        private const string controllerName = "Vehicles";

        public static List<Vehicle> GetAll()
        {
            return WebApi<List<Vehicle>>.GetData(controllerName);
        }

        public static Vehicle GetById(int id)
        {
            return WebApi<Vehicle>.GetDataById(controllerName, id);
        }

        public static int Insert(Vehicle Vehicle)
        {
            return WebApi<int>.PostAsync(controllerName, Vehicle, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<Vehicle> Vehicles)
        {
            return WebApi<int>.PostAsync(controllerName, Vehicles, "MultiPost").Result;
        }

        public static bool Update(Vehicle Vehicle)
        {
            return WebApi<bool>.PutAsync(controllerName, Vehicle, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
