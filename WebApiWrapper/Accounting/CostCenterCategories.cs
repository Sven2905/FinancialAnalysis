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

        public static IEnumerable<CostCenterCategory> GetAll()
        {
            return WebApi.GetData<IEnumerable<CostCenterCategory>>(controllerName);
        }

        public static CostCenterCategory GetById(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Id", id);
            return WebApi.GetData<CostCenterCategory>(controllerName, "GetById", parameters);
        }

        public static int Insert(CostCenterCategory CostCenterCategory)
        {
            return WebApi.PostAsync(controllerName, CostCenterCategory).Result;
        }

        public static int Insert(IEnumerable<CostCenterCategory> CostCenterCategories)
        {
            return WebApi.PostAsync(controllerName, CostCenterCategories).Result;
        }

        public static bool Update(CostCenterCategory CostCenterCategory)
        {
            return WebApi.PutAsync(controllerName, CostCenterCategory, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
