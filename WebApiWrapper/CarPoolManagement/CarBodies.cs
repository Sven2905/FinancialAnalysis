using FinancialAnalysis.Models.CarPoolManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.CarPoolManagement
{
    public static class CarBodies
    {
        private const string controllerName = "CarBodies";

        public static IEnumerable<CarBody> GetAll()
        {
            return WebApi.GetData<IEnumerable<CarBody>>(controllerName);
        }

        public static IEnumerable<CarBody> GetByRefCarModelId(int RefCarModelId)
        {
            return WebApi.GetDataById<IEnumerable<CarBody>>(controllerName, RefCarModelId, "GetByRefCarModelId");
        }

        public static int Insert(CarBody CarBody)
        {
            return WebApi.PostAsync(controllerName, CarBody).Result;
        }

        public static int Insert(IEnumerable<CarBody> CarBodys)
        {
            return WebApi.PostAsync(controllerName, CarBodys).Result;
        }

        public static bool Update(CarBody CarBody)
        {
            return WebApi.PutAsync(controllerName, CarBody, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
