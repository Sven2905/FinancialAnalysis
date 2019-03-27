using FinancialAnalysis.Models.SalesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.SalesManagement
{
    public static class ShippedProducts
    {
        private const string controllerName = "ShippedProducts";

        public static int Insert(ShippedProduct ShippedProduct)
        {
            return WebApi<int>.PostAsync(controllerName, ShippedProduct).Result;
        }

        public static int Insert(IEnumerable<ShippedProduct> ShippedProducts)
        {
            return WebApi<int>.PostAsync(controllerName, ShippedProducts).Result;
        }

        public static bool Update(ShippedProduct ShippedProduct)
        {
            return WebApi<bool>.PutAsync(controllerName, ShippedProduct, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id).Result;
        }
    }
}
