using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.Accounting.CostCenterManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.Accounting
{
    public static class CostCenterBudgets
    {
        private const string controllerName = "CostCenterBudgets";

        public static List<CostCenterBudget> GetAll()
        {
            return WebApi<List<CostCenterBudget>>.GetData(controllerName);
        }

        public static CostCenterBudget GetById(int id)
        {
            return WebApi<CostCenterBudget>.GetDataById(controllerName, id);
        }

        public static List<CostCenterCurrentCosts> GetAnnuallyCosts(int RefCostCenterId, int Year)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("RefCostCenterId", RefCostCenterId);
            parameters.Add("Year", Year);
            return WebApi<List<CostCenterCurrentCosts>>.GetData(controllerName, "GetAnnuallyCosts", parameters);
        }

        public static int Insert(CostCenterBudget CostCenterBudget)
        {
            return WebApi<int>.PostAsync(controllerName, CostCenterBudget).Result;
        }

        public static int Insert(IEnumerable<CostCenterBudget> CostCenterBudgets)
        {
            return WebApi<int>.PostAsync(controllerName, CostCenterBudgets).Result;
        }

        public static bool Update(CostCenterBudget CostCenterBudget)
        {
            return WebApi<bool>.PutAsync(controllerName, CostCenterBudget, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id).Result;
        }
    }
}
