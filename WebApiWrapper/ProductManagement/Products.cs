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

        public static IEnumerable<Product> GetAll()
        {
            return WebApi.GetData<IEnumerable<Product>>(controllerName);
        }

        public static Product GetById(int id)
        {
            return WebApi.GetDataById<Product>(controllerName, id);
        }

        public static int Insert(Product Product)
        {
            return WebApi.PostAsync(controllerName, Product).Result;
        }

        public static int Insert(IEnumerable<Product> Products)
        {
            return WebApi.PostAsync(controllerName, Products).Result;
        }

        public static bool Update(Product Product)
        {
            return WebApi.PutAsync(controllerName, Product, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
