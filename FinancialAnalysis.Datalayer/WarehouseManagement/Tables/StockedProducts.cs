using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models.WarehouseManagement;
using Serilog;

namespace FinancialAnalysis.Datalayer.WarehouseManagement
{
    public class StockedProducts : ITable
    {
        private readonly StockedProductsStoredProcedures sp = new StockedProductsStoredProcedures();

        public StockedProducts()
        {
            TableName = "StockedProducts";
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
                    $"If not exists (select name from sysobjects where name = '{TableName}') " +
                    $"CREATE TABLE {TableName}(" +
                    "StockedProductId int IDENTITY(1,1) PRIMARY KEY, " +
                    "RefProductId int," +
                    "Quantity int," +
                    "RefStockyardId int)";

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
        ///     Returns all StockedProduct records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StockedProduct> GetAll()
        {
            IEnumerable<StockedProduct> output = new List<StockedProduct>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<StockedProduct>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Returns StockedProduct by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StockedProduct GetById(int id)
        {
            var output = new StockedProduct();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<StockedProduct>($"dbo.{TableName}_GetById @StockedProductId",
                        new { StockedProductId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Returns StockedProduct for RefStockyardId
        /// </summary>
        /// <param name="RefStockyardId"></param>
        /// <returns></returns>
        public IEnumerable<StockedProduct> GetByRefStockyardId(int RefStockyardId)
        {
            IEnumerable<StockedProduct> output = new List<StockedProduct>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<StockedProduct>($"dbo.{TableName}_GetByRefStockyardId @RefStockyardId",
                        new { RefStockyardId });
                }

            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetByRefWarehouseId' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the StockedProduct item
        /// </summary>
        /// <param name="StockedProduct"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(StockedProduct StockedProduct)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>(
                        $"dbo.{TableName}_Insert @RefProductId, @RefStockyardId, @Quantity",
                        StockedProduct);
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
        ///     Inserts the list of StockedProduct items
        /// </summary>
        /// <param name="StockedProducts"></param>
        public void Insert(IEnumerable<StockedProduct> StockedProducts)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var StockedProduct in StockedProducts) Insert(StockedProduct);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Update StockedProduct, if not exist, insert it
        /// </summary>
        /// <param name="StockedProduct"></param>
        public void UpdateOrInsert(StockedProduct StockedProduct)
        {
            if (StockedProduct.StockedProductId == 0 || GetById(StockedProduct.StockedProductId) is null)
            {
                Insert(StockedProduct);
                return;
            }

            Update(StockedProduct);
        }

        /// <summary>
        ///     Update StockedProducts, if not exist insert them
        /// </summary>
        /// <param name="StockedProducts"></param>
        public void UpdateOrInsert(IEnumerable<StockedProduct> StockedProducts)
        {
            foreach (var StockedProduct in StockedProducts) UpdateOrInsert(StockedProduct);
        }

        /// <summary>
        ///     Update StockedProduct
        /// </summary>
        /// <param name="StockedProduct"></param>
        public void Update(StockedProduct StockedProduct)
        {
            if (StockedProduct.StockedProductId == 0 || GetById(StockedProduct.StockedProductId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @StockedProductId, @RefProductId, @RefStockyardId, @Quantity", StockedProduct);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete StockedProduct by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @StockedProductId", new { StockedProductId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        public void AddReferences()
        {
            AddStockyardReference();
            AddProductsReference();
        }

        private void AddStockyardReference()
        {
            string refTable = "Stockyards";

            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_{refTable}', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_{refTable} FOREIGN KEY(RefStockyardId) REFERENCES {refTable}(StockyardId) ON DELETE CASCADE";

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

        private void AddProductsReference()
        {
            string refTable = "Products";

            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_{refTable}', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_{refTable} FOREIGN KEY(RefProductId) REFERENCES {refTable}(ProductId) ON DELETE CASCADE";

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