using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.Accounting
{
    public static class CostCenterCategories
    {
        private const string controllerName = "CostCenterCategories";

        public static List<CostCenterCategory> GetAll()
        {
            return WebApi<List<CostCenterCategory>>.GetData(controllerName);
        }

        public static CostCenterCategory GetById(int id)
        {
            return WebApi<CostCenterCategory>.GetDataById(controllerName, id);
        }

        public static int Insert(CostCenterCategory CostCenterCategory)
        {
            return WebApi<int>.PostAsync(controllerName, CostCenterCategory).Result;
        }

        public static int Insert(IEnumerable<CostCenterCategory> CostCenterCategories)
        {
            return WebApi<int>.PostAsync(controllerName, CostCenterCategories).Result;
        }

        public static bool Update(CostCenterCategory CostCenterCategory)
        {
            return WebApi<bool>.PutAsync(controllerName, CostCenterCategory, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
