using FinancialAnalysis.Models.Accounting;
using System;
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

        public static decimal GetRemainingBudgetForId(int costCenterId, int year)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "CostCenterId", costCenterId },
                { "Year", year },
            };
            return WebApi<decimal>.GetData(controllerName, "GetRemainingBudgetForId", parameters);
        }

        public static decimal GetBudgetForId(int costCenterId, int year)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "CostCenterId", costCenterId },
                { "Year", year },
            };
            return WebApi<decimal>.GetData(controllerName, "GetBudgetForId", parameters);
        }
    }
}
