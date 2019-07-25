using FinancialAnalysis.Models.SalesManagement;
using System.Collections.Generic;

namespace WebApiWrapper.SalesManagement
{
    public static class SalesTypes
    {
        private const string controllerName = "SalesTypes";

        public static List<SalesType> GetAll()
        {
            return WebApi<List<SalesType>>.GetData(controllerName);
        }

        public static SalesType GetById(int id)
        {
            return WebApi<SalesType>.GetDataById(controllerName, id);
        }

        public static int Insert(SalesType SalesType)
        {
            return WebApi<int>.PostAsync(controllerName, SalesType, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<SalesType> SalesTypes)
        {
            return WebApi<int>.PostAsync(controllerName, SalesTypes, "MultiPost").Result;
        }

        public static bool Update(SalesType SalesType)
        {
            return WebApi<bool>.PutAsync(controllerName, SalesType, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}