﻿using FinancialAnalysis.Models.ProductManagement;
using System.Collections.Generic;

namespace WebApiWrapper.ProductManagement
{
    public static class ProductCategories
    {
        private const string controllerName = "ProductCategories";

        public static List<ProductCategory> GetAll()
        {
            return WebApi<List<ProductCategory>>.GetData(controllerName);
        }

        public static ProductCategory GetById(int id)
        {
            return WebApi<ProductCategory>.GetDataById(controllerName, id);
        }

        public static int Insert(ProductCategory ProductCategory)
        {
            return WebApi<int>.PostAsync(controllerName, ProductCategory, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<ProductCategory> ProductCategories)
        {
            return WebApi<int>.PostAsync(controllerName, ProductCategories, "MultiPost").Result;
        }

        public static bool Update(ProductCategory ProductCategory)
        {
            return WebApi<bool>.PutAsync(controllerName, ProductCategory, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}