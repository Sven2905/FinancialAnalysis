using FinancialAnalysis.Models.CarPoolManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return WebApi<int>.PostAsync(controllerName, CarMake).Result;
        }

        public static int Insert(IEnumerable<CarMake> CarMakes)
        {
            return WebApi<int>.PostAsync(controllerName, CarMakes).Result;
        }

        public static bool Update(CarMake CarMake)
        {
            return WebApi<bool>.PutAsync(controllerName, CarMake, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id).Result;
        }
    }
}
