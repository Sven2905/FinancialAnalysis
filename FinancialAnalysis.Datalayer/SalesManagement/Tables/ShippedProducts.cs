using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models.SalesManagement;
using Serilog;

namespace FinancialAnalysis.Datalayer.SalesManagement
{
    public class ShippedProducts : ITable
    {
        private readonly ShippedProductsStoredProcedures sp = new ShippedProductsStoredProcedures();

        public ShippedProducts()
        {
            TableName = "ShippedProducts";
            CheckAndCreateTable();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs\\Tables.txt", rollingInterval: RollingInterval.Month)
                .CreateLogger();
        }

        public string TableName { get; }

        public void CheckAndCreateStoredProcedures()
        {
            sp.CheckAndCreateProcedures();
        }

        public void CheckAndCreateTable()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr = $"If not exists (select name from sysobjects where name = '{TableName}') " +
                                 $"CREATE TABLE {TableName}" +
                                 "(ShippedProductId int IDENTITY(1,1) PRIMARY KEY, " +
                                 "RefShipmentId int, " +
                                 "RefSalesOrderPositionId int, " +
                                 "Quantity int)";

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

        /// <summary>
        ///     Inserts the ShippedProduct item
        /// </summary>
        /// <param name="ShippedProduct"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(ShippedProduct ShippedProduct)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>(
                        $"dbo.{TableName}_Insert @RefShipmentId, @RefSalesOrderPositionId, @Quantity ",
                        ShippedProduct);
                    return result.Single();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' from table '{TableName}'", e);
            }

            return id;
        }

        /// <summary>
        ///     Inserts the list of ShippedProduct items
        /// </summary>
        /// <param name="ShippedProducts"></param>
        public void Insert(IEnumerable<ShippedProduct> ShippedProducts)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var ShippedProduct in ShippedProducts) Insert(ShippedProduct);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Update ShippedProduct, if not exist, insert it
        /// </summary>
        /// <param name="ShippedProduct"></param>
        public void UpdateOrInsert(ShippedProduct ShippedProduct)
        {
            if (ShippedProduct.ShippedProductId == 0)
            {
                Insert(ShippedProduct);
                return;
            }

            Update(ShippedProduct);
        }

        /// <summary>
        ///     Update ShippedProducts, if not exist insert them
        /// </summary>
        /// <param name="ShippedProducts"></param>
        public void UpdateOrInsert(IEnumerable<ShippedProduct> ShippedProducts)
        {
            foreach (var ShippedProduct in ShippedProducts) UpdateOrInsert(ShippedProduct);
        }

        /// <summary>
        ///     Update ShippedProduct
        /// </summary>
        /// <param name="ShippedProduct"></param>
        public void Update(ShippedProduct ShippedProduct)
        {
            if (ShippedProduct.ShippedProductId == 0) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute(
                        $"dbo.{TableName}_Update @ShippedProductId, @RefShipmentId, @RefSalesOrderPositionId, @Quantity ",
                        ShippedProduct);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete ShippedProduct by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @ShippedProductId", new {ShippedProductId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete ShippedProduct by Item
        /// </summary>
        /// <param name="id"></param>
        public void Delete(ShippedProduct ShippedProduct)
        {
            Delete(ShippedProduct.ShippedProductId);
        }

        public void AddReferences()
        {
            AddSalesOrderPositionsReference();
            AddShipmentsReference();
        }

        private void AddSalesOrderPositionsReference()
        {
            var refTable = "SalesOrderPositions";

            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_{refTable}', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_{refTable} FOREIGN KEY(RefSalesOrderPositionId) REFERENCES {refTable}(SalesOrderPositionId) ON DELETE CASCADE";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and {refTable}",
                    e);
            }
        }

        private void AddShipmentsReference()
        {
            var refTable = "Shipments";

            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_{refTable}', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_{refTable} FOREIGN KEY(RefShipmentId) REFERENCES {refTable}(ShipmentId) ON DELETE CASCADE";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and {refTable}",
                    e);
            }
        }
    }
}