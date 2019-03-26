using FinancialAnalysis.Models.SalesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.SalesManagement
{
    public static class SalesTypes
    {
        private const string controllerName = "SalesTypes";

        public static IEnumerable<SalesType> GetAll()
        {
            return WebApi.GetData<IEnumerable<SalesType>>(controllerName);
        }

        public static SalesType GetById(int id)
        {
            return WebApi.GetDataById<SalesType>(controllerName, id);
        }

        public static int Insert(SalesType SalesType)
        {
            return WebApi.PostAsync(controllerName, SalesType).Result;
        }

        public static int Insert(IEnumerable<SalesType> SalesTypes)
        {
            return WebApi.PostAsync(controllerName, SalesTypes).Result;
        }

        public static bool Update(SalesType SalesType)
        {
            return WebApi.PutAsync(controllerName, SalesType, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
