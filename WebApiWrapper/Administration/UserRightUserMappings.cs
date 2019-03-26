using FinancialAnalysis.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.Administration
{
    public static class UserRightUserMappings
    {
        private const string controllerName = "UserRightUserMappings";

        public static IEnumerable<UserRightUserMapping> GetAll()
        {
            return WebApi.GetData<IEnumerable<UserRightUserMapping>>(controllerName);
        }

        public static UserRightUserMapping GetById(int id)
        {
            return WebApi.GetDataById<UserRightUserMapping>(controllerName, id);
        }

        public static int Insert(UserRightUserMapping UserRightUserMapping)
        {
            return WebApi.PostAsync(controllerName, UserRightUserMapping).Result;
        }

        public static int Insert(IEnumerable<UserRightUserMapping> UserRightUserMappings)
        {
            return WebApi.PostAsync(controllerName, UserRightUserMappings).Result;
        }

        public static bool Update(UserRightUserMapping UserRightUserMapping)
        {
            return WebApi.PutAsync(controllerName, UserRightUserMapping).Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
