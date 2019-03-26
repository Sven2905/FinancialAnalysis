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
        private const string controllerName = "Employees";

        public static IEnumerable<User> GetAll()
        {
            return WebApi.GetData<IEnumerable<User>>(controllerName);
        }

        public static User GetById(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Id", id);
            return WebApi.GetData<User>(controllerName, "GetById", parameters);
        }

        public static User GetUserByNameAndPassword(string username, string password)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("username", username);
            parameters.Add("password", password);
            return WebApi.GetData<User>(controllerName, "GetUserByNameAndPassword", parameters);
        }

        public static int Insert(User user)
        {
            return WebApi.PostAsync(controllerName, user).Result;
        }

        public static int Insert(IEnumerable<User> users)
        {
            return WebApi.PostAsync(controllerName, users).Result;
        }

        public static bool Update(User user)
        {
            return WebApi.PutAsync(controllerName, user).Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
