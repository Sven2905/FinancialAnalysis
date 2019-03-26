using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.Accounting
{
    public static class FixedCostAllocations
    {
        private const string controllerName = "FixedCostAllocations";

        public static IEnumerable<FixedCostAllocation> GetAll()
        {
            return WebApi.GetData<IEnumerable<FixedCostAllocation>>(controllerName);
        }

        public static FixedCostAllocation GetById(int id)
        {
            return WebApi.GetDataById<FixedCostAllocation>(controllerName, id);
        }

        public static int Insert(FixedCostAllocation FixedCostAllocation)
        {
            return WebApi.PostAsync(controllerName, FixedCostAllocation).Result;
        }

        public static int Insert(IEnumerable<FixedCostAllocation> FixedCostAllocations)
        {
            return WebApi.PostAsync(controllerName, FixedCostAllocations).Result;
        }

        public static bool Update(FixedCostAllocation FixedCostAllocation)
        {
            return WebApi.PutAsync(controllerName, FixedCostAllocation, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
