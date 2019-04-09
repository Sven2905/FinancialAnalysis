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

        public static List<CarModel> GetAll()
        {
            return WebApi<List<CarModel>>.GetData(controllerName);
        }

        public static List<CarModel> GetByRefCarMakeId(int RefCarMakeId)
        {
            return WebApi<List<CarModel>>.GetDataById(controllerName, RefCarMakeId, "GetByRefCarMakeId");
        }

        public static int Insert(CarModel CarModel)
        {
            return WebApi<int>.PostAsync(controllerName, CarModel, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<CarModel> CarModels)
        {
            return WebApi<int>.PostAsync(controllerName, CarModels, "MultiPost").Result;
        }

        public static bool Update(CarModel CarModel)
        {
            return WebApi<bool>.PutAsync(controllerName, CarModel, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
