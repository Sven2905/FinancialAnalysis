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
    public class ProductPrototypes : ITable
    {
        private readonly ProductPrototypesStoredProcedures sp = new ProductPrototypesStoredProcedures();

        public ProductPrototypes()
        {
            TableName = "ProductPrototypes";
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
                    "ProductPrototypeId int IDENTITY(1,1) PRIMARY KEY," +
                    "Name nvarchar(150) NOT NULL," +
                    "Description nvarchar(150)," +
                    "DimensionX int, " +
                    "DimensionY int, " +
                    "DimensionZ int, " +
                    "Weight decimal, " +
                    "IsStackable bit, " +
                    "RefProductCategory int NOT NULL)";

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
        ///     Returns all Product Prototypes records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductPrototype> GetAll()
        {
            IEnumerable<ProductPrototype> output = new List<ProductPrototype>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<ProductPrototype>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the ProductPrototype item
        /// </summary>
        /// <param name="ProductPrototype"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(ProductPrototype ProductPrototype)
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
                            ProductPrototype);
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
        ///     Inserts the list of ProductPrototypes items
        /// </summary>
        /// <param name="ProductPrototype"></param>
        public void Insert(IEnumerable<ProductPrototype> ProductPrototypes)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var ProductPrototype in ProductPrototypes) Insert(ProductPrototype);
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
        public ProductPrototype GetById(int id)
        {
            var output = new ProductPrototype();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<ProductPrototype>($"dbo.{TableName}_GetById @ProductPrototypeId",
                        new {ProductPrototypeId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        public void AddReferences()
        {
            AddCostAccountsReference();
        }

        private void AddCostAccountsReference()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_ProductPrototype_ProductCategory', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_ProductPrototype_ProductCategory FOREIGN KEY(RefProductCategory) REFERENCES ProductCategories(ProductCategoryId)";

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
    }
}