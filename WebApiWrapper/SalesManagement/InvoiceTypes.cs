using FinancialAnalysis.Models.SalesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.SalesManagement
{
    public static class InvoiceTypes
    {
        private const string controllerName = "InvoiceTypes";

        public static List<InvoiceType> GetAll()
        {
            return WebApi<List<InvoiceType>>.GetData(controllerName);
        }

        public static InvoiceType GetById(int id)
        {
            return WebApi<InvoiceType>.GetDataById(controllerName, id);
        }

        public static int Insert(InvoiceType InvoiceType)
        {
            return WebApi<int>.PostAsync(controllerName, InvoiceType).Result;
        }

        public static int Insert(IEnumerable<InvoiceType> InvoiceTypes)
        {
            return WebApi<int>.PostAsync(controllerName, InvoiceTypes).Result;
        }

        public static bool Update(InvoiceType InvoiceType)
        {
            return WebApi<bool>.PutAsync(controllerName, InvoiceType, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id).Result;
        }
    }
}
