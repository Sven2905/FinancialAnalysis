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

        public static IEnumerable<Creditor> GetAll()
        {
            return WebApi.GetData<IEnumerable<Creditor>>(controllerName);
        }

        public static Creditor GetById(int id)
        {
            return WebApi.GetDataById<Creditor>(controllerName, id);
        }

        public static bool IsCreditorInUse(int id)
        {
            return WebApi.GetDataById<bool>(controllerName, id, "GetIsCreditorInUse");
        }

        public static int Insert(Creditor Creditor)
        {
            return WebApi.PostAsync(controllerName, Creditor).Result;
        }

        public static int Insert(IEnumerable<Creditor> Creditors)
        {
            return WebApi.PostAsync(controllerName, Creditors).Result;
        }

        public static bool Update(Creditor Creditor)
        {
            return WebApi.PutAsync(controllerName, Creditor, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
