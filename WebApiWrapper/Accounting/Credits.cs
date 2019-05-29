using FinancialAnalysis.Models.Accounting;
using System.Collections.Generic;

namespace WebApiWrapper.Accounting
{
    public static class Credits
    {
        private const string controllerName = "Credits";

        public static List<Credit> GetAll()
        {
            return WebApi<List<Credit>>.GetData(controllerName);
        }

        public static Credit GetById(int id)
        {
            return WebApi<Credit>.GetDataById(controllerName, id);
        }

        public static int Insert(Credit Credit)
        {
            return WebApi<int>.PostAsync(controllerName, Credit, "SinglePost").Result;
        }

        public static int Insert(List<Credit> Credits)
        {
            return WebApi<int>.PostAsync(controllerName, Credits, "MultiPost").Result;
        }
    }
}
