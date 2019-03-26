using FinancialAnalysis.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.Administration
{
    public static class UserRights
    {
        private const string controllerName = "UserRights";

        public static IEnumerable<UserRight> GetAll()
        {
            return WebApi.GetData<IEnumerable<UserRight>>(controllerName);
        }

        public static UserRight GetById(int id)
        {
            return WebApi.GetDataById<UserRight>(controllerName, id);
        }

        public static int Insert(UserRight UserRight)
        {
            return WebApi.PostAsync(controllerName, UserRight).Result;
        }

        public static int Insert(IEnumerable<UserRight> UserRights)
        {
            return WebApi.PostAsync(controllerName, UserRights).Result;
        }

        public static bool Update(UserRight UserRight)
        {
            return WebApi.PutAsync(controllerName, UserRight).Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
