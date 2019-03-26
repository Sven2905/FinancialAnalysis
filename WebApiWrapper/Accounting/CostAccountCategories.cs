using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.Accounting
{
    public static class CostAccountCategories
    {
        private const string controllerName = "CostAccountCategories";

        public static IEnumerable<CostAccountCategory> GetAll()
        {
            return WebApi.GetData<IEnumerable<CostAccountCategory>>(controllerName);
        }

        public static CostAccountCategory GetById(int id)
        {
            return WebApi.GetDataById<CostAccountCategory>(controllerName, id);
        }

        public static int GetCreditorId()
        {
            return WebApi.GetData<int>(controllerName, "GetCreditorId");
        }

        public static int GetDebitorId()
        {
            return WebApi.GetData<int>(controllerName, "GetDebitorId");
        }

        public static int Insert(CostAccountCategory CostAccountCategory)
        {
            return WebApi.PostAsync(controllerName, CostAccountCategory).Result;
        }

        public static int Insert(IEnumerable<CostAccountCategory> CostAccountCategories)
        {
            return WebApi.PostAsync(controllerName, CostAccountCategories).Result;
        }

        public static bool Update(CostAccountCategory CostAccountCategory)
        {
            return WebApi.PutAsync(controllerName, CostAccountCategory, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
