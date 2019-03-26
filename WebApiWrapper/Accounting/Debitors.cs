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
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Id", id);
            return WebApi.GetData<Debitor>(controllerName, "GetById", parameters);
        }

        public static bool GetIsDebitorInUse(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Id", id);
            return WebApi.GetData<bool>(controllerName, "GetIsDebitorInUse", parameters);
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
