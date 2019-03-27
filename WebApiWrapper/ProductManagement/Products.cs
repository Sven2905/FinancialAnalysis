using FinancialAnalysis.Models.ProductManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.ProductManagement
{
    public static class Products
    {
        private const string controllerName = "Products";

        public static List<Product> GetAll()
        {
            return WebApi<List<Product>>.GetData(controllerName);
        }

        public static Product GetById(int id)
        {
            return WebApi<Product>.GetDataById(controllerName, id);
        }

        public static int Insert(Product Product)
        {
            return WebApi<bool>.PostAsync(controllerName, Product).Result;
        }

        public static int Insert(IEnumerable<Product> Products)
        {
            return WebApi<bool>.PostAsync(controllerName, Products).Result;
        }

        public static bool Update(Product Product)
        {
            return WebApi<int>.PutAsync(controllerName, Product, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<int>.DeleteAsync(controllerName, id).Result;
        }
    }
}
