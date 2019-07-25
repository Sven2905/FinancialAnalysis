using FinancialAnalysis.Models.ProjectManagement;
using System.Collections.Generic;

namespace WebApiWrapper.ProjectManagement
{
    public static class HealthInsurances
    {
        private const string controllerName = "HealthInsurances";

        public static List<HealthInsurance> GetAll()
        {
            return WebApi<List<HealthInsurance>>.GetData(controllerName);
        }

        public static HealthInsurance GetById(int id)
        {
            return WebApi<HealthInsurance>.GetDataById(controllerName, id);
        }

        public static int Insert(HealthInsurance HealthInsurance)
        {
            return WebApi<int>.PostAsync(controllerName, HealthInsurance, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<HealthInsurance> HealthInsurances)
        {
            return WebApi<int>.PostAsync(controllerName, HealthInsurances, "MultiPost").Result;
        }

        public static bool Update(HealthInsurance HealthInsurance)
        {
            return WebApi<bool>.PutAsync(controllerName, HealthInsurance, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}