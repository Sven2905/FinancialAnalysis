using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.Accounting
{
    public static class Creditors
    {
        private const string controllerName = "Creditors";

        public static List<Creditor> GetAll()
        {
            return WebApi<List<Creditor>>.GetData(controllerName);
        }

        public static Creditor GetById(int id)
        {
            return WebApi<Creditor>.GetDataById(controllerName, id);
        }

        public static bool IsCreditorInUse(int id)
        {
            return WebApi<bool>.GetDataById(controllerName, id, "GetIsCreditorInUse");
        }

        public static int Insert(Creditor Creditor)
        {
            return WebApi<int>.PostAsync(controllerName, Creditor).Result;
        }

        public static int Insert(IEnumerable<Creditor> Creditors)
        {
            return WebApi<int>.PostAsync(controllerName, Creditors).Result;
        }

        public static bool Update(Creditor Creditor)
        {
            return WebApi<bool>.PutAsync(controllerName, Creditor, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
