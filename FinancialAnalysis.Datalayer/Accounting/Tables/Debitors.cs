using Dapper;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FinancialAnalysis.Datalayer.Accounting
{
    public class Debitors : ITable
    {
        public string TableName { get; }
        private DebitorsStoredProcedures sp = new DebitorsStoredProcedures();

        public Debitors()
        {
            TableName = "Debitors";
            CheckAndCreateTable();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs\\Tables.txt", rollingInterval: RollingInterval.Month)
                .CreateLogger();
        }

        public void CheckAndCreateTable()
        {
            try
            {
                SqlConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr = $"If not exists (select name from sysobjects where name = '{TableName}') CREATE TABLE {TableName}(" +
                                 $"DebitorId int IDENTITY(1,1) PRIMARY KEY," +
                                 $"RefCompanyId int NOT NULL," +
                                 $"RefCostAccountId int NOT NULL)";

                using (SqlCommand command = new SqlCommand(commandStr, con))
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
        /// Returns all Debitor records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Debitor> GetAll()
        {
            IEnumerable<Debitor> output = new List<Debitor>();
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<Debitor, Company, CostAccount, Debitor>($"dbo.{TableName}_GetAll",
                            (debitor, company, costaccount) => {
                                debitor.Company = company;
                                debitor.CostAccount = costaccount;
                                return debitor;
                            }, splitOn: "CompanyId, CostAccountId",
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
        /// Inserts the Debitor item
        /// </summary>
        /// <param name="debitor"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(Debitor debitor)
        {
            int id = 0;
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @RefCompanyId, @RefCostAccountId", debitor);
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
        /// Inserts the list of Debitor items
        /// </summary>
        /// <param name="debitor"></param>
        public void Insert(IEnumerable<Debitor> debitors)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var debitor in debitors)
                    {
                        Insert(debitor);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        /// Returns Debitor by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Debitor GetById(int id)
        {
            Debitor output = new Debitor();
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<Debitor>($"dbo.{TableName}_GetById @DebitorId", new { DebitorId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }
            return output;
        }

        /// <summary>
        /// Update Debitor, if not exist, insert it
        /// </summary>
        /// <param name="creditor"></param>
        public void UpdateOrInsert(Debitor debitor)
        {
            if (debitor.DebitorId == 0 || GetById(debitor.DebitorId) is null)
            {
                Insert(debitor);
                return;
            }

            Update(debitor);
        }

        /// <summary>
        /// Update Debitors, if not exist insert them
        /// </summary>
        /// <param name="creditor"></param>
        public void UpdateOrInsert(IEnumerable<Debitor> debitors)
        {
            foreach (var debitor in debitors)
            {
                UpdateOrInsert(debitor);
            }
        }

        /// <summary>
        /// Update Debitor
        /// </summary>
        /// <param name="creditor"></param>
        public void Update(Debitor debitor)
        {
            if (debitor.DebitorId == 0 || GetById(debitor.DebitorId) is null)
            {
                return;
            }

            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @RefCompanyId, @RefCostAccountId", debitor);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        /// Delete Debitor by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @DebitorId", new { DebitorId = id });
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

        private void AddCostCompaniesReference()
        {
            try
            {
                SqlConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr = $"IF(OBJECT_ID('FK_Debitors_CostAccounts', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_Debitors_CostAccounts FOREIGN KEY(RefCostAccountId) REFERENCES CostAccounts(CostAccountId)";

                using (SqlCommand command = new SqlCommand(commandStr, con))
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

        private void AddCostAccountsReference()
        {
            try
            {
                SqlConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr = $"IF(OBJECT_ID('FK_Debitors_Companies', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_Debitors_Companies FOREIGN KEY(RefCompanyId) REFERENCES Companies(CompanyId)";

                using (SqlCommand command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and Companies", e);
            }
        }

        /// <summary>
        /// Checks if Debitor has Cost Accounts
        /// </summary>
        /// <param name="id"></param>
        public bool IsDebitorInUse(int id)
        {
            bool IsInUse = true;
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    IsInUse = con.ExecuteScalar<bool>($"dbo.{TableName}_IsDebitorInUse @DebitorId", new { DebitorId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'IsDebitorInUse' from table '{TableName}'", e);
            }
            return IsInUse;
        }
    }
}

