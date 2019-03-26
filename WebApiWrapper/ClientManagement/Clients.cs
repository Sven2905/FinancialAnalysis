using FinancialAnalysis.Models.ClientManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.ClientManagement
{
    public static class Clients
    {
        private const string controllerName = "Clients";

        public static IEnumerable<Client> GetAll()
        {
            return WebApi.GetData<IEnumerable<Client>>(controllerName);
        }

        public static Client GetById(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Id", id);
            return WebApi.GetData<Client>(controllerName, "GetById", parameters);
        }

        public static bool IsClientInUse(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Id", id);
            return WebApi.GetData<bool>(controllerName, "GetIsClientInUse", parameters);
        }

        public static int Insert(Client Client)
        {
            return WebApi.PostAsync(controllerName, Client).Result;
        }

        public static int Insert(IEnumerable<Client> Clients)
        {
            return WebApi.PostAsync(controllerName, Clients).Result;
        }

        public static bool Update(Client Client)
        {
            return WebApi.PutAsync(controllerName, Client, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
