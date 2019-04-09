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
            return WebApi<int>.PostAsync(controllerName, InvoicePosition, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<InvoicePosition> InvoicePositions)
        {
            return WebApi<int>.PostAsync(controllerName, InvoicePositions, "MultiPost").Result;
        }

        public static bool Update(InvoicePosition InvoicePosition)
        {
            return WebApi<bool>.PutAsync(controllerName, InvoicePosition, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
