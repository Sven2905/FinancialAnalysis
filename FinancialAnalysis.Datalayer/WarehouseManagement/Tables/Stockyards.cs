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
    public class Stockyards : ITable
    {
        private readonly StockyardsStoredProcedures sp = new StockyardsStoredProcedures();

        public Stockyards()
        {
            TableName = "Stockyards";
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
                    "StockyardId int IDENTITY(1,1) PRIMARY KEY, " +
                    "Name nvarchar(150) NOT NULL," +
                    "RefWarehouseId int NOT NULL";

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
        ///     Returns all Stockyard records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Stockyard> GetAll()
        {
            IEnumerable<Stockyard> output = new List<Stockyard>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<Stockyard>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the Stockyard item
        /// </summary>
        /// <param name="Stockyard"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(Stockyard Stockyard)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>(
                        $"dbo.{TableName}_Insert @Name, @RefWarehouseId",
                        Stockyard);
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
        ///     Inserts the list of Stockyard items
        /// </summary>
        /// <param name="Stockyards"></param>
        public void Insert(IEnumerable<Stockyard> Stockyards)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var Stockyard in Stockyards) Insert(Stockyard);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns Stockyard by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Stockyard GetById(int id)
        {
            var output = new Stockyard();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<Stockyard>($"dbo.{TableName}_GetById @StockyardId",
                        new { StockyardId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update Stockyard, if not exist, insert it
        /// </summary>
        /// <param name="Stockyard"></param>
        public void UpdateOrInsert(Stockyard Stockyard)
        {
            if (Stockyard.StockyardId == 0 || GetById(Stockyard.StockyardId) is null)
            {
                Insert(Stockyard);
                return;
            }

            Update(Stockyard);
        }

        /// <summary>
        ///     Update Stockyards, if not exist insert them
        /// </summary>
        /// <param name="Stockyards"></param>
        public void UpdateOrInsert(IEnumerable<Stockyard> Stockyards)
        {
            foreach (var Stockyard in Stockyards) UpdateOrInsert(Stockyard);
        }

        /// <summary>
        ///     Update Stockyard
        /// </summary>
        /// <param name="Stockyard"></param>
        public void Update(Stockyard Stockyard)
        {
            if (Stockyard.StockyardId == 0 || GetById(Stockyard.StockyardId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @StockyardId, @Name, @RefWarehouseId", Stockyard);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete Stockyard by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @StockyardId", new { StockyardId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        public void AddReferences()
        {
            AddWarehousesReference();
        }

        private void AddWarehousesReference()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_Warehouses', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_Warehouses FOREIGN KEY(RefWarehouseId) REFERENCES Warehouses(WarehouseId)";

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