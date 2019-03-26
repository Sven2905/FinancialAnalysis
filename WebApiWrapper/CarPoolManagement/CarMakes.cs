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

        public static IEnumerable<CarMake> GetAll()
        {
            return WebApi.GetData<IEnumerable<CarMake>>(controllerName);
        }

        public static CarMake GetById(int id)
        {
            return WebApi.GetDataById<CarMake>(controllerName, id);
        }

        public static int Insert(CarMake CarMake)
        {
            return WebApi.PostAsync(controllerName, CarMake).Result;
        }

        public static int Insert(IEnumerable<CarMake> CarMakes)
        {
            return WebApi.PostAsync(controllerName, CarMakes).Result;
        }

        public static bool Update(CarMake CarMake)
        {
            return WebApi.PutAsync(controllerName, CarMake, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
