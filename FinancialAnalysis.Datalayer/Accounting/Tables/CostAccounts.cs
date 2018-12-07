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
    public class CostAccounts : ITable
    {
        private readonly CostAccountsStoredProcedures sp = new CostAccountsStoredProcedures();

        public CostAccounts()
        {
            TableName = "CostAccounts";
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
                var commandStr = $"If not exists (select name from sysobjects where name = '{TableName}') " +
                                 $"CREATE TABLE {TableName}(CostAccountId int IDENTITY(1,1) PRIMARY KEY, " +
                                 "Description nvarchar(150) NOT NULL, AccountNumber int UNIQUE, " +
                                 "RefTaxTypeId int NULL, " +
                                 "RefCostAccountCategoryId int, " +
                                 "IsVisible bit, " +
                                 "IsEditable bit )";

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
        ///     Returns all CostAccount records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CostAccount> GetAll()
        {
            IEnumerable<CostAccount> output = new List<CostAccount>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<CostAccount, CostAccountCategory, CostAccount>($"dbo.{TableName}_GetAll",
                        (objCostAccount, objCostAccountCategory) =>
                        {
                            objCostAccount.CostAccountCategory = objCostAccountCategory;
                            return objCostAccount;
                        }, splitOn: "CostAccountCategoryId",
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
        ///     Returns all visible CostAccount records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CostAccount> GetAllVisible()
        {
            IEnumerable<CostAccount> output = new List<CostAccount>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<CostAccount, CostAccountCategory, CostAccount>($"dbo.{TableName}_GetAllVisible",
                        (objCostAccount, objCostAccountCategory) =>
                        {
                            objCostAccount.CostAccountCategory = objCostAccountCategory;
                            return objCostAccount;
                        }, splitOn: "CostAccountCategoryId",
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
        ///     Inserts the CostAccount item
        /// </summary>
        /// <param name="costAccount"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(CostAccount costAccount)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result =
                        con.Query<int>(
                            $"dbo.{TableName}_Insert @Description, @AccountNumber, @RefTaxTypeId, @RefCostAccountCategoryId, @IsVisible, @IsEditable ",
                            costAccount);
                    return result.Single();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' from table '{TableName}'", e);
            }

            return id;
        }

        /// <summary>
        ///     Inserts the list of CostAccount items
        /// </summary>
        /// <param name="costAccounts"></param>
        public void Insert(IEnumerable<CostAccount> costAccounts)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var costAccount in costAccounts) Insert(costAccount);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns CostAccount by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CostAccount GetById(int id)
        {
            var output = new CostAccount();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<CostAccount>($"dbo.{TableName}_GetById @CostAccountId",
                        new {CostAccountId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update CostAccount, if not exist, insert it
        /// </summary>
        /// <param name="costAccount"></param>
        public void UpdateOrInsert(CostAccount costAccount)
        {
            if (costAccount.CostAccountId == 0 || GetById(costAccount.CostAccountId) is null)
            {
                Insert(costAccount);
                return;
            }

            Update(costAccount);
        }

        /// <summary>
        ///     Update CostAccounts, if not exist insert them
        /// </summary>
        /// <param name="costAccounts"></param>
        public void UpdateOrInsert(IEnumerable<CostAccount> costAccounts)
        {
            foreach (var costAccount in costAccounts) UpdateOrInsert(costAccount);
        }

        /// <summary>
        ///     Update CostAccount
        /// </summary>
        /// <param name="costAccount"></param>
        public void Update(CostAccount costAccount)
        {
            if (costAccount.CostAccountId == 0 || GetById(costAccount.CostAccountId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute(
                        $"dbo.{TableName}_Update @CostAccountId, @Description, @AccountNumber, @RefTaxTypeId, @RefCostAccountCategoryId, @IsVisible, @IsEditable",
                        costAccount);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete CostAccount by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @CostAccountId", new {CostAccountId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete CostAccount by Item
        /// </summary>
        /// <param name="id"></param>
        public void Delete(CostAccount costAccount)
        {
            Delete(costAccount.CostAccountId);
        }

        public int GetNextDebitorNumber()
        {
            var output = 10000;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.ExecuteScalar<int>($"dbo.{TableName}_GetNextDebitorNumber");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetNextDebitorNumber' from table '{TableName}'", e);
            }

            if (output == 0)
                output = 10000;
            else
                output++;

            return output;
        }

        public int GetNextCreditorNumber()
        {
            var output = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.ExecuteScalar<int>($"dbo.{TableName}_GetNextCreditorNumber");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetNextCreditorNumber' from table '{TableName}'", e);
            }

            if (output == 0)
                output = 70000;
            else
                output++;

            return output;
        }

        public void AddReferences()
        {
            AddCostAccountCategoriesReference();
            AddTaxTypesReference();
        }

        private void AddCostAccountCategoriesReference()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_CostAccounts_CostAccountCategories', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_CostAccounts_CostAccountCategories FOREIGN KEY(RefCostAccountCategoryId) REFERENCES CostAccountCategories(CostAccountCategoryId) ON DELETE CASCADE";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and CostAccountCategories",
                    e);
            }
        }

        private void AddTaxTypesReference()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_CostAccounts_TaxTypes', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_CostAccounts_TaxTypes FOREIGN KEY(RefTaxTypeId) REFERENCES TaxTypes(TaxTypeId)";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and TaxTypes", e);
            }
        }
    }
}