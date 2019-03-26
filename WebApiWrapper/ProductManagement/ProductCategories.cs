using FinancialAnalysis.Models.ProductManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.ProductManagement
{
    public static class ProductCategories
    {
        private const string controllerName = "ProductCategories";

        public static IEnumerable<ProductCategory> GetAll()
        {
            return WebApi.GetData<IEnumerable<ProductCategory>>(controllerName);
        }

        public static ProductCategory GetById(int id)
        {
            return WebApi.GetDataById<ProductCategory>(controllerName, id);
        }

        public static int Insert(ProductCategory ProductCategory)
        {
            return WebApi.PostAsync(controllerName, ProductCategory).Result;
        }

        public static int Insert(IEnumerable<ProductCategory> ProductCategories)
        {
            return WebApi.PostAsync(controllerName, ProductCategories).Result;
        }

        public static bool Update(ProductCategory ProductCategory)
        {
            return WebApi.PutAsync(controllerName, ProductCategory, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
