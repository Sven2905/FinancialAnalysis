using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.Accounting
{
    public static class CostCenters
    {
        private const string controllerName = "CostCenters";

        public static IEnumerable<CostCenter> GetAll()
        {
            return WebApi.GetData<IEnumerable<CostCenter>>(controllerName);
        }

        public static CostCenter GetById(int id)
        {
            return WebApi.GetDataById<CostCenter>(controllerName, id);
        }

        public static int Insert(CostCenter CostCenter)
        {
            return WebApi.PostAsync(controllerName, CostCenter).Result;
        }

        public static int Insert(IEnumerable<CostCenter> CostCenters)
        {
            return WebApi.PostAsync(controllerName, CostCenters).Result;
        }

        public static bool Update(CostCenter CostCenter)
        {
            return WebApi.PutAsync(controllerName, CostCenter, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
