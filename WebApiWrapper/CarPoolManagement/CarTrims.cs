using FinancialAnalysis.Models.CarPoolManagement;
using System.Collections.Generic;

namespace WebApiWrapper.CarPoolManagement
{
    public static class CarTrims
    {
        private const string controllerName = "CarTrims";

        public static List<CarTrim> GetAll()
        {
            return WebApi<List<CarTrim>>.GetData(controllerName);
        }

        public static List<CarTrim> GetByRefCarGenerationId(int RefCarGenerationId)
        {
            return WebApi<List<CarTrim>>.GetDataById(controllerName, RefCarGenerationId, "GetByRefCarGenerationId");
        }

        public static int Insert(CarTrim CarTrim)
        {
            return WebApi<int>.PostAsync(controllerName, CarTrim, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<CarTrim> CarTrims)
        {
            return WebApi<int>.PostAsync(controllerName, CarTrims, "MultiPost").Result;
        }

        public static bool Update(CarTrim CarTrim)
        {
            return WebApi<bool>.PutAsync(controllerName, CarTrim, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
