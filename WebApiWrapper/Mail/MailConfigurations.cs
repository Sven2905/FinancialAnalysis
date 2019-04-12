using FinancialAnalysis.Models.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return WebApi<bool>.PostAsync(controllerName, MailConfiguration, "SinglePost").Result;
        }

        public static bool Update(MailConfiguration MailConfiguration)
        {
            return WebApi<int>.PutAsync(controllerName, MailConfiguration, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<int>.DeleteAsync(controllerName, id);
        }
    }
}
