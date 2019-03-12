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
    public class CarGenerations : ITable
    {
        private readonly CarGenerationsStoredProcedures sp = new CarGenerationsStoredProcedures();

        public CarGenerations()
        {
            TableName = "CarGenerations";
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
                    "(CarGenerationId int IDENTITY(1,1) PRIMARY KEY," +
                    "Name nvarchar(150) NOT NULL, " +
                    "RefCarBodyId int)";

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
        ///     Returns all CarGeneration records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CarGeneration> GetAll()
        {
            IEnumerable<CarGeneration> output = new List<CarGeneration>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<CarGeneration>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the CarGeneration item
        /// </summary>
        /// <param name="CarGeneration"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(CarGeneration CarGeneration)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Name, @RefCarBodyId ", CarGeneration);
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
        ///     Inserts the list of CarGeneration items
        /// </summary>
        /// <param name="creditor"></param>
        public void Insert(IEnumerable<CarGeneration> CarGenerations)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var CarGeneration in CarGenerations) Insert(CarGeneration);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns CarGeneration by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CarGeneration GetByRefCarBodyId(int id)
        {
            var output = new CarGeneration();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<CarGeneration>(
                        $"dbo.{TableName}_GetByRefCarBodyId @RefCarBodyId", new { RefCarBodyId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetByRefCarBodyId' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update CarGeneration, if not exist, insert it
        /// </summary>
        /// <param name="CarGeneration"></param>
        public void UpdateOrInsert(CarGeneration CarGeneration)
        {
            if (CarGeneration.CarGenerationId == 0)
            {
                Insert(CarGeneration);
                return;
            }

            Update(CarGeneration);
        }

        /// <summary>
        ///     Update CarGenerations, if not exist insert them
        /// </summary>
        /// <param name="User"></param>
        public void UpdateOrInsert(IEnumerable<CarGeneration> CarGenerations)
        {
            foreach (var CarGeneration in CarGenerations) UpdateOrInsert(CarGeneration);
        }

        /// <summary>
        ///     Update CarGeneration
        /// </summary>
        /// <param name="CarGeneration"></param>
        public void Update(CarGeneration CarGeneration)
        {
            if (CarGeneration.CarGenerationId == 0) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @CarGenerationId, @Name, @RefCarBodyId",
                        CarGeneration);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete CarGeneration by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @CarGenerationId", new {CarGenerationId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }
    }
}