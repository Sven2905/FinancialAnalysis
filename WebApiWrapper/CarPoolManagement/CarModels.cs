using FinancialAnalysis.Models.CarPoolManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.CarPoolManagement
{
    public static class CarModels
    {
        private const string controllerName = "CarModels";

        public static IEnumerable<CarModel> GetAll()
        {
            return WebApi.GetData<IEnumerable<CarModel>>(controllerName);
        }

        public static IEnumerable<CarModel> GetByRefCarMakeId(int RefCarMakeId)
        {
            return WebApi.GetDataById<IEnumerable<CarModel>>(controllerName, RefCarMakeId, "GetByRefCarMakeId");
        }

        public static int Insert(CarModel CarModel)
        {
            return WebApi.PostAsync(controllerName, CarModel).Result;
        }

        public static int Insert(IEnumerable<CarModel> CarModels)
        {
            return WebApi.PostAsync(controllerName, CarModels).Result;
        }

        public static bool Update(CarModel CarModel)
        {
            return WebApi.PutAsync(controllerName, CarModel, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
