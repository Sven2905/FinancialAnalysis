﻿using FinancialAnalysis.Models.Administration;
using System.Collections.Generic;

namespace WebApiWrapper.Administration
{
    public static class UserRights
    {
        private const string controllerName = "UserRights";

        public static List<UserRight> GetAll()
        {
            return WebApi<List<UserRight>>.GetData(controllerName);
        }

        public static UserRight GetById(int id)
        {
            return WebApi<UserRight>.GetDataById(controllerName, id);
        }

        public static int Insert(UserRight UserRight)
        {
            return WebApi<int>.PostAsync(controllerName, UserRight, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<UserRight> UserRights)
        {
            return WebApi<int>.PostAsync(controllerName, UserRights, "MultiPost").Result;
        }

        public static bool Update(UserRight UserRight)
        {
            return WebApi<bool>.PutAsync(controllerName, UserRight, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}