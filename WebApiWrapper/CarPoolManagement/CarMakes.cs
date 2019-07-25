using FinancialAnalysis.Models.CarPoolManagement;
using System.Collections.Generic;

namespace WebApiWrapper.CarPoolManagement
{
    public static class CarMakes
    {
        private const string controllerName = "CarMakes";

        public static List<CarMake> GetAll()
        {
            return WebApi<List<CarMake>>.GetData(controllerName);
        }

        public static CarMake GetById(int id)
        {
            return WebApi<CarMake>.GetDataById(controllerName, id);
        }

        public static int Insert(CarMake CarMake)
        {
            return WebApi<int>.PostAsync(controllerName, CarMake, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<CarMake> CarMakes)
        {
            return WebApi<int>.PostAsync(controllerName, CarMakes, "MultiPost").Result;
        }

        public static bool Update(CarMake CarMake)
        {
            return WebApi<bool>.PutAsync(controllerName, CarMake, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}