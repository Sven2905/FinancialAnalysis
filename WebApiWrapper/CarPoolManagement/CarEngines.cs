using FinancialAnalysis.Models.CarPoolManagement;
using System.Collections.Generic;

namespace WebApiWrapper.CarPoolManagement
{
    public static class CarEngines
    {
        private const string controllerName = "CarEngines";

        public static List<CarEngine> GetAll()
        {
            return WebApi<List<CarEngine>>.GetData(controllerName);
        }

        public static List<CarEngine> GetByRefCarTrimId(int RefCarTrimId)
        {
            return WebApi<List<CarEngine>>.GetDataById(controllerName, RefCarTrimId, "GetByRefCarTrimId");
        }

        public static int Insert(CarEngine CarEngine)
        {
            return WebApi<int>.PostAsync(controllerName, CarEngine, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<CarEngine> CarEngines)
        {
            return WebApi<int>.PostAsync(controllerName, CarEngines, "MultiPost").Result;
        }

        public static bool Update(CarEngine CarEngine)
        {
            return WebApi<bool>.PutAsync(controllerName, CarEngine, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
