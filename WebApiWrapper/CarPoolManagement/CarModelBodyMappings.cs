using FinancialAnalysis.Models.CarPoolManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.CarPoolManagement
{
    public static class CarModelBodyMappings
    {
        private const string controllerName = "CarModelBodyMappings";

        public static int Insert(CarModelBodyMapping CarModelBodyMapping)
        {
            return WebApi<int>.PostAsync(controllerName, CarModelBodyMapping, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<CarModelBodyMapping> CarModelBodyMappings)
        {
            return WebApi<int>.PostAsync(controllerName, CarModelBodyMappings, "MultiPost").Result;
        }
    }
}
