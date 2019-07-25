using FinancialAnalysis.Models.Accounting;
using System.Collections.Generic;

namespace WebApiWrapper.Accounting
{
    public static class DepreciationItems
    {
        private const string controllerName = "DepreciationItems";

        public static List<DepreciationItem> GetAll()
        {
            return WebApi<List<DepreciationItem>>.GetData(controllerName);
        }

        public static DepreciationItem GetById(int id)
        {
            return WebApi<DepreciationItem>.GetDataById(controllerName, id);
        }

        public static int Insert(DepreciationItem DepreciationItem)
        {
            return WebApi<int>.PostAsync(controllerName, DepreciationItem, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<DepreciationItem> DepreciationItems)
        {
            return WebApi<int>.PostAsync(controllerName, DepreciationItems, "MultiPost").Result;
        }

        public static bool Update(DepreciationItem DepreciationItem)
        {
            return WebApi<bool>.PutAsync(controllerName, DepreciationItem, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}