using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models.ProjectManagement;
using Serilog;

namespace FinancialAnalysis.Datalayer.ProjectManagement
{
    public class HealthInsurances : ITable
    {
        private readonly HealthInsurancesStoredProcedures sp = new HealthInsurancesStoredProcedures();

        public HealthInsurances()
        {
            TableName = "HealthInsurances";
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
                    "HealthInsuranceId int IDENTITY(1,1) PRIMARY KEY," +
                    "Name nvarchar(150) NOT NULL," +
                    "Street nvarchar(150)," +
                    "City nvarchar(150)," +
                    "Postcode int," +
                    "ContactName nvarchar(150)," +
                    "Phone nvarchar(150)," +
                    "Mail nvarchar(150))";

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
        ///     Returns all HealthInsurance records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HealthInsurance> GetAll()
        {
            IEnumerable<HealthInsurance> output = new List<HealthInsurance>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<HealthInsurance>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the HealthInsurance item
        /// </summary>
        /// <param name="HealthInsurance"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(HealthInsurance HealthInsurance)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result =
                        con.Query<int>(
                            $"dbo.{TableName}_Insert @Name, @Description, @Budget, @StartDate, @ExpectedEndDate, @TotalEndDate, @IsEnded, @RefCostCenterId, @RefEmployeeId",
                            HealthInsurance);
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
        ///     Inserts the list of HealthInsurance items
        /// </summary>
        /// <param name="HealthInsurance"></param>
        public void Insert(IEnumerable<HealthInsurance> HealthInsurances)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var HealthInsurance in HealthInsurances) Insert(HealthInsurance);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns HealthInsurance by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HealthInsurance GetById(int id)
        {
            var output = new HealthInsurance();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<HealthInsurance>($"dbo.{TableName}_GetById @HealthInsuranceId",
                        new {HealthInsuranceId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }
    }
}