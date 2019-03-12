using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.CarPoolManagement;
using Serilog;

namespace FinancialAnalysis.Datalayer.Accounting
{
    public class CarEngines : ITable
    {
        private readonly CarEnginesStoredProcedures sp = new CarEnginesStoredProcedures();

        public CarEngines()
        {
            TableName = "CarEngines";
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
                    $"If not exists (select name from sysobjects where name = '{TableName}') CREATE TABLE {TableName}" +
                    "(CarEngineId int IDENTITY(1,1) PRIMARY KEY," +
                    "Volume int, " +
                    "Power int, " +
                    "EngineType int, " +
                    "CarGear int, " +
                    "RefCarTrimId int)";

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
        ///     Returns all CarEngine records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CarEngine> GetAll()
        {
            IEnumerable<CarEngine> output = new List<CarEngine>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<CarEngine>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the CarEngine item
        /// </summary>
        /// <param name="CarEngine"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(CarEngine CarEngine)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Volume, @Power, @EngineType, @CarGear, @RefCarTrimId ", CarEngine);
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
        ///     Inserts the list of CarEngine items
        /// </summary>
        /// <param name="creditor"></param>
        public void Insert(IEnumerable<CarEngine> CarEngines)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var CarEngine in CarEngines) Insert(CarEngine);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns CarEngine by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CarEngine GetByRefCarTrimId(int id)
        {
            var output = new CarEngine();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<CarEngine>(
                        $"dbo.{TableName}_GetByRefCarTrimId @RefCarTrimId", new { RefCarTrimId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetByRefCarTrimId' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update CarEngine, if not exist, insert it
        /// </summary>
        /// <param name="CarEngine"></param>
        public void UpdateOrInsert(CarEngine CarEngine)
        {
            if (CarEngine.CarEngineId == 0)
            {
                Insert(CarEngine);
                return;
            }

            Update(CarEngine);
        }

        /// <summary>
        ///     Update CarEngines, if not exist insert them
        /// </summary>
        /// <param name="User"></param>
        public void UpdateOrInsert(IEnumerable<CarEngine> CarEngines)
        {
            foreach (var CarEngine in CarEngines) UpdateOrInsert(CarEngine);
        }

        /// <summary>
        ///     Update CarEngine
        /// </summary>
        /// <param name="CarEngine"></param>
        public void Update(CarEngine CarEngine)
        {
            if (CarEngine.CarEngineId == 0) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @CarEngineId, @Volume, @Power, @EngineType, @CarGear, @RefCarTrimId",
                        CarEngine);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete CarEngine by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @CarEngineId", new {CarEngineId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }
    }
}