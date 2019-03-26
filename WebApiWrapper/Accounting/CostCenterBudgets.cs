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

        public static IEnumerable<CostCenterBudget> GetAll()
        {
            return WebApi.GetData<IEnumerable<CostCenterBudget>>(controllerName);
        }

        public static CostCenterBudget GetById(int id)
        {
            return WebApi.GetDataById<CostCenterBudget>(controllerName, id);
        }

        public static IEnumerable<CostCenterCurrentCosts> GetAnnuallyCosts(int RefCostCenterId, int Year)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("RefCostCenterId", RefCostCenterId);
            parameters.Add("Year", Year);
            return WebApi.GetData<IEnumerable<CostCenterCurrentCosts>>(controllerName, "GetAnnuallyCosts", parameters);
        }

        public static int Insert(CostCenterBudget CostCenterBudget)
        {
            return WebApi.PostAsync(controllerName, CostCenterBudget).Result;
        }

        public static int Insert(IEnumerable<CostCenterBudget> CostCenterBudgets)
        {
            return WebApi.PostAsync(controllerName, CostCenterBudgets).Result;
        }

        public static bool Update(CostCenterBudget CostCenterBudget)
        {
            return WebApi.PutAsync(controllerName, CostCenterBudget, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
