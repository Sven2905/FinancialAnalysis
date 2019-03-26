using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.Accounting
{
    public static class GainAndLossAccounts
    {
        private const string controllerName = "GainAndLossAccounts";

        public static IEnumerable<GainAndLossAccount> GetAll()
        {
            return WebApi.GetData<IEnumerable<GainAndLossAccount>>(controllerName);
        }

        public static GainAndLossAccount GetById(int id)
        {
            return WebApi.GetDataById<GainAndLossAccount>(controllerName, id);
        }

        public static int Insert(GainAndLossAccount GainAndLossAccount)
        {
            return WebApi.PostAsync(controllerName, GainAndLossAccount).Result;
        }

        public static int Insert(IEnumerable<GainAndLossAccount> GainAndLossAccounts)
        {
            return WebApi.PostAsync(controllerName, GainAndLossAccounts).Result;
        }

        public static bool Update(GainAndLossAccount GainAndLossAccount)
        {
            return WebApi.PutAsync(controllerName, GainAndLossAccount, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
