using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models.Accounting;
using Serilog;

namespace FinancialAnalysis.Datalayer.Accounting
{
    public class Cashbacks : ITable
    {
        private readonly CashbacksStoredProcedures sp = new CashbacksStoredProcedures();

        public Cashbacks()
        {
            TableName = "Cashbacks";
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
                    "CashbackId int IDENTITY(1,1) PRIMARY KEY," +
                    "Percentage money NOT NULL," +
                    "TimeValue int NOT NULL," +
                    "PayType int NOT NULL)";

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
        ///     Returns all Cashback records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Cashback> GetAll()
        {
            IEnumerable<Cashback> output = new List<Cashback>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<Cashback>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the Cashback item
        /// </summary>
        /// <param name="cashback"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(Cashback Cashback)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Percentage, @TimeValue, @PayType",
                        Cashback);
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
        ///     Inserts the list of Cashback items
        /// </summary>
        /// <param name="creditor"></param>
        public void Insert(IEnumerable<Cashback> Cashbacks)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var Cashback in Cashbacks)
                        Insert(Cashback);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns Cashback by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cashback GetById(int id)
        {
            var output = new Cashback();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<Cashback>($"dbo.{TableName}_GetById @CashbackId", new { CashbackId = id});
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