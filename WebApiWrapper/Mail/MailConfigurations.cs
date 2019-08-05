using FinancialAnalysis.Models.MailManagement;
using System.Collections.Generic;

namespace WebApiWrapper.MailManagement
{
    public static class MailConfigurations
    {
        private const string controllerName = "MailConfigurations";

        public static List<MailConfiguration> GetAll()
        {
            return WebApi<List<MailConfiguration>>.GetData(controllerName);
        }

        public static MailConfiguration GetById(int id)
        {
            return WebApi<MailConfiguration>.GetDataById(controllerName, id);
        }

        public static int Insert(MailConfiguration MailConfiguration)
        {
            return WebApi<int>.PostAsync(controllerName, MailConfiguration).Result;
        }

        public static bool Update(MailConfiguration MailConfiguration)
        {
            return WebApi<bool>.PutAsync(controllerName, MailConfiguration, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}