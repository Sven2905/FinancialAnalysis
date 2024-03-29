﻿using FinancialAnalysis.Models.CarPoolManagement;
using System.Collections.Generic;

namespace WebApiWrapper.CarPoolManagement
{
    public static class CarBodies
    {
        private const string controllerName = "CarBodies";

        public static List<CarBody> GetAll()
        {
            return WebApi<List<CarBody>>.GetData(controllerName);
        }

        public static List<CarBody> GetByRefCarModelId(int RefCarModelId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "RefCarModelId", RefCarModelId },
            };

            return WebApi<List<CarBody>>.GetData(controllerName, "GetByRefCarModelId", parameters);
            //return WebApi<List<CarBody>>.GetDataById(controllerName, RefCarModelId, "GetByRefCarModelId");
        }

        public static int Insert(CarBody CarBody)
        {
            return WebApi<int>.PostAsync(controllerName, CarBody, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<CarBody> CarBodys)
        {
            return WebApi<int>.PostAsync(controllerName, CarBodys, "MultiPost").Result;
        }

        public static bool Update(CarBody CarBody)
        {
            return WebApi<bool>.PutAsync(controllerName, CarBody, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}