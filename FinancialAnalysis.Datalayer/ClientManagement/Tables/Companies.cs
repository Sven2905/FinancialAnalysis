using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Datalayer.StoredProcedures;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.ClientManagement;
using Serilog;

namespace FinancialAnalysis.Datalayer.ClientManagement
{
    public class Companies : ITable
    {
        private readonly CompaniesStoredProcedures sp = new CompaniesStoredProcedures();

        public Companies()
        {
            TableName = "Companies";
            CheckAndCreateTable();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs\\Tables.txt", rollingInterval: RollingInterval.Month)
                .CreateLogger();
        }

        public string TableName { get; }

        public void CheckAndCreateStoredProcedures()
        {
            sp.CheckAndCreateProcedures();
        }

        public void CheckAndCreateTable()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"If not exists (select name from sysobjects where name = '{TableName}') CREATE TABLE {TableName}(" +
                    "CompanyId int IDENTITY(1,1) PRIMARY KEY," +
                    "ContactPerson nvarchar(50)," +
                    "UStID nvarchar(50)," +
                    "TaxNumber nvarchar(50)," +
                    "Website nvarchar(50)," +
                    "Logo varbinary(MAX), " +
                    "RefClientId int, " +
                    "CEO nvarchar(50))";

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

        /// <summary>
        ///     Inserts the Company item
        /// </summary>
        /// <param name="Company"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(Company company)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result =
                        con.Query<int>(
                            $"dbo.{TableName}_Insert @ContactPerson, @UStID, @TaxNumber, @Website, @CEO, @Logo, @RefClientId",
                            company);
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
        ///     Inserts the list of Company items
        /// </summary>
        /// <param name="company"></param>
        public void Insert(IEnumerable<Company> companies)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var company in companies)
                        con.Query(
                            $"dbo.{TableName}_Insert @ContactPerson, @UStID, @TaxNumber, @Website,@CEO, @Logo, @RefClientId",
                            company);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Update Company, if not exist, insert it
        /// </summary>
        /// <param name="company"></param>
        public void UpdateOrInsert(Company company)
        {
            if (company.CompanyId == 0)
            {
                Insert(company);
                return;
            }

            Update(company);
        }

        /// <summary>
        ///     Update Companies, if not exist insert them
        /// </summary>
        /// <param name="companies"></param>
        public void UpdateOrInsert(IEnumerable<Company> companies)
        {
            foreach (var company in companies) UpdateOrInsert(company);
        }

        /// <summary>
        ///     Update Company
        /// </summary>
        /// <param name="company"></param>
        public void Update(Company company)
        {
            if (company.CompanyId == 0) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute(
                        $"dbo.{TableName}_Update @CompanyId, @ContactPerson, @UStID, @TaxNumber, @Website, @CEO, @Logo, @RefClientId",
                        company);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete Company by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @CompanyId", new {CompanyId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Checks if Company has Creditor or Debitor
        /// </summary>
        /// <param name="id"></param>
        public bool IsCompanyInUse(int id)
        {
            var IsInUse = true;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    IsInUse = con.ExecuteScalar<bool>($"dbo.{TableName}_IsCompanyInUse @CompanyId",
                        new {CompanyId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'IsCompanyInUse' from table '{TableName}'", e);
            }

            return IsInUse;
        }

        public void AddReferences()
        {
            AddClientsReference();
        }

        private void AddClientsReference()
        {
            string refTable = "Clients";

            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_{refTable}', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_{refTable} FOREIGN KEY(RefClientId) REFERENCES {refTable}(ClientId) ON DELETE CASCADE";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and {TableName}",
                    e);
            }
        }
    }
}