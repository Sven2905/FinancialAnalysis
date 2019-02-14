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
    public class SalesOrderPositions : ITable
    {
        private readonly SalesOrderPositionsStoredProcedures sp = new SalesOrderPositionsStoredProcedures();

        public SalesOrderPositions()
        {
            TableName = "SalesOrderPositions";
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
                                 "(SalesOrderPositionId int IDENTITY(1,1) PRIMARY KEY, " +
                                 "RefSalesOrderId int, " +
                                 "RefProductId int, " +
                                 "Description nvarchar(150), " +
                                 "Quantity int, " +
                                 "Price money, " +
                                 "DiscountPercentage money, " +
                                 "IsShipped bit, " +
                                 "IsCanceled bit)";

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
        ///     Returns all SalesOrderPosition records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SalesOrderPosition> GetAll()
        {
            IEnumerable<SalesOrderPosition> output = new List<SalesOrderPosition>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<SalesOrderPosition>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the SalesOrderPosition item
        /// </summary>
        /// <param name="SalesOrderPosition"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(SalesOrderPosition SalesOrderPosition)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>(
                        $"dbo.{TableName}_Insert @RefSalesOrderId, @RefProductId, @Description, @Quantity, @Price, @DiscountPercentage, @IsShipped, @IsCanceled ",
                        SalesOrderPosition);
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
        ///     Inserts the list of SalesOrderPosition items
        /// </summary>
        /// <param name="SalesOrderPositions"></param>
        public void Insert(IEnumerable<SalesOrderPosition> SalesOrderPositions)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var SalesOrderPosition in SalesOrderPositions) Insert(SalesOrderPosition);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns SalesOrderPosition by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SalesOrderPosition GetById(int id)
        {
            var output = new SalesOrderPosition();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<SalesOrderPosition>(
                        $"dbo.{TableName}_GetById @SalesOrderPositionId", new {SalesOrderPositionId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update SalesOrderPosition, if not exist, insert it
        /// </summary>
        /// <param name="SalesOrderPosition"></param>
        public void UpdateOrInsert(SalesOrderPosition SalesOrderPosition)
        {
            if (SalesOrderPosition.SalesOrderPositionId == 0 ||
                GetById(SalesOrderPosition.SalesOrderPositionId) is null)
            {
                Insert(SalesOrderPosition);
                return;
            }

            Update(SalesOrderPosition);
        }

        /// <summary>
        ///     Update SalesOrderPositions, if not exist insert them
        /// </summary>
        /// <param name="SalesOrderPositions"></param>
        public void UpdateOrInsert(IEnumerable<SalesOrderPosition> SalesOrderPositions)
        {
            foreach (var SalesOrderPosition in SalesOrderPositions) UpdateOrInsert(SalesOrderPosition);
        }

        /// <summary>
        ///     Update SalesOrderPosition
        /// </summary>
        /// <param name="SalesOrderPosition"></param>
        public void Update(SalesOrderPosition SalesOrderPosition)
        {
            if (SalesOrderPosition.SalesOrderPositionId == 0 ||
                GetById(SalesOrderPosition.SalesOrderPositionId) is null)
                return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute(
                        $"dbo.{TableName}_Update @SalesOrderPositionId, @RefSalesOrderId, @RefProductId, @Description, @Quantity, @Price, @DiscountPercentage, @IsShipped, @IsCanceled ",
                        SalesOrderPosition);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete SalesOrderPosition by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @SalesOrderPositionId", new {SalesOrderPositionId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete SalesOrderPosition by Item
        /// </summary>
        /// <param name="id"></param>
        public void Delete(SalesOrderPosition SalesOrderPosition)
        {
            Delete(SalesOrderPosition.SalesOrderPositionId);
        }

        public void AddReferences()
        {
            AddProductsReference();
            AddSalesOrdersReference();
        }

        private void AddProductsReference()
        {
            var refTable = "Products";

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
                Log.Error($"Exception occured while creating reference between '{TableName}' and {refTable}",
                    e);
            }
        }

        private void AddSalesOrdersReference()
        {
            var refTable = "SalesOrders";

            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_{refTable}', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_{refTable} FOREIGN KEY(RefSalesOrderId) REFERENCES {refTable}(SalesOrderId) ON DELETE CASCADE";

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