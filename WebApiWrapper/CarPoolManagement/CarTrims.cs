using FinancialAnalysis.Models.CarPoolManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.CarPoolManagement
{
    public static class CarTrims
    {
        private const string controllerName = "CarTrims";

        public static IEnumerable<CarTrim> GetAll()
        {
            return WebApi.GetData<IEnumerable<CarTrim>>(controllerName);
        }

        public static IEnumerable<CarTrim> GetByRefCarGenerationId(int RefCarGenerationId)
        {
            return WebApi.GetDataById<IEnumerable<CarTrim>>(controllerName, RefCarGenerationId, "GetByRefCarGenerationId");
        }

        public static int Insert(CarTrim CarTrim)
        {
            return WebApi.PostAsync(controllerName, CarTrim).Result;
        }

        public static int Insert(IEnumerable<CarTrim> CarTrims)
        {
            return WebApi.PostAsync(controllerName, CarTrims).Result;
        }

        public static bool Update(CarTrim CarTrim)
        {
            return WebApi.PutAsync(controllerName, CarTrim, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
