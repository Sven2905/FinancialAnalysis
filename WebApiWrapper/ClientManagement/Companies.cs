using FinancialAnalysis.Models.ClientManagement;
using System.Collections.Generic;

namespace WebApiWrapper.ClientManagement
{
    public static class Companies
    {
        private const string controllerName = "Companies";

        public static int Insert(Company Company)
        {
            return WebApi<int>.PostAsync(controllerName, Company, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<Company> Companies)
        {
            return WebApi<int>.PostAsync(controllerName, Companies, "MultiPost").Result;
        }

        public static bool Update(Company Company)
        {
            return WebApi<bool>.PutAsync(controllerName, Company, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}