using FinancialAnalysis.Models.SalesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.SalesManagement
{
    public static class SalesOrderPositions
    {
        private const string controllerName = "SalesOrderPositions";

        public static IEnumerable<SalesOrderPosition> GetAll()
        {
            return WebApi.GetData<IEnumerable<SalesOrderPosition>>(controllerName);
        }

        public static SalesOrderPosition GetById(int id)
        {
            return WebApi.GetDataById<SalesOrderPosition>(controllerName, id);
        }

        public static int Insert(SalesOrderPosition SalesOrderPosition)
        {
            return WebApi.PostAsync(controllerName, SalesOrderPosition).Result;
        }

        public static int Insert(IEnumerable<SalesOrderPosition> SalesOrderPositions)
        {
            return WebApi.PostAsync(controllerName, SalesOrderPositions).Result;
        }

        public static bool Update(SalesOrderPosition SalesOrderPosition)
        {
            return WebApi.PutAsync(controllerName, SalesOrderPosition, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
