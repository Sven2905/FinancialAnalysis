using FinancialAnalysis.Models.Accounting;
using System.Collections.Generic;

namespace WebApiWrapper.Accounting
{
    public static class GainAndLossAccounts
    {
        private const string controllerName = "GainAndLossAccounts";

        public static List<GainAndLossAccount> GetAll()
        {
            return WebApi<List<GainAndLossAccount>>.GetData(controllerName);
        }

        public static GainAndLossAccount GetById(int id)
        {
            return WebApi<GainAndLossAccount>.GetDataById(controllerName, id);
        }

        public static int Insert(GainAndLossAccount GainAndLossAccount)
        {
            return WebApi<int>.PostAsync(controllerName, GainAndLossAccount, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<GainAndLossAccount> GainAndLossAccounts)
        {
            return WebApi<int>.PostAsync(controllerName, GainAndLossAccounts, "MultiPost").Result;
        }

        public static bool Update(GainAndLossAccount GainAndLossAccount)
        {
            return WebApi<bool>.PutAsync(controllerName, GainAndLossAccount, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
