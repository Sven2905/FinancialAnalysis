using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.ClientManagement;
using Serilog;

namespace FinancialAnalysis.Datalayer.Accounting
{
    public class Creditors : ITable
    {
        private readonly CreditorsStoredProcedures sp = new CreditorsStoredProcedures();

        public Creditors()
        {
            TableName = "Creditors";
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
                    "CreditorId int IDENTITY(1,1) PRIMARY KEY," +
                    "RefClientId int NOT NULL," +
                    "RefCostAccountId int NOT NULL)";

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
        ///     Returns all Creditor records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Creditor> GetAll()
        {
            IEnumerable<Creditor> output = new List<Creditor>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<Creditor, Client, Company, CostAccount, Creditor>($"dbo.{TableName}_GetAll",
                        (creditor, Client, company, costaccount) =>
                        {
                            creditor.Client = Client;
                            creditor.Client.Company = company;
                            creditor.CostAccount = costaccount;
                            return creditor;
                        }, splitOn: "CreditorId, ClientId, CompanyId, CostAccountId",
                        commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the Creditor item
        /// </summary>
        /// <param name="creditor"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(Creditor creditor)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @RefClientId, @RefCostAccountId", creditor);
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
        ///     Inserts the list of Creditor items
        /// </summary>
        /// <param name="creditor"></param>
        public void Insert(IEnumerable<Creditor> creditors)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var creditor in creditors) Insert(creditor);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns Creditor by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Creditor GetById(int id)
        {
            var output = new Creditor();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<Creditor>($"dbo.{TableName}_GetById @CreditorId",
                        new {CreditorId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update Creditor, if not exist, insert it
        /// </summary>
        /// <param name="creditor"></param>
        public void UpdateOrInsert(Creditor creditor)
        {
            if (creditor.CreditorId == 0 || GetById(creditor.CreditorId) is null)
            {
                Insert(creditor);
                return;
            }

            Update(creditor);
        }

        /// <summary>
        ///     Update Creditors, if not exist insert them
        /// </summary>
        /// <param name="creditor"></param>
        public void UpdateOrInsert(IEnumerable<Creditor> creditors)
        {
            foreach (var creditor in creditors) UpdateOrInsert(creditor);
        }

        /// <summary>
        ///     Update Creditor
        /// </summary>
        /// <param name="creditor"></param>
        public void Update(Creditor creditor)
        {
            if (creditor.CreditorId == 0 || GetById(creditor.CreditorId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @RefClientId, @RefCostAccountId", creditor);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete Creditor by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @CreditorId", new {CreditorId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        public void AddReferences()
        {
            AddCostCompaniesReference();
            AddCostAccountsReference();
        }

        private void AddCostAccountsReference()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_Creditors_CostAccounts', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_Creditors_CostAccounts FOREIGN KEY(RefCostAccountId) REFERENCES CostAccounts(CostAccountId)";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and CostAccounts", e);
            }
        }

        private void AddCostCompaniesReference()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_Creditors_Clients', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_Creditors_Clients FOREIGN KEY(RefClientId) REFERENCES Clients(ClientId)";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and Clients", e);
            }
        }

        /// <summary>
        ///     Checks if Creditor has Cost Accounts
        /// </summary>
        /// <param name="id"></param>
        public bool IsCreditorInUse(int id)
        {
            var IsInUse = true;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    IsInUse = con.ExecuteScalar<bool>($"dbo.{TableName}_IsCreditorInUse @CreditorId",
                        new {CreditorId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'IsCreditorInUse' from table '{TableName}'", e);
            }

            return IsInUse;
        }
    }
}