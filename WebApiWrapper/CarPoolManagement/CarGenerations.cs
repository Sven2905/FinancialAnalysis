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

        public static IEnumerable<CarGeneration> GetAll()
        {
            return WebApi.GetData<IEnumerable<CarGeneration>>(controllerName);
        }

        public static IEnumerable<CarGeneration> GetByRefCarBodyId(int RefCarBodyId)
        {
            return WebApi.GetDataById<IEnumerable<CarGeneration>>(controllerName, RefCarBodyId, "GetByRefCarBodyId");
        }

        public static int Insert(CarGeneration CarGeneration)
        {
            return WebApi.PostAsync(controllerName, CarGeneration).Result;
        }

        public static int Insert(IEnumerable<CarGeneration> CarGenerations)
        {
            return WebApi.PostAsync(controllerName, CarGenerations).Result;
        }

        public static bool Update(CarGeneration CarGeneration)
        {
            return WebApi.PutAsync(controllerName, CarGeneration, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
