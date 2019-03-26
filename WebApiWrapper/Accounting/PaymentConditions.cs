using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.Accounting
{
    public static class PaymentConditions
    {
        private const string controllerName = "PaymentConditions";

        public static IEnumerable<PaymentCondition> GetAll()
        {
            return WebApi.GetData<IEnumerable<PaymentCondition>>(controllerName);
        }

        public static PaymentCondition GetById(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Id", id);
            return WebApi.GetData<PaymentCondition>(controllerName, "GetById", parameters);
        }

        public static int Insert(PaymentCondition PaymentCondition)
        {
            return WebApi.PostAsync(controllerName, PaymentCondition).Result;
        }

        public static int Insert(IEnumerable<PaymentCondition> PaymentConditions)
        {
            return WebApi.PostAsync(controllerName, PaymentConditions).Result;
        }

        public static bool Update(PaymentCondition PaymentCondition)
        {
            return WebApi.PutAsync(controllerName, PaymentCondition, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
