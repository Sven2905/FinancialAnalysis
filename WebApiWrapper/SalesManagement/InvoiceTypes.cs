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

        public static IEnumerable<InvoiceType> GetAll()
        {
            return WebApi.GetData<IEnumerable<InvoiceType>>(controllerName);
        }

        public static InvoiceType GetById(int id)
        {
            return WebApi.GetDataById<InvoiceType>(controllerName, id);
        }

        public static int Insert(InvoiceType InvoiceType)
        {
            return WebApi.PostAsync(controllerName, InvoiceType).Result;
        }

        public static int Insert(IEnumerable<InvoiceType> InvoiceTypes)
        {
            return WebApi.PostAsync(controllerName, InvoiceTypes).Result;
        }

        public static bool Update(InvoiceType InvoiceType)
        {
            return WebApi.PutAsync(controllerName, InvoiceType, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
