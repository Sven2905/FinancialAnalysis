using FinancialAnalysis.Models.Accounting;
using System.Collections.Generic;

namespace WebApiWrapper.Accounting
{
    public static class Debitors
    {
        private const string controllerName = "Debitors";

        public static List<Debitor> GetAll()
        {
            return WebApi<List<Debitor>>.GetData(controllerName);
        }

        public static Debitor GetById(int id)
        {
            return WebApi<Debitor>.GetDataById(controllerName, id);
        }

        public static bool IsDebitorInUse(int id)
        {
            return WebApi<bool>.GetDataById(controllerName, id, "GetIsDebitorInUse");
        }

        public static int Insert(Debitor Debitor)
        {
            return WebApi<int>.PostAsync(controllerName, Debitor, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<Debitor> Debitors)
        {
            return WebApi<int>.PostAsync(controllerName, Debitors, "MultiPost").Result;
        }

        public static bool Update(Debitor Debitor)
        {
            return WebApi<bool>.PutAsync(controllerName, Debitor, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}