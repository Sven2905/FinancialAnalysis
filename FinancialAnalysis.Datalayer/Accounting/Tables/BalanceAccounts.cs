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
    public class BalanceAccounts : ITable
    {
        private readonly BalanceAccountsStoredProcedures sp = new BalanceAccountsStoredProcedures();

        public BalanceAccounts()
        {
            TableName = "BalanceAccounts";
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
                    "(BalanceAccountId int IDENTITY(1,1) PRIMARY KEY, " +
                    "Name nvarchar(MAX) NOT NULL, " +
                    "ParentId int, " +
                    "AccountType int)";

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
        ///     Returns all BalanceAccount records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BalanceAccount> GetAll()
        {
            IEnumerable<BalanceAccount> output = new List<BalanceAccount>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<BalanceAccount>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the BalanceAccount item
        /// </summary>
        /// <param name="BalanceAccount"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(BalanceAccount BalanceAccount)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Name, @ParentId, @AccountType ", BalanceAccount);
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
        ///     Inserts the list of BalanceAccount items
        /// </summary>
        /// <param name="creditor"></param>
        public void Insert(IEnumerable<BalanceAccount> BalanceAccounts)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var BalanceAccount in BalanceAccounts) Insert(BalanceAccount);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns BalanceAccount by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BalanceAccount GetById(int id)
        {
            var output = new BalanceAccount();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<BalanceAccount>($"dbo.{TableName}_GetById @BalanceAccountId",
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
        ///     Update BalanceAccount, if not exist, insert it
        /// </summary>
        /// <param name="BalanceAccount"></param>
        public void UpdateOrInsert(BalanceAccount BalanceAccount)
        {
            if (BalanceAccount.BalanceAccountId == 0 ||
                GetById(BalanceAccount.BalanceAccountId) is null)
            {
                Insert(BalanceAccount);
                return;
            }

            Update(BalanceAccount);
        }

        /// <summary>
        ///     Update BalanceAccounts, if not exist insert them
        /// </summary>
        /// <param name="User"></param>
        public void UpdateOrInsert(IEnumerable<BalanceAccount> BalanceAccounts)
        {
            foreach (var BalanceAccount in BalanceAccounts) UpdateOrInsert(BalanceAccount);
        }

        /// <summary>
        ///     Update BalanceAccount
        /// </summary>
        /// <param name="BalanceAccount"></param>
        public void Update(BalanceAccount BalanceAccount)
        {
            if (BalanceAccount.BalanceAccountId == 0 ||
                GetById(BalanceAccount.BalanceAccountId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @BalanceAccountId, @Name, @ParentId, @AccountType",
                        BalanceAccount);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete BalanceAccount by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @BalanceAccountId", new { BalanceAccountId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }
    }
}