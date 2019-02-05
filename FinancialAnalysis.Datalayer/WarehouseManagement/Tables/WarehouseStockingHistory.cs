using Dapper;
using FinancialAnalysis.Models.ProductManagement;
using FinancialAnalysis.Models.WarehouseManagement;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Utilities;

namespace FinancialAnalysis.Datalayer.WarehouseManagement
{
    public class WarehouseStockingHistories : ITable
    {
        private readonly WarehouseStockingHistoriesStoredProcedures sp = new WarehouseStockingHistoriesStoredProcedures();

        public WarehouseStockingHistories()
        {
            TableName = "WarehouseStockingHistories";
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
                    "WarehouseStockingHistoryId int IDENTITY(1,1) PRIMARY KEY, " +
                    "RefProductId int, " +
                    "ProductName nvarchar(150)," +
                    "RefStockyardId int, " +
                    "StockyardName nvarchar(150)," +
                    "Quantity int, " +
                    "RefUserId int, " +
                    "UserName nvarchar(150)," +
                    "Date datetime)";

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
        ///     Returns all WarehouseStockingHistory records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WarehouseStockingHistory> GetAll()
        {
            var WarehouseStockingHistoryDictionary = new Dictionary<int, WarehouseStockingHistory>();
            using (var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                var query = con.Query<WarehouseStockingHistory, Product, Stockyard, WarehouseStockingHistory>
                    ($"dbo.{TableName}_GetAll",
                        (w, p, s) =>
                        {
                            if (!WarehouseStockingHistoryDictionary.TryGetValue(w.WarehouseStockingHistoryId, out var WarehouseStockingHistoryEntry))
                            {
                                WarehouseStockingHistoryEntry = w;
                                WarehouseStockingHistoryDictionary.Add(WarehouseStockingHistoryEntry.WarehouseStockingHistoryId, WarehouseStockingHistoryEntry);
                            }

                            if (p != null)
                            {
                                WarehouseStockingHistoryEntry.Product = p;
                            }

                            if (s != null)
                            {
                                WarehouseStockingHistoryEntry.Stockyard = s;
                            }

                            return WarehouseStockingHistoryEntry;
                        }, splitOn: "WarehouseStockingHistoryId, ProductId, StockyardId")
                    .AsQueryable();
                return WarehouseStockingHistoryDictionary.Values.ToList();
            }
        }

        /// <summary>
        ///     Returns all WarehouseStockingHistory records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WarehouseStockingHistory> GetLast10(int RefProductId, int RefStockyardId)
        {
            var WarehouseStockingHistoryDictionary = new Dictionary<int, WarehouseStockingHistory>();
            using (var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                var query = con.Query<WarehouseStockingHistory, Product, Stockyard, WarehouseStockingHistory>
                    ($"dbo.{TableName}_GetLast10 @RefProductId, @RefStockyardId",
                        (w, p, s) =>
                        {
                            if (!WarehouseStockingHistoryDictionary.TryGetValue(w.WarehouseStockingHistoryId, out var WarehouseStockingHistoryEntry))
                            {
                                WarehouseStockingHistoryEntry = w;
                                WarehouseStockingHistoryDictionary.Add(WarehouseStockingHistoryEntry.WarehouseStockingHistoryId, WarehouseStockingHistoryEntry);
                            }

                            if (p != null)
                            {
                                WarehouseStockingHistoryEntry.Product = p;
                            }

                            if (s != null)
                            {
                                WarehouseStockingHistoryEntry.Stockyard = s;
                            }

                            return WarehouseStockingHistoryEntry;
                        }, new { RefProductId, RefStockyardId }, splitOn: "WarehouseStockingHistoryId, ProductId, StockyardId")
                    .AsQueryable();
                return WarehouseStockingHistoryDictionary.Values.ToList();
            }
        }

        /// <summary>
        ///     Inserts the WarehouseStockingHistory item
        /// </summary>
        /// <param name="WarehouseStockingHistory"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(WarehouseStockingHistory WarehouseStockingHistory)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>(
                        $"dbo.{TableName}_Insert @RefProductId, @ProductName, @RefStockyardId, @StockyardName, @Quantity, @RefUserId, @UserName, @Date",
                        WarehouseStockingHistory);
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
        ///     Inserts the list of WarehouseStockingHistory items
        /// </summary>
        /// <param name="WarehouseStockingHistories"></param>
        public void Insert(IEnumerable<WarehouseStockingHistory> WarehouseStockingHistories)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var WarehouseStockingHistory in WarehouseStockingHistories)
                    {
                        Insert(WarehouseStockingHistory);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns Employee by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WarehouseStockingHistory GetById(int id)
        {
            var WarehouseStockingHistoryDictionary = new Dictionary<int, WarehouseStockingHistory>();
            using (var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                var query = con.Query<WarehouseStockingHistory, Product, Stockyard, WarehouseStockingHistory>
                    ($"dbo.{TableName}_GetById @WarehouseStockingHistoryId",
                        (w, p, s) =>
                        {
                            if (!WarehouseStockingHistoryDictionary.TryGetValue(w.WarehouseStockingHistoryId, out var WarehouseStockingHistoryEntry))
                            {
                                WarehouseStockingHistoryEntry = w;
                                WarehouseStockingHistoryDictionary.Add(WarehouseStockingHistoryEntry.WarehouseStockingHistoryId, WarehouseStockingHistoryEntry);
                            }

                            if (p != null)
                            {
                                WarehouseStockingHistoryEntry.Product = p;
                            }

                            if (s != null)
                            {
                                WarehouseStockingHistoryEntry.Stockyard = s;
                            }

                            return w;
                        }, new { WarehouseStockingHistoryId = id }, splitOn: "WarehouseStockingHistoryId, ProductId, StockyardId")
                    .AsQueryable();
                return query.FirstOrDefault();
            }
        }

        /// <summary>
        ///     Update WarehouseStockingHistory, if not exist, insert it
        /// </summary>
        /// <param name="WarehouseStockingHistory"></param>
        public void UpdateOrInsert(WarehouseStockingHistory WarehouseStockingHistory)
        {
            if (WarehouseStockingHistory.WarehouseStockingHistoryId == 0 || GetById(WarehouseStockingHistory.WarehouseStockingHistoryId) is null)
            {
                Insert(WarehouseStockingHistory);
                return;
            }

            Update(WarehouseStockingHistory);
        }

        /// <summary>
        ///     Update WarehouseStockingHistories, if not exist insert them
        /// </summary>
        /// <param name="WarehouseStockingHistories"></param>
        public void UpdateOrInsert(IEnumerable<WarehouseStockingHistory> WarehouseStockingHistories)
        {
            foreach (var WarehouseStockingHistory in WarehouseStockingHistories)
            {
                UpdateOrInsert(WarehouseStockingHistory);
            }
        }

        /// <summary>
        ///     Update WarehouseStockingHistory
        /// </summary>
        /// <param name="WarehouseStockingHistory"></param>
        public void Update(WarehouseStockingHistory WarehouseStockingHistory)
        {
            if (WarehouseStockingHistory.WarehouseStockingHistoryId == 0 || GetById(WarehouseStockingHistory.WarehouseStockingHistoryId) is null)
            {
                return;
            }

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @WarehouseStockingHistoryId, @RefProductId, @ProductName, @RefStockyardId, @StockyardName, @Quantity, @RefUserId, @UserName, @Date",
                        WarehouseStockingHistory);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete WarehouseStockingHistory by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @WarehouseStockingHistoryId", new { WarehouseStockingHistoryId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }
    }
}