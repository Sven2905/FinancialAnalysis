using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.Accounting
{
    public static class CostAccounts
    {
        private const string controllerName = "CostAccounts";

        public static IEnumerable<CostAccount> GetAll()
        {
            return WebApi.GetData<IEnumerable<CostAccount>>(controllerName);
        }

        public static CostAccount GetById(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Id", id);
            return WebApi.GetData<CostAccount>(controllerName, "GetById", parameters);
        }

        public static IEnumerable<CostAccount> GetAllVisible()
        {
            return WebApi.GetData<IEnumerable<CostAccount>>(controllerName, "GetAllVisible");
        }

        public static int GetNextCreditorNumber()
        {
            return WebApi.GetData<int>(controllerName, "GetNextCreditorNumber");
        }

        public static int GetNextDebitorNumber()
        {
            return WebApi.GetData<int>(controllerName, "GetNextDebitorNumber"); 
        }

        public static int Insert(CostAccount CostAccount)
        {
            return WebApi.PostAsync(controllerName, CostAccount).Result;
        }

        public static int Insert(IEnumerable<CostAccount> CostAccounts)
        {
            return WebApi.PostAsync(controllerName, CostAccounts).Result;
        }

        public static bool Update(CostAccount CostAccount)
        {
            return WebApi.PutAsync(controllerName, CostAccount, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
