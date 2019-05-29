using FinancialAnalysis.Models.Accounting;
using System.Collections.Generic;

namespace WebApiWrapper.Accounting
{
    public static class CostAccounts
    {
        private const string controllerName = "CostAccounts";

        public static List<CostAccount> GetAll()
        {
            return WebApi<List<CostAccount>>.GetData(controllerName);
        }

        public static CostAccount GetById(int id)
        {
            return WebApi<CostAccount>.GetDataById(controllerName, id);
        }

        public static List<CostAccount> GetAllVisible()
        {
            return WebApi<List<CostAccount>>.GetData(controllerName, "GetAllVisible");
        }

        public static int GetNextCreditorNumber()
        {
            return WebApi<int>.GetData(controllerName, "GetNextCreditorNumber");
        }

        public static int GetNextDebitorNumber()
        {
            return WebApi<int>.GetData(controllerName, "GetNextDebitorNumber");
        }

        public static int GetByAccountNumber(int AccountNumber)
        {
            return WebApi<int>.GetDataById(controllerName, AccountNumber, "GetNextDebitorNumber");
        }

        public static int Insert(CostAccount CostAccount)
        {
            return WebApi<int>.PostAsync(controllerName, CostAccount, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<CostAccount> CostAccounts)
        {
            return WebApi<int>.PostAsync(controllerName, CostAccounts, "MultiPost").Result;
        }

        public static bool Update(CostAccount CostAccount)
        {
            return WebApi<bool>.PutAsync(controllerName, CostAccount, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
