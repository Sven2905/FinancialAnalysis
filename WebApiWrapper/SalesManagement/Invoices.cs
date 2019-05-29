using FinancialAnalysis.Models.SalesManagement;
using System.Collections.Generic;

namespace WebApiWrapper.SalesManagement
{
    public static class Invoices
    {
        private const string controllerName = "Invoices";

        public static List<Invoice> GetAll()
        {
            return WebApi<List<Invoice>>.GetData(controllerName);
        }

        public static Invoice GetById(int id)
        {
            return WebApi<Invoice>.GetDataById(controllerName, id);
        }

        public static List<Invoice> GetOpenInvoices()
        {
            return WebApi<List<Invoice>>.GetData(controllerName, "GetOpenInvoices");
        }

        public static int Insert(Invoice Invoice)
        {
            return WebApi<int>.PostAsync(controllerName, Invoice, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<Invoice> Invoices)
        {
            return WebApi<int>.PostAsync(controllerName, Invoices, "MultiPost").Result;
        }

        public static bool Update(Invoice Invoice)
        {
            return WebApi<bool>.PutAsync(controllerName, Invoice, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
