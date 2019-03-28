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

        public static List<BalanceAccount> GetAll()
        {
            return WebApi<List<BalanceAccount>>.GetData(controllerName);
        }

        public static BalanceAccount GetById(int id)
        {
            return WebApi<BalanceAccount>.GetDataById(controllerName, id);
        }

        public static List<BalanceAccount> GetActiveAccounts()
        {
            return WebApi<List<BalanceAccount>>.GetData(controllerName, "GetActiveAccounts");
        }

        public static List<BalanceAccount> GetPassiveAccounts()
        {
            return WebApi<List<BalanceAccount>>.GetData(controllerName, "GetPassiveAccounts");
        }

        public static int Insert(BalanceAccount balanceAccount)
        {
            return WebApi<int>.PostAsync(controllerName, balanceAccount).Result;
        }

        public static int Insert(IEnumerable<BalanceAccount> balanceAccounts)
        {
            return WebApi<int>.PostAsync(controllerName, balanceAccounts).Result;
        }

        public static bool Update(BalanceAccount balanceAccount)
        {
            return WebApi<bool>.PutAsync(controllerName, balanceAccount, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
