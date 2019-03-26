using FinancialAnalysis.Models.CarPoolManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.CarPoolManagement
{
    public static class CarEngines
    {
        private const string controllerName = "CarEngines";

        public static IEnumerable<CarEngine> GetAll()
        {
            return WebApi.GetData<IEnumerable<CarEngine>>(controllerName);
        }

        public static IEnumerable<CarEngine> GetByRefCarTrimId(int RefCarTrimId)
        {
            return WebApi.GetDataById<IEnumerable<CarEngine>>(controllerName, RefCarTrimId, "GetByRefCarTrimId");
        }

        public static int Insert(CarEngine CarEngine)
        {
            return WebApi.PostAsync(controllerName, CarEngine).Result;
        }

        public static int Insert(IEnumerable<CarEngine> CarEngines)
        {
            return WebApi.PostAsync(controllerName, CarEngines).Result;
        }

        public static bool Update(CarEngine CarEngine)
        {
            return WebApi.PutAsync(controllerName, CarEngine, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
