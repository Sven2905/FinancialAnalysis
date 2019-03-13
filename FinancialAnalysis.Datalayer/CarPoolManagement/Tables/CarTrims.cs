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
    public class CarTrims : ITable
    {
        private readonly CarTrimsStoredProcedures sp = new CarTrimsStoredProcedures();

        public CarTrims()
        {
            TableName = "CarTrims";
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
                    "(CarTrimId int IDENTITY(1,1) PRIMARY KEY," +
                    "Name nvarchar(150) NOT NULL, " +
                    "RefCarGenerationId int, " +
                    "Year int)";

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
        ///     Returns all CarTrim records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CarTrim> GetAll()
        {
            IEnumerable<CarTrim> output = new List<CarTrim>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<CarTrim>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the CarTrim item
        /// </summary>
        /// <param name="CarTrim"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(CarTrim CarTrim)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Name, @RefCarGenerationId, @Year ", CarTrim);
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
        ///     Inserts the list of CarTrim items
        /// </summary>
        /// <param name="creditor"></param>
        public void Insert(IEnumerable<CarTrim> CarTrims)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var CarTrim in CarTrims) Insert(CarTrim);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns CarTrim by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<CarTrim> GetByRefCarGenerationId(int id)
        {
            IEnumerable<CarTrim> output = new List<CarTrim>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<CarTrim>(
                        $"dbo.{TableName}_GetByRefCarGenerationId @RefCarGenerationId", new { RefCarGenerationId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetByRefCarGenerationId' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update CarTrim, if not exist, insert it
        /// </summary>
        /// <param name="CarTrim"></param>
        public void UpdateOrInsert(CarTrim CarTrim)
        {
            if (CarTrim.CarTrimId == 0)
            {
                Insert(CarTrim);
                return;
            }

            Update(CarTrim);
        }

        /// <summary>
        ///     Update CarTrims, if not exist insert them
        /// </summary>
        /// <param name="User"></param>
        public void UpdateOrInsert(IEnumerable<CarTrim> CarTrims)
        {
            foreach (var CarTrim in CarTrims) UpdateOrInsert(CarTrim);
        }

        /// <summary>
        ///     Update CarTrim
        /// </summary>
        /// <param name="CarTrim"></param>
        public void Update(CarTrim CarTrim)
        {
            if (CarTrim.CarTrimId == 0) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @CarTrimId, @Name, @RefCarModelId, @Year",
                        CarTrim);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete CarTrim by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @CarTrimId", new {CarTrimId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }
    }
}