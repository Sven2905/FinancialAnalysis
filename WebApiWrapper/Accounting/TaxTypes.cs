using FinancialAnalysis.Models.Accounting;
using System.Collections.Generic;

namespace WebApiWrapper.Accounting
{
    public static class TaxTypes
    {
        private const string controllerName = "TaxTypes";

        public static List<TaxType> GetAll()
        {
            return WebApi<List<TaxType>>.GetData(controllerName);
        }

        public static TaxType GetById(int id)
        {
            return WebApi<TaxType>.GetDataById(controllerName, id);
        }

        public static int Insert(TaxType TaxType)
        {
            return WebApi<int>.PostAsync(controllerName, TaxType, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<TaxType> TaxTypes)
        {
            return WebApi<int>.PostAsync(controllerName, TaxTypes, "MultiPost").Result;
        }

        public static bool Update(TaxType TaxType)
        {
            return WebApi<bool>.PutAsync(controllerName, TaxType, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
