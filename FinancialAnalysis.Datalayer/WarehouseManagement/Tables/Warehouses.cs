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
                var query = con.Query<Warehouse, Stockyard, StockedProduct, Product, Warehouse>
                    ($"dbo.{TableName}_GetAll",
                        (w, s, sp, p) =>
                        {
                            if (!warehouseDictionary.TryGetValue(w.WarehouseId, out var warehouseEntry))
                            {
                                warehouseEntry = w;
                                warehouseDictionary.Add(warehouseEntry.WarehouseId, warehouseEntry);
                                warehouseEntry.Stockyards = new SvenTechCollection<Stockyard>();
                            }

                            if (s != null)
                            {
                                if (warehouseEntry.Stockyards.All(x => x.StockyardId != s.StockyardId))
                                {
                                    warehouseEntry.Stockyards.Add(s);
                                }
                            }

                            if (sp != null)
                            {
                                sp.Product = p;
                                warehouseEntry.Stockyards.Single(x => s != null && x.StockyardId == s.StockyardId)
                                    .StockedProducts.Add(sp);
                            }

                            return warehouseEntry;
                        }, splitOn: "WarehouseId, StockyardId, StockedProductId, ProductId")
                    .AsQueryable();
                return warehouseDictionary.Values.ToList();
            }
        }

        public IEnumerable<Warehouse> GetAllWithoutStock()
        {
            var warehouseDictionary = new Dictionary<int, Warehouse>();
            using (var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                var query = con.Query<Warehouse, Stockyard, Warehouse>
                    ($"dbo.{TableName}_GetAllWithoutStock",
                        (w, s) =>
                        {
                            if (!warehouseDictionary.TryGetValue(w.WarehouseId, out var warehouseEntry))
                            {
                                warehouseEntry = w;
                                warehouseDictionary.Add(warehouseEntry.WarehouseId, warehouseEntry);
                                warehouseEntry.Stockyards = new SvenTechCollection<Stockyard>();
                            }

                            if (s != null)
                            {
                                if (warehouseEntry.Stockyards.All(x => x.StockyardId != s.StockyardId))
                                {
                                    warehouseEntry.Stockyards.Add(s);
                                }
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
        /// <param name="warehouse"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(Warehouse warehouse)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>(
                        $"dbo.{TableName}_Insert @Name, @Description, @Street, @City, @Postcode",
                        warehouse);
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
                    foreach (var Warehouse in Warehouses)
                    {
                        Insert(Warehouse);
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
        public Warehouse GetById(int id)
        {
            var warehouseDictionary = new Dictionary<int, Warehouse>();
            using (var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                var query = con.Query<Warehouse, Stockyard, StockedProduct, Product, Warehouse>
                    ($"dbo.{TableName}_GetById @WarehouseId",
                        (w, s, sp, p) =>
                        {
                            if (!warehouseDictionary.TryGetValue(w.WarehouseId, out var warehouseEntry))
                            {
                                warehouseEntry = w;
                                warehouseEntry.Stockyards = new SvenTechCollection<Stockyard>();
                                warehouseDictionary.Add(warehouseEntry.WarehouseId, warehouseEntry);
                            }

                            if (s != null)
                            {
                                if (warehouseEntry.Stockyards.All(x => x.StockyardId != s.StockyardId))
                                {
                                    warehouseEntry.Stockyards.Add(s);
                                }
                            }

                            if (sp != null)
                            {
                                sp.Product = p;
                                warehouseEntry.Stockyards.Single(x => s != null && x.StockyardId == s.StockyardId)
                                    .StockedProducts.Add(sp);
                            }

                            return w;
                        }, new { WarehouseId = id }, splitOn: "WarehouseId, StockyardId, StockedProductId, ProductId")
                    .AsQueryable();
                return query.FirstOrDefault();
            }
        }

        /// <summary>
        ///     Returns Warehouses by ProductId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Warehouse> GetByProductId(int ProductId)
        {
            var warehouseDictionary = new Dictionary<int, Warehouse>();
            using (var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                var query = con.Query<Warehouse, Stockyard, StockedProduct, Product, Warehouse>
                    ($"dbo.{TableName}_GetByProductId @ProductId",
                        (w, s, sp, p) =>
                        {
                            if (!warehouseDictionary.TryGetValue(w.WarehouseId, out var warehouseEntry))
                            {
                                warehouseEntry = w;
                                warehouseEntry.Stockyards = new SvenTechCollection<Stockyard>();
                                warehouseDictionary.Add(warehouseEntry.WarehouseId, warehouseEntry);
                            }

                            if (s != null)
                            {
                                if (warehouseEntry.Stockyards.All(x => x.StockyardId != s.StockyardId))
                                {
                                    warehouseEntry.Stockyards.Add(s);
                                }
                            }

                            if (sp != null)
                            {
                                sp.Product = p;
                                warehouseEntry.Stockyards.Single(x => s != null && x.StockyardId == s.StockyardId)
                                    .StockedProducts.Add(sp);
                            }

                            return w;
                        }, new { ProductId }, splitOn: "WarehouseId, StockyardId, StockedProductId, ProductId")
                    .AsQueryable();

                foreach (var item in warehouseDictionary.Values)
                {
                    item.Stockyards = item.Stockyards.OrderBy(x => x.Name).ToSvenTechCollection();
                }

                return warehouseDictionary.Values.ToList();
            }
        }

        /// <summary>
        ///     Update Warehouse, if not exist, insert it
        /// </summary>
        /// <param name="warehouse"></param>
        public void UpdateOrInsert(Warehouse warehouse)
        {
            if (warehouse.WarehouseId == 0 || GetById(warehouse.WarehouseId) is null)
            {
                Insert(warehouse);
                return;
            }

            Update(warehouse);
        }

        /// <summary>
        ///     Update Warehouses, if not exist insert them
        /// </summary>
        /// <param name="warehouses"></param>
        public void UpdateOrInsert(IEnumerable<Warehouse> warehouses)
        {
            foreach (var warehouse in warehouses)
            {
                UpdateOrInsert(warehouse);
            }
        }

        /// <summary>
        ///     Update Warehouse
        /// </summary>
        /// <param name="Warehouse"></param>
        public void Update(Warehouse Warehouse)
        {
            if (Warehouse.WarehouseId == 0 || GetById(Warehouse.WarehouseId) is null)
            {
                return;
            }

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @WarehouseId, @Name, @Description, @Street, @City, @Postcode",
                        Warehouse);
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