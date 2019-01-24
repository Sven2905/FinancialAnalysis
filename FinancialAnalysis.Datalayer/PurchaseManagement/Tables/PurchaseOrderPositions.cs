using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models.PurchaseManagement;
using Serilog;

namespace FinancialAnalysis.Datalayer.PurchaseManagement
{
    public class PurchaseOrderPositions : ITable
    {
        private readonly PurchaseOrderPositionsStoredProcedures sp = new PurchaseOrderPositionsStoredProcedures();

        public PurchaseOrderPositions()
        {
            TableName = "PurchaseOrderPositions";
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
                                 "(PurchaseOrderPositionId int IDENTITY(1,1) PRIMARY KEY, " +
                                 "RefPurchaseOrderId int, " +
                                 "RefProductId int, " +
                                 "RefTaxTypeId int, " +
                                 "Description nvarchar(150), " +
                                 "Quantity int, " +
                                 "Price money, " +
                                 "DiscountPercentage money, " +
                                 "IsDelivered bit, " +
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
        ///     Returns all PurchaseOrderPosition records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PurchaseOrderPosition> GetAll()
        {
            IEnumerable<PurchaseOrderPosition> output = new List<PurchaseOrderPosition>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<PurchaseOrderPosition>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the PurchaseOrderPosition item
        /// </summary>
        /// <param name="PurchaseOrderPosition"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(PurchaseOrderPosition PurchaseOrderPosition)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @RefPurchaseOrderId, @RefProductId, @RefTaxTypeId, @Description, @Quantity, @Price, @DiscountPercentage, @IsDelivered, @IsCanceled ",
                        PurchaseOrderPosition);
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
        ///     Inserts the list of PurchaseOrderPosition items
        /// </summary>
        /// <param name="PurchaseOrderPositions"></param>
        public void Insert(IEnumerable<PurchaseOrderPosition> PurchaseOrderPositions)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var PurchaseOrderPosition in PurchaseOrderPositions) Insert(PurchaseOrderPosition);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns PurchaseOrderPosition by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PurchaseOrderPosition GetById(int id)
        {
            var output = new PurchaseOrderPosition();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<PurchaseOrderPosition>(
                        $"dbo.{TableName}_GetById @PurchaseOrderPositionId", new {PurchaseOrderPositionId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update PurchaseOrderPosition, if not exist, insert it
        /// </summary>
        /// <param name="PurchaseOrderPosition"></param>
        public void UpdateOrInsert(PurchaseOrderPosition PurchaseOrderPosition)
        {
            if (PurchaseOrderPosition.PurchaseOrderPositionId == 0 ||
                GetById(PurchaseOrderPosition.PurchaseOrderPositionId) is null)
            {
                Insert(PurchaseOrderPosition);
                return;
            }

            Update(PurchaseOrderPosition);
        }

        /// <summary>
        ///     Update PurchaseOrderPositions, if not exist insert them
        /// </summary>
        /// <param name="PurchaseOrderPositions"></param>
        public void UpdateOrInsert(IEnumerable<PurchaseOrderPosition> PurchaseOrderPositions)
        {
            foreach (var PurchaseOrderPosition in PurchaseOrderPositions) UpdateOrInsert(PurchaseOrderPosition);
        }

        /// <summary>
        ///     Update PurchaseOrderPosition
        /// </summary>
        /// <param name="PurchaseOrderPosition"></param>
        public void Update(PurchaseOrderPosition PurchaseOrderPosition)
        {
            if (PurchaseOrderPosition.PurchaseOrderPositionId == 0 ||
                GetById(PurchaseOrderPosition.PurchaseOrderPositionId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @PurchaseOrderPositionId, @RefPurchaseOrderId, @RefProductId, @RefTaxTypeId, @Description, @Quantity, @Price, @DiscountPercentage, @IsDelivered, @IsCanceled ",
                        PurchaseOrderPosition);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete PurchaseOrderPosition by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @PurchaseOrderPositionId", new {PurchaseOrderPositionId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete PurchaseOrderPosition by Item
        /// </summary>
        /// <param name="id"></param>
        public void Delete(PurchaseOrderPosition PurchaseOrderPosition)
        {
            Delete(PurchaseOrderPosition.PurchaseOrderPositionId);
        }

        public void AddReferences()
        {
            AddTaxTypesReference();
            AddProductsReference();
            AddPurchaseOrdersReference();
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

        private void AddPurchaseOrdersReference()
        {
            string refTable = "PurchaseOrders";

            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_{refTable}', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_{refTable} FOREIGN KEY(RefPurchaseOrderId) REFERENCES {refTable}(PurchaseOrderId) ON DELETE CASCADE";

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