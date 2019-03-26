using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.Accounting
{
    public static class BalanceAccounts
    {
        private const string controllerName = "BalanceAccounts";

        public static IEnumerable<BalanceAccount> GetAll()
        {
            return WebApi.GetData<IEnumerable<BalanceAccount>>(controllerName);
        }

        public static BalanceAccount GetById(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Id", id);
            return WebApi.GetData<BalanceAccount>(controllerName, "GetById", parameters);
        }

        public static IEnumerable<BalanceAccount> GetActiveAccounts()
        {
            return WebApi.GetData<IEnumerable<BalanceAccount>>("BalanceAccounts", "GetActiveAccounts");
        }

        public static IEnumerable<BalanceAccount> GetPassiveAccounts()
        {
            return WebApi.GetData<IEnumerable<BalanceAccount>>("BalanceAccounts", "GetPassiveAccounts");
        }

        public static int Insert(BalanceAccount balanceAccount)
        {
            return WebApi.PostAsync(controllerName, balanceAccount).Result;
        }

        public static int Insert(IEnumerable<BalanceAccount> balanceAccounts)
        {
            return WebApi.PostAsync(controllerName, balanceAccounts).Result;
        }

        public static bool Update(BalanceAccount balanceAccount)
        {
            return WebApi.PutAsync(controllerName, balanceAccount, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
