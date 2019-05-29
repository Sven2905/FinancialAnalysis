using FinancialAnalysis.Models.Accounting;
using System.Collections.Generic;

namespace WebApiWrapper.Accounting
{
    public static class CostCenters
    {
        private const string controllerName = "CostCenters";

        public static List<CostCenter> GetAll()
        {
            return WebApi<List<CostCenter>>.GetData(controllerName);
        }

        public static CostCenter GetById(int id)
        {
            return WebApi<CostCenter>.GetDataById(controllerName, id);
        }

        public static int Insert(CostCenter CostCenter)
        {
            return WebApi<int>.PostAsync(controllerName, CostCenter, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<CostCenter> CostCenters)
        {
            return WebApi<int>.PostAsync(controllerName, CostCenters, "MultiPost").Result;
        }

        public static bool Update(CostCenter CostCenter)
        {
            return WebApi<bool>.PutAsync(controllerName, CostCenter, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
