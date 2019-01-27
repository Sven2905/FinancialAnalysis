﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Serilog;
using FinancialAnalysis.Models.ProductManagement;
using FinancialAnalysis.Models.Accounting;

namespace FinancialAnalysis.Datalayer.ProductManagement
{
    public class Products : ITable
    {
        private readonly ProductsStoredProcedures sp = new ProductsStoredProcedures();

        public Products()
        {
            TableName = "Products";
            CheckAndCreateTable();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs\\Tables.txt", rollingInterval: RollingInterval.Month)
                .CreateLogger();
        }

        public string TableName { get; }

        public void CheckAndCreateTable()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"If not exists (select name from sysobjects where name = '{TableName}') CREATE TABLE {TableName}(" +
                    "ProductId int IDENTITY(1,1) PRIMARY KEY," +
                    "Name nvarchar(150) NOT NULL," +
                    "Description nvarchar(150)," +
                    "Barcode nvarchar(150)," +
                    "RefTaxTypeId int, " +
                    "DimensionX decimal(7,2), " +
                    "DimensionY decimal(7,2), " +
                    "DimensionZ decimal(7,2), " +
                    "Weight decimal(7,3), " +
                    "IsStackable bit, " +
                    "Picture varbinary(MAX), " +
                    "PackageUnit int, " +
                    "DefaultBuyingPrice money, " +
                    "DefaultSellingPrice money, " +
                    "RefProductCategoryId int NOT NULL)";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating table '{TableName}'", e);
            }
        }

        public void CheckAndCreateStoredProcedures()
        {
            sp.CheckAndCreateProcedures();
        }

        /// <summary>
        ///     Returns all Product records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Product> GetAll()
        {
            IEnumerable<Product> output = new List<Product>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<Product, ProductCategory, TaxType, Product>($"dbo.{TableName}_GetAll",
                        (objProduct, objProductCategory, objTaxType) =>
                        {
                            objProduct.ProductCategory = objProductCategory;
                            objProduct.TaxType = objTaxType;
                            return objProduct;
                        }, splitOn: "ProductId, ProductCategoryId, TaxTypeId",
                        commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the Product item
        /// </summary>
        /// <param name="Product"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(Product Product)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result =
                        con.Query<int>(
                            $"dbo.{TableName}_Insert @Name, @Description, @Barcode, @RefTaxTypeId, @DimensionX, @DimensionY, @DimensionZ, @Weight, @IsStackable, @Picture, @PackageUnit, @DefaultBuyingPrice, @DefaultSellingPrice, @RefProductCategoryId",
                            Product);
                    id = result.Single();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }

            return id;
        }

        /// <summary>
        ///     Inserts the list of Products items
        /// </summary>
        /// <param name="Product"></param>
        public void Insert(IEnumerable<Product> Products)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var Product in Products) Insert(Product);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns Product Prototype by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product GetById(int id)
        {
            IEnumerable<Product> output = new List<Product>();

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<Product, ProductCategory, TaxType, Product>($"dbo.{TableName}_GetById @ProductId",
                        (objProduct, objProductCategory, objTaxType) =>
                        {
                            objProduct.ProductCategory = objProductCategory;
                            objProduct.TaxType = objTaxType;
                            return objProduct;
                        }, new { ProductId = id }, splitOn: "ProductId, ProductCategoryId, TaxTypeId",
                        commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output.FirstOrDefault();
        }

        /// <summary>
        ///     Update Product, if not exist, insert it
        /// </summary>
        /// <param name="Product"></param>
        public void UpdateOrInsert(Product Product)
        {
            if (Product.ProductId == 0 || GetById(Product.ProductId) is null)
            {
                Insert(Product);
                return;
            }

            Update(Product);
        }

        /// <summary>
        ///     Update Products, if not exist insert them
        /// </summary>
        /// <param name="User"></param>
        public void UpdateOrInsert(IEnumerable<Product> Products)
        {
            foreach (var Product in Products)
            {
                UpdateOrInsert(Product);
            }
        }

        /// <summary>
        ///     Update Product
        /// </summary>
        /// <param name="Product"></param>
        public void Update(Product Product)
        {
            if (Product.ProductId == 0)
            {
                return;
            }

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @ProductId, @Name, @Description, @Barcode, @RefTaxTypeId, @DimensionX, @DimensionY, @DimensionZ, @Weight, @IsStackable, @Picture, @PackageUnit, @DefaultBuyingPrice, @DefaultSellingPrice, @RefProductCategoryId", Product);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete User by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @ProductId", new { ProductId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        public void AddReferences()
        {
            AddCostAccountsReference();
            AddProductCategoriesReference();
            AddTaxTypesReference();
        }

        private void AddCostAccountsReference()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_ProductCategory', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_ProductCategory FOREIGN KEY(RefProductCategoryId) REFERENCES ProductCategories(ProductCategoryId)";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and ProductCategories", e);
            }
        }

        private void AddProductCategoriesReference()
        {
            string refTable = "ProductCategories";

            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_{refTable}', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_{refTable} FOREIGN KEY(RefProductCategoryId) REFERENCES {refTable}(ProductCategoryId) ON DELETE CASCADE";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and {TableName}",
                    e);
            }
        }

        private void AddTaxTypesReference()
        {
            string refTable = "TaxTypes";

            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_{refTable}', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_{refTable} FOREIGN KEY(RefTaxTypeId) REFERENCES {refTable}(TaxTypeId) ON DELETE CASCADE";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and {TableName}",
                    e);
            }
        }
    }
}