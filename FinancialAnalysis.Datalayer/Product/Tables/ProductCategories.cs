using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models.Product;
using Serilog;

namespace FinancialAnalysis.Datalayer.Product
{
    public class ProductCategories : ITable
    {
        private readonly ProductCategoriesStoredProcedures sp = new ProductCategoriesStoredProcedures();

        public ProductCategories()
        {
            TableName = "ProductCategories";
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
                    "ProductCategoryId int IDENTITY(1,1) PRIMARY KEY, " +
                    "Name nvarchar(150) NOT NULL, " +
                    "Description nvarchar(150))";

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
        ///     Returns all Product Category records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductCategory> GetAll()
        {
            IEnumerable<ProductCategory> output = new List<ProductCategory>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<ProductCategory>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the Product Category item
        /// </summary>
        /// <param name="Product Category"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(ProductCategory ProductCategory)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result =
                        con.Query<int>(
                            $"dbo.{TableName}_Insert @Name, @Description, @DimensionX, @DimensionY, @DimensionZ, @Weight, @IsStackable, @RefProductCategory",
                            ProductCategory);
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
        ///     Inserts the list of Product Category items
        /// </summary>
        /// <param name="ProductPrototype"></param>
        public void Insert(IEnumerable<ProductCategory> ProductCategories)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var ProductCategory in ProductCategories) Insert(ProductCategory);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns Product Category by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductCategory GetById(int id)
        {
            var output = new ProductCategory();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<ProductCategory>($"dbo.{TableName}_GetById @ProductPrototypeId",
                        new {ProductCategoryId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }
    }
}