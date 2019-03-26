using FinancialAnalysis.Models.SalesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.SalesManagement
{
    public static class InvoicePositions
    {
        private const string controllerName = "InvoicePositions";

        public static int Insert(InvoicePosition InvoicePosition)
        {
            return WebApi.PostAsync(controllerName, InvoicePosition).Result;
        }

        public static int Insert(IEnumerable<InvoicePosition> InvoicePositions)
        {
            return WebApi.PostAsync(controllerName, InvoicePositions).Result;
        }

        public static bool Update(InvoicePosition InvoicePosition)
        {
            return WebApi.PutAsync(controllerName, InvoicePosition, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
