using FinancialAnalysis.Models.ClientManagement;
using System.Collections.Generic;

namespace WebApiWrapper.ClientManagement
{
    public static class Clients
    {
        private const string controllerName = "Clients";

        public static List<Client> GetAll()
        {
            return WebApi<List<Client>>.GetData(controllerName);
        }

        public static Client GetById(int id)
        {
            return WebApi<Client>.GetDataById(controllerName, id);
        }

        public static bool IsClientInUse(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "Id", id }
            };
            return WebApi<bool>.GetData(controllerName, "GetIsClientInUse", parameters);
        }

        public static int Insert(Client Client)
        {
            return WebApi<bool>.PostAsync(controllerName, Client, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<Client> Clients)
        {
            return WebApi<bool>.PostAsync(controllerName, Clients, "MultiPost").Result;
        }

        public static bool Update(Client Client)
        {
            return WebApi<int>.PutAsync(controllerName, Client, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<int>.DeleteAsync(controllerName, id);
        }
    }
}
