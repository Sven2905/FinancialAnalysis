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

        public static List<BalanceAccountResultItem> GetActiveAccounts(DateTime StartDate, DateTime EndDate)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("StartDate", StartDate);
            parameters.Add("EndDate", EndDate);
            return WebApi<List<BalanceAccountResultItem>>.GetData(controllerName, "GetActiveAccounts", parameters);
        }

        public static List<BalanceAccountResultDetailItem> GetActiveAccountsDetailed(DateTime StartDate, DateTime EndDate)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("StartDate", StartDate);
            parameters.Add("EndDate", EndDate);
            return WebApi<List<BalanceAccountResultDetailItem>>.GetData(controllerName, "GetActiveAccountsDetailed", parameters);
        }

        public static List<BalanceAccountResultItem> GetPassiveAccounts(DateTime StartDate, DateTime EndDate)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("StartDate", StartDate);
            parameters.Add("EndDate", EndDate);
            return WebApi<List<BalanceAccountResultItem>>.GetData(controllerName, "GetPassiveAccounts", parameters);
        }

        public static List<BalanceAccountResultDetailItem> GetPassiveAccountsDetailed(DateTime StartDate, DateTime EndDate)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("StartDate", StartDate);
            parameters.Add("EndDate", EndDate);
            return WebApi<List<BalanceAccountResultDetailItem>>.GetData(controllerName, "GetPassiveAccountsDetailed", parameters);
        }

        public static int Insert(BalanceAccount balanceAccount)
        {
            return WebApi<int>.PostAsync(controllerName, balanceAccount, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<BalanceAccount> balanceAccounts)
        {
            return WebApi<int>.PostAsync(controllerName, balanceAccounts, "MultiPost").Result;
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
