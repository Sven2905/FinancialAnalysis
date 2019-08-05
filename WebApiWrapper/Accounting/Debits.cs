using FinancialAnalysis.Models.Accounting;
using System.Collections.Generic;

namespace WebApiWrapper.Accounting
{
    public static class Debits
    {
        private const string controllerName = "Debits";

        public static List<Debit> GetAll()
        {
            return WebApi<List<Debit>>.GetData(controllerName);
        }

        public static Debit GetById(int id)
        {
            return WebApi<Debit>.GetDataById(controllerName, id);
        }

        public static int Insert(Debit Debit)
        {
            return WebApi<int>.PostAsync(controllerName, Debit, "SinglePost").Result;
        }

        public static int Insert(List<Debit> Debits)
        {
            return WebApi<int>.PostAsync(controllerName, Debits, "MultiPost").Result;
        }
    }
}