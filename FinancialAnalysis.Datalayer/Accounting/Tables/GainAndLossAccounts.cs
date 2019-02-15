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
    public class GainAndLossAccounts : ITable
    {
        private readonly GainAndLossAccountsStoredProcedures sp = new GainAndLossAccountsStoredProcedures();

        public GainAndLossAccounts()
        {
            TableName = "GainAndLossAccounts";
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
                    $"CREATE TABLE {TableName}" +
                    "(GainAndLossAccountId int IDENTITY(1,1) PRIMARY KEY, " +
                    "Name nvarchar(MAX) NOT NULL, " +
                    "ParentId int)";

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
        ///     Returns all GainAndLossAccount records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GainAndLossAccount> GetAll()
        {
            IEnumerable<GainAndLossAccount> output = new List<GainAndLossAccount>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<GainAndLossAccount>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the GainAndLossAccount item
        /// </summary>
        /// <param name="GainAndLossAccount"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(GainAndLossAccount GainAndLossAccount)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Name, @ParentId ", GainAndLossAccount);
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
        ///     Inserts the list of GainAndLossAccount items
        /// </summary>
        /// <param name="creditor"></param>
        public void Insert(IEnumerable<GainAndLossAccount> GainAndLossAccounts)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var GainAndLossAccount in GainAndLossAccounts) Insert(GainAndLossAccount);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns GainAndLossAccount by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GainAndLossAccount GetById(int id)
        {
            var output = new GainAndLossAccount();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<GainAndLossAccount>($"dbo.{TableName}_GetById @GainAndLossAccountId",
                        new { CashbackId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update GainAndLossAccount, if not exist, insert it
        /// </summary>
        /// <param name="GainAndLossAccount"></param>
        public void UpdateOrInsert(GainAndLossAccount GainAndLossAccount)
        {
            if (GainAndLossAccount.GainAndLossAccountId == 0 ||
                GetById(GainAndLossAccount.GainAndLossAccountId) is null)
            {
                Insert(GainAndLossAccount);
                return;
            }

            Update(GainAndLossAccount);
        }

        /// <summary>
        ///     Update GainAndLossAccounts, if not exist insert them
        /// </summary>
        /// <param name="User"></param>
        public void UpdateOrInsert(IEnumerable<GainAndLossAccount> GainAndLossAccounts)
        {
            foreach (var GainAndLossAccount in GainAndLossAccounts) UpdateOrInsert(GainAndLossAccount);
        }

        /// <summary>
        ///     Update GainAndLossAccount
        /// </summary>
        /// <param name="GainAndLossAccount"></param>
        public void Update(GainAndLossAccount GainAndLossAccount)
        {
            if (GainAndLossAccount.GainAndLossAccountId == 0 ||
                GetById(GainAndLossAccount.GainAndLossAccountId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @GainAndLossAccountId, @Name, @ParentId",
                        GainAndLossAccount);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete GainAndLossAccount by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @GainAndLossAccountId", new { GainAndLossAccountId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }
    }
}