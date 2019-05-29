using FinancialAnalysis.Models.SalesManagement;
using System.Collections.Generic;

namespace WebApiWrapper.SalesManagement
{
    public static class InvoiceReminders
    {
        private const string controllerName = "InvoiceReminders";

        public static List<InvoiceReminder> GetAll()
        {
            return WebApi<List<InvoiceReminder>>.GetData(controllerName);
        }

        public static InvoiceReminder GetById(int id)
        {
            return WebApi<InvoiceReminder>.GetDataById(controllerName, id);
        }

        public static int Insert(InvoiceReminder InvoiceReminder)
        {
            return WebApi<int>.PostAsync(controllerName, InvoiceReminder, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<InvoiceReminder> InvoiceReminders)
        {
            return WebApi<int>.PostAsync(controllerName, InvoiceReminders, "MultiPost").Result;
        }

        public static bool Update(InvoiceReminder InvoiceReminder)
        {
            return WebApi<bool>.PutAsync(controllerName, InvoiceReminder, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
