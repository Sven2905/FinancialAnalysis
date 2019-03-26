using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.Accounting
{
    public static class Debitors
    {
        private const string controllerName = "Debitors";

        public static IEnumerable<Debitor> GetAll()
        {
            return WebApi.GetData<IEnumerable<Debitor>>(controllerName);
        }

        public static Debitor GetById(int id)
        {
            return WebApi.GetDataById<Debitor>(controllerName, id);
        }

        public static bool GetIsDebitorInUse(int id)
        {
            return WebApi.GetDataById<bool>(controllerName, id, "GetIsDebitorInUse");
        }

        public static int Insert(Debitor Debitor)
        {
            return WebApi.PostAsync(controllerName, Debitor).Result;
        }

        public static int Insert(IEnumerable<Debitor> Debitors)
        {
            return WebApi.PostAsync(controllerName, Debitors).Result;
        }

        public static bool Update(Debitor Debitor)
        {
            return WebApi.PutAsync(controllerName, Debitor, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
