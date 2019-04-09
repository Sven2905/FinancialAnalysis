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

        public static List<CostAccountCategory> GetAll()
        {
            return WebApi<List<CostAccountCategory>>.GetData(controllerName);
        }

        public static CostAccountCategory GetById(int id)
        {
            return WebApi<CostAccountCategory>.GetDataById(controllerName, id);
        }

        public static int GetCreditorId()
        {
            return WebApi<int>.GetData(controllerName, "GetCreditorId");
        }

        public static int GetDebitorId()
        {
            return WebApi<int>.GetData(controllerName, "GetDebitorId");
        }

        public static int Insert(CostAccountCategory CostAccountCategory)
        {
            return WebApi<int>.PostAsync(controllerName, CostAccountCategory, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<CostAccountCategory> CostAccountCategories)
        {
            return WebApi<int>.PostAsync(controllerName, CostAccountCategories, "MultiPost").Result;
        }

        public static bool Update(CostAccountCategory CostAccountCategory)
        {
            return WebApi<bool>.PutAsync(controllerName, CostAccountCategory, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
