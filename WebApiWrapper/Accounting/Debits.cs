using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.Accounting
{
    public static class Debits
    {
        private const string controllerName = "Debits";

        public static IEnumerable<Debit> GetAll()
        {
            return WebApi.GetData<IEnumerable<Debit>>(controllerName);
        }

        public static Debit GetById(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Id", id);
            return WebApi.GetData<Debit>(controllerName, "GetById", parameters);
        }

        public static int Insert(Debit Debit)
        {
            return WebApi.PostAsync(controllerName, Debit).Result;
        }

        public static int Insert(IEnumerable<Debit> Debits)
        {
            return WebApi.PostAsync(controllerName, Debits).Result;
        }
    }
}
