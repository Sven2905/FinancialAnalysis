using FinancialAnalysis.Models.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.ProjectManagement
{
    public static class HealthInsurances
    {
        private const string controllerName = "HealthInsurances";

        public static IEnumerable<HealthInsurance> GetAll()
        {
            return WebApi.GetData<IEnumerable<HealthInsurance>>(controllerName);
        }

        public static HealthInsurance GetById(int id)
        {
            return WebApi.GetDataById<HealthInsurance>(controllerName, id);
        }

        public static int Insert(HealthInsurance HealthInsurance)
        {
            return WebApi.PostAsync(controllerName, HealthInsurance).Result;
        }

        public static int Insert(IEnumerable<HealthInsurance> HealthInsurances)
        {
            return WebApi.PostAsync(controllerName, HealthInsurances).Result;
        }

        public static bool Update(HealthInsurance HealthInsurance)
        {
            return WebApi.PutAsync(controllerName, HealthInsurance, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
