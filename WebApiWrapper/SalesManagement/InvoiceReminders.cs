using FinancialAnalysis.Models.SalesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.SalesManagement
{
    public static class InvoiceReminders
    {
        private const string controllerName = "InvoiceReminders";

        public static IEnumerable<InvoiceReminder> GetAll()
        {
            return WebApi.GetData<IEnumerable<InvoiceReminder>>(controllerName);
        }

        public static InvoiceReminder GetById(int id)
        {
            return WebApi.GetDataById<InvoiceReminder>(controllerName, id);
        }

        public static int Insert(InvoiceReminder InvoiceReminder)
        {
            return WebApi.PostAsync(controllerName, InvoiceReminder).Result;
        }

        public static int Insert(IEnumerable<InvoiceReminder> InvoiceReminders)
        {
            return WebApi.PostAsync(controllerName, InvoiceReminders).Result;
        }

        public static bool Update(InvoiceReminder InvoiceReminder)
        {
            return WebApi.PutAsync(controllerName, InvoiceReminder, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
