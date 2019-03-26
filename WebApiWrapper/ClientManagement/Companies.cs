using FinancialAnalysis.Models.ClientManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.ClientManagement
{
    public static class Companies
    {
        private const string controllerName = "Companies";

        public static int Insert(Company Company)
        {
            return WebApi.PostAsync(controllerName, Company).Result;
        }

        public static int Insert(IEnumerable<Company> Companies)
        {
            return WebApi.PostAsync(controllerName, Companies).Result;
        }

        public static bool Update(Company Company)
        {
            return WebApi.PutAsync(controllerName, Company, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
