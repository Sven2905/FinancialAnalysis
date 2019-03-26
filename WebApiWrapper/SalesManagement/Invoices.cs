using FinancialAnalysis.Models.SalesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.SalesManagement
{
    public static class Invoices
    {
        private const string controllerName = "Invoices";

        public static IEnumerable<Invoice> GetAll()
        {
            return WebApi.GetData<IEnumerable<Invoice>>(controllerName);
        }

        public static Invoice GetById(int id)
        {
            return WebApi.GetDataById<Invoice>(controllerName, id);
        }

        public static IEnumerable<Invoice> GetOpenInvoices()
        {
            return WebApi.GetData<IEnumerable<Invoice>>(controllerName, "GetOpenInvoices");
        }

        public static int Insert(Invoice Invoice)
        {
            return WebApi.PostAsync(controllerName, Invoice).Result;
        }

        public static int Insert(IEnumerable<Invoice> Invoices)
        {
            return WebApi.PostAsync(controllerName, Invoices).Result;
        }

        public static bool Update(Invoice Invoice)
        {
            return WebApi.PutAsync(controllerName, Invoice, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
