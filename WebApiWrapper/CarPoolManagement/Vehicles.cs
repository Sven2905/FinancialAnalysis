using FinancialAnalysis.Models.CarPoolManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.CarPoolManagement
{
    public static class Vehicles
    {
        private const string controllerName = "Vehicles";

        public static IEnumerable<Vehicle> GetAll()
        {
            return WebApi.GetData<IEnumerable<Vehicle>>(controllerName);
        }

        public static Vehicle GetById(int id)
        {
            return WebApi.GetDataById<Vehicle>(controllerName, id);
        }

        public static int Insert(Vehicle Vehicle)
        {
            return WebApi.PostAsync(controllerName, Vehicle).Result;
        }

        public static int Insert(IEnumerable<Vehicle> Vehicles)
        {
            return WebApi.PostAsync(controllerName, Vehicles).Result;
        }

        public static bool Update(Vehicle Vehicle)
        {
            return WebApi.PutAsync(controllerName, Vehicle, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
