using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.Accounting
{
    public static class TaxTypes
    {
        private const string controllerName = "TaxTypes";

        public static IEnumerable<TaxType> GetAll()
        {
            return WebApi.GetData<IEnumerable<TaxType>>(controllerName);
        }

        public static TaxType GetById(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Id", id);
            return WebApi.GetData<TaxType>(controllerName, "GetById", parameters);
        }

        public static int Insert(TaxType TaxType)
        {
            return WebApi.PostAsync(controllerName, TaxType).Result;
        }

        public static int Insert(IEnumerable<TaxType> TaxTypes)
        {
            return WebApi.PostAsync(controllerName, TaxTypes).Result;
        }

        public static bool Update(TaxType TaxType)
        {
            return WebApi.PutAsync(controllerName, TaxType, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
