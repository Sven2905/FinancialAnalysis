using FinancialAnalysis.Models.Accounting;
using System.Collections.Generic;

namespace WebApiWrapper.Accounting
{
    public static class FixedCostAllocations
    {
        private const string controllerName = "FixedCostAllocations";

        public static List<FixedCostAllocation> GetAll()
        {
            return WebApi<List<FixedCostAllocation>>.GetData(controllerName);
        }

        public static FixedCostAllocation GetById(int id)
        {
            return WebApi<FixedCostAllocation>.GetDataById(controllerName, id);
        }

        public static int Insert(FixedCostAllocation FixedCostAllocation)
        {
            return WebApi<int>.PostAsync(controllerName, FixedCostAllocation, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<FixedCostAllocation> FixedCostAllocations)
        {
            return WebApi<int>.PostAsync(controllerName, FixedCostAllocations, "MultiPost").Result;
        }

        public static bool Update(FixedCostAllocation FixedCostAllocation)
        {
            return WebApi<bool>.PutAsync(controllerName, FixedCostAllocation, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
