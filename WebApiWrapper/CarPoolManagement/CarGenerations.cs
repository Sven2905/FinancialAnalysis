using FinancialAnalysis.Models.CarPoolManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.CarPoolManagement
{
    public static class CarGenerations
    {
        private const string controllerName = "CarGenerations";

        public static List<CarGeneration> GetAll()
        {
            return WebApi<List<CarGeneration>>.GetData(controllerName);
        }

        public static List<CarGeneration> GetByRefCarBodyId(int RefCarBodyId)
        {
            return WebApi<List<CarGeneration>>.GetDataById(controllerName, RefCarBodyId, "GetByRefCarBodyId");
        }

        public static int Insert(CarGeneration CarGeneration)
        {
            return WebApi<int>.PostAsync(controllerName, CarGeneration).Result;
        }

        public static int Insert(IEnumerable<CarGeneration> CarGenerations)
        {
            return WebApi<int>.PostAsync(controllerName, CarGenerations).Result;
        }

        public static bool Update(CarGeneration CarGeneration)
        {
            return WebApi<bool>.PutAsync(controllerName, CarGeneration, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
