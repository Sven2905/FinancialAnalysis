using FinancialAnalysis.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.Administration
{
    public static class Users
    {
        private const string controllerName = "Users";

        public static List<User> GetAll()
        {
            return WebApi<List<User>>.GetData(controllerName);
        }

        public static User GetById(int id)
        {
            return WebApi<User>.GetDataById(controllerName, id);
        }

        public static User GetUserByNameAndPassword(string username, string password)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("username", username);
            parameters.Add("password", password);
            return WebApi<User>.GetData(controllerName, "GetUserByNameAndPassword", parameters);
        }

        public static int Insert(User user)
        {
            return WebApi<int>.PostAsync(controllerName, user).Result;
        }

        public static int Insert(IEnumerable<User> users)
        {
            return WebApi<int>.PostAsync(controllerName, users).Result;
        }

        public static bool Update(User user)
        {
            return WebApi<int>.PutAsync(controllerName, user).Result;
        }

        public static bool UpdatePassword(User user)
        {
            return WebApi<bool>.PutAsync(controllerName, user, "PutUpdatePassword").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id).Result;
        }
    }
}
