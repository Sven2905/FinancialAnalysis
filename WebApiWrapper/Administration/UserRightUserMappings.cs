using FinancialAnalysis.Models.Administration;
using System.Collections.Generic;

namespace WebApiWrapper.Administration
{
    public static class UserRightUserMappings
    {
        private const string controllerName = "UserRightUserMappings";

        public static List<UserRightUserMapping> GetAll()
        {
            return WebApi<List<UserRightUserMapping>>.GetData(controllerName);
        }

        public static UserRightUserMapping GetById(int id)
        {
            return WebApi<UserRightUserMapping>.GetDataById(controllerName, id);
        }

        public static int Insert(UserRightUserMapping UserRightUserMapping)
        {
            return WebApi<bool>.PostAsync(controllerName, UserRightUserMapping, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<UserRightUserMapping> UserRightUserMappings)
        {
            return WebApi<bool>.PostAsync(controllerName, UserRightUserMappings, "MultiPost").Result;
        }

        public static bool Update(UserRightUserMapping UserRightUserMapping)
        {
            return WebApi<int>.PutAsync(controllerName, UserRightUserMapping, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<int>.DeleteAsync(controllerName, id);
        }
    }
}