using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models.WarehouseManagement;
using Serilog;
using Utilities;

namespace FinancialAnalysis.Datalayer.WarehouseManagement
{
    public class Warehouses : ITable
    {
        private readonly WarehousesStoredProcedures sp = new WarehousesStoredProcedures();

        public Warehouses()
        {
            TableName = "Warehouses";
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
                    "WarehouseId int IDENTITY(1,1) PRIMARY KEY, " +
                    "Name nvarchar(150) NOT NULL, " +
                    "Description nvarchar(150), " +
                    "Street nvarchar(150), " +
                    "City nvarchar(150), " +
                    "Postcode int)";

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
        ///     Returns all Warehouse records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Warehouse> GetAll()
        {
            var warehouseDictionary = new Dictionary<int, Warehouse>();
            using (var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                var query = con.Query<Warehouse, Stockyard, Warehouse>
                    ($"dbo.{TableName}_GetAll",
                        (w, s) =>
                        {
                            Warehouse warehouseEntry;

                            if (!warehouseDictionary.TryGetValue(w.WarehouseId, out warehouseEntry))
                            {
                                if (warehouseEntry == null)
                                {
                                    warehouseEntry = w;
                                warehouseDictionary.Add(warehouseEntry.WarehouseId, warehouseEntry);
                                warehouseEntry.Stockyards = new SvenTechCollection<Stockyard>();
                                }
                            }
                            if (s != null)
                            {
                                warehouseEntry.Stockyards.Add(s);
                            }

                            return warehouseEntry;
                        }, splitOn: "WarehouseId, StockyardId")
                    .AsQueryable();
                return warehouseDictionary.Values.ToList();
            }
        }

        /// <summary>
        ///     Inserts the Warehouse item
        /// </summary>
        /// <param name="Warehouse"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(Warehouse Warehouse)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>(
                        $"dbo.{TableName}_Insert @Name, @Description, @Street, @City, @Postcode",
                        Warehouse);
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
        ///     Inserts the list of Warehouse items
        /// </summary>
        /// <param name="Warehouses"></param>
        public void Insert(IEnumerable<Warehouse> Warehouses)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var Warehouse in Warehouses) Insert(Warehouse);
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
        public Warehouse GetById(int id)
        {
            var warehouseDictionary = new Dictionary<int, Warehouse>();
            using (var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                var query = con.Query<Warehouse, Stockyard, Warehouse>
                    ($"dbo.{TableName}_GetById",
                        (w, s) =>
                        {
                            Warehouse warehouseEntry;

                            if (!warehouseDictionary.TryGetValue(w.WarehouseId, out warehouseEntry))
                            {
                                warehouseEntry = w;
                                warehouseEntry.Stockyards = new SvenTechCollection<Stockyard>();
                                warehouseDictionary.Add(warehouseEntry.WarehouseId, warehouseEntry);
                            }

                            warehouseEntry.Stockyards.Add(s);

                            return w;
                        }, new { WarehouseId = id }, splitOn: "WarehouseId, StockyardId")
                    .AsQueryable();
                return query.FirstOrDefault();
            }
        }

        /// <summary>
        ///     Update Warehouse, if not exist, insert it
        /// </summary>
        /// <param name="Warehouse"></param>
        public void UpdateOrInsert(Warehouse Warehouse)
        {
            if (Warehouse.WarehouseId == 0 || GetById(Warehouse.WarehouseId) is null)
            {
                Insert(Warehouse);
                return;
            }

            Update(Warehouse);
        }

        /// <summary>
        ///     Update Warehouses, if not exist insert them
        /// </summary>
        /// <param name="Warehouses"></param>
        public void UpdateOrInsert(IEnumerable<Warehouse> Warehouses)
        {
            foreach (var Warehouse in Warehouses) UpdateOrInsert(Warehouse);
        }

        /// <summary>
        ///     Update Warehouse
        /// </summary>
        /// <param name="Warehouse"></param>
        public void Update(Warehouse Warehouse)
        {
            if (Warehouse.WarehouseId == 0 || GetById(Warehouse.WarehouseId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @WarehouseId, @Name, @Description, @Street, @City, @Postcode", Warehouse);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete Warehouse by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @WarehouseId", new { WarehouseId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }
    }
}