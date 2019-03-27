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
            return WebApi<bool>.PostAsync(controllerName, UserRightUserMapping).Result;
        }

        public static int Insert(IEnumerable<UserRightUserMapping> UserRightUserMappings)
        {
            return WebApi<bool>.PostAsync(controllerName, UserRightUserMappings).Result;
        }

        public static bool Update(UserRightUserMapping UserRightUserMapping)
        {
            return WebApi<int>.PutAsync(controllerName, UserRightUserMapping).Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<int>.DeleteAsync(controllerName, id).Result;
        }
    }
}
