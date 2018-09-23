using Dapper;
using FinancialAnalysis.Datalayer.StoredProcedures;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FinancialAnalysis.Datalayer.Tables
{
    public class Companies : ITable
    {
        public string TableName { get; }
        private CompaniesStoredProcedures sp = new CompaniesStoredProcedures();

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

        public void CheckAndCreateStoredProcedures()
        {
            sp.CheckAndCreateProcedures();
        }

        public void CheckAndCreateTable()
        {
            try
            {
                SqlConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr = $"If not exists (select name from sysobjects where name = '{TableName}') CREATE TABLE {TableName}(" +
                                 $"CompanyId int IDENTITY(1,1) PRIMARY KEY," +
                                 $"Name nvarchar(50) NOT NULL," +
                                 $"Street nvarchar(50) NOT NULL," +
                                 $"Postcode int NOT NULL," +
                                 $"City nvarchar(50) NOT NULL," +
                                 $"ContactPerson nvarchar(50)," +
                                 $"UStID nvarchar(50)," +
                                 $"TaxNumber nvarchar(50)," +
                                 $"Phone nvarchar(50)," +
                                 $"Fax nvarchar(50)," +
                                 $"eMail nvarchar(50)," +
                                 $"Website nvarchar(50)," +
                                 $"IBAN nvarchar(50)," +
                                 $"BIC nvarchar(50)," +
                                 $"BankName nvarchar(50)," +
                                 $"FederalState int)";

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

        /// <summary>
        /// Returns all Company records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Company> GetAll()
        {
            IEnumerable<Company> output = new List<Company>();
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<Company>($"dbo.{TableName}_GetAll");
                }

            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }
            return output;
        }

        /// <summary>
        /// Inserts the Company item
        /// </summary>
        /// <param name="Company"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(Company company)
        {
            int id = 0;
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Name, @Street, @Postcode, @City, @ContactPerson, @UStID, @TaxNumber, @Phone, @Fax, @eMail, @Website, @IBAN, @BIC, @BankName, @FederalState", company);
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
        /// Inserts the list of Company items
        /// </summary>
        /// <param name="company"></param>
        public void Insert(IEnumerable<Company> companies)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var company in companies)
                    {
                        con.Query($"dbo.{TableName}_Insert @Name, @Street, @Postcode, @City, @ContactPerson, @UStID, @TaxNumber, @Phone, @Fax, @eMail, @Website, @IBAN, @BIC, @BankName, @FederalState", company);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        /// Returns Company by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Company GetById(int id)
        {
            Company output = new Company();
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<Company>($"dbo.{TableName}_GetById @CompanyId", new { CompanyId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }
            return output;
        }

        /// <summary>
        /// Update Company, if not exist, insert it
        /// </summary>
        /// <param name="company"></param>
        public void UpdateOrInsert(Company company)
        {
            if (company.CompanyId == 0 || GetById(company.CompanyId) is null)
            {
                Insert(company);
                return;
            }

            Update(company);
        }

        /// <summary>
        /// Update Companies, if not exist insert them
        /// </summary>
        /// <param name="companies"></param>
        public void UpdateOrInsert(IEnumerable<Company> companies)
        {
            foreach (var company in companies)
            {
                UpdateOrInsert(company);
            }
        }

        /// <summary>
        /// Update Company
        /// </summary>
        /// <param name="company"></param>
        public void Update(Company company)
        {
            if (company.CompanyId == 0 || GetById(company.CompanyId) is null)
            {
                return;
            }

            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @CompanyId, @Name, @Street, @Postcode, @City, @ContactPerson, @UStID, @TaxNumber, @Phone, @Fax, @eMail, @Website, @IBAN, @BIC, @BankName, @FederalState", company);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        /// Delete Company by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @CompanyId", new { CompanyId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }
    }
}
