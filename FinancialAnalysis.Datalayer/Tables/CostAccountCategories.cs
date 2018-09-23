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
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Datalayer.Tables
{
    public class CostAccountCategories : ITable
    {
        public string TableName { get; }
        private CostAccountCategoriesStoredProcedures sp = new CostAccountCategoriesStoredProcedures();

        public CostAccountCategories()
        {
            TableName = "CostAccountCategories";
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
                var commandStr = $"If not exists (select name from sysobjects where name = '{TableName}') " +
                    $"CREATE TABLE {TableName}(CostAccountCategoryId int IDENTITY(1,1) PRIMARY KEY,Description nvarchar(50) NOT NULL, ParentCategoryId int )";

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
        /// Returns all CostAccountCategory records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CostAccountCategory> GetAll()
        {
            IEnumerable<CostAccountCategory> output = new List<CostAccountCategory>();
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<CostAccountCategory>($"dbo.{TableName}_GetAll");
                }

            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }
            return output;
        }

        /// <summary>
        /// Inserts the CostAccountCategory item
        /// </summary>
        /// <param name="costAccountCategory"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(CostAccountCategory costAccountCategory)
        {
            int id = 0;
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Description, @ParentCategoryId", costAccountCategory);
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
        /// Inserts the list of CostAccountCategory items
        /// </summary>
        /// <param name="costAccountCategories"></param>
        public void Insert(IEnumerable<CostAccountCategory> costAccountCategories)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var costAccountCategory in costAccountCategories)
                    {
                        Insert(costAccountCategory);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }
        }

        /// <summary>
        /// Returns CostAccountCategory by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CostAccountCategory GetById(int id)
        {
            CostAccountCategory output = new CostAccountCategory();
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<CostAccountCategory>($"dbo.{TableName}_GetById @Id", new { CostAccountCategoryId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }
            return output;
        }

        /// <summary>
        /// Update CostAccountCategory, if not exist, insert it
        /// </summary>
        /// <param name="costAccountCategory"></param>
        public void UpdateOrInsert(CostAccountCategory costAccountCategory)
        {
            if (costAccountCategory.CostAccountCategoryId == 0 || GetById(costAccountCategory.CostAccountCategoryId) is null)
            {
                Insert(costAccountCategory);
                return;
            }

            Update(costAccountCategory);
        }

        /// <summary>
        /// Update CostAccountCategories, if not exist insert them
        /// </summary>
        /// <param name="costAccountCategories"></param>
        public void UpdateOrInsert(IEnumerable<CostAccountCategory> costAccountCategories)
        {
            foreach (var taxType in costAccountCategories)
            {
                UpdateOrInsert(taxType);
            }
        }

        /// <summary>
        /// Update CostAccountCategory
        /// </summary>
        /// <param name="costAccountCategory"></param>
        public void Update(CostAccountCategory costAccountCategory)
        {
            if (costAccountCategory.CostAccountCategoryId == 0 || GetById(costAccountCategory.CostAccountCategoryId) is null)
            {
                return;
            }

            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @CostAccountCategoryId, @Description @ParentCategoryId", costAccountCategory);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        /// Delete CostAccountCategory by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @CostAccountCategoryId", new { CostAccountCategoryId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        /// <summary>
        /// Delete CostAccountCategory by Item
        /// </summary>
        /// <param name="id"></param>
        public void Delete(CostAccountCategory costAccountCategory)
        {
            Delete(costAccountCategory.CostAccountCategoryId);
        }

        /// <summary>
        /// Returns the id of creditor main group "Kreditoren"
        /// </summary>
        /// <returns></returns>
        public int GetCreditorId()
        {
            int output = 0;
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.ExecuteScalar<int>($"dbo.{TableName}_GetCreditorId @Description", new { Description = "Kreditoren" });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetCreditorId' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        /// Returns the id of debitor main group "Debitoren"
        /// </summary>
        /// <returns></returns>
        public int GetDebitorId()
        {
            int output = 0;
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.ExecuteScalar<int>($"dbo.{TableName}_GetDebitorId @Description", new { Description = "Debitoren" });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetNextCreditorNumber' from table '{TableName}'", e);
            }

            return output;
        }
    }
}
