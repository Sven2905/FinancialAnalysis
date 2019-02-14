using Dapper;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.Accounting.CostCenterManagement;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FinancialAnalysis.Datalayer.Accounting
{
    public class CostCenterBudgets : ITable
    {
        private readonly CostCenterBudgetsStoredProcedures sp = new CostCenterBudgetsStoredProcedures();

        public CostCenterBudgets()
        {
            TableName = "CostCenterBudgets";
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
                    "CostCenterBudgetId int IDENTITY(1,1) PRIMARY KEY," +
                    "Year int, " +
                    "RefCostCenterId int, " +
                    "January money, " +
                    "February money, " +
                    "March money, " +
                    "April money, " +
                    "May money, " +
                    "June money, " +
                    "July money, " +
                    "August money, " +
                    "September money, " +
                    "October money, " +
                    "November money, " +
                    "December money)";

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
        ///     Returns all CostCenterBudget records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CostCenterBudget> GetAll()
        {
            IEnumerable<CostCenterBudget> output = new List<CostCenterBudget>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<CostCenterBudget>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Returns CostCenterBudget by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CostCenterBudget GetById(int id)
        {
            var output = new CostCenterBudget();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<CostCenterBudget>($"dbo.{TableName}_GetById @CostCenterBudgetId",
                        new { CostCenterBudgetId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Returns CostCenterCurrentCosts by CostCenter and Year
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<CostCenterCurrentCosts> GetAnnuallyCosts(int RefCostCenterId, int Year)
        {
            IEnumerable<CostCenterCurrentCosts> output = new List<CostCenterCurrentCosts>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<CostCenterCurrentCosts>($"dbo.{TableName}_GetAnnuallyCosts @RefCostCenterId, @Year ",
                        new { RefCostCenterId, Year });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAnnuallyCosts' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the CostCenterBudget item
        /// </summary>
        /// <param name="CostCenterBudget"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(CostCenterBudget CostCenterBudget)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Year, @RefCostCenterId, @January, @February, @March, @April, @May, @June, @July, @August, @September, @October, @November, @December",
                        CostCenterBudget);
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
        ///     Inserts the list of CostCenterBudgets items
        /// </summary>
        /// <param name="CostCenterBudgetor"></param>
        public void Insert(IEnumerable<CostCenterBudget> CostCenterBudgets)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var CostCenterBudget in CostCenterBudgets)
                    {
                        Insert(CostCenterBudget);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Update CostCenterBudget, if not exist, insert it
        /// </summary>
        /// <param name="CostCenterBudget"></param>
        public void UpdateOrInsert(CostCenterBudget CostCenterBudget)
        {
            if (CostCenterBudget.CostCenterBudgetId == 0 || GetById(CostCenterBudget.CostCenterBudgetId) is null)
            {
                Insert(CostCenterBudget);
                return;
            }

            Update(CostCenterBudget);
        }

        /// <summary>
        ///     Update CostCenterBudgets, if not exist insert them
        /// </summary>
        /// <param name="User"></param>
        public void UpdateOrInsert(IEnumerable<CostCenterBudget> CostCenterBudgets)
        {
            foreach (var CostCenterBudget in CostCenterBudgets)
            {
                UpdateOrInsert(CostCenterBudget);
            }
        }

        /// <summary>
        ///     Update CostCenterBudget
        /// </summary>
        /// <param name="CostCenterBudget"></param>
        public void Update(CostCenterBudget CostCenterBudget)
        {
            if (CostCenterBudget.CostCenterBudgetId == 0 || GetById(CostCenterBudget.CostCenterBudgetId) is null)
            {
                return;
            }

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute(
                        $"dbo.{TableName}_Update @CostCenterBudgetId, @Year, @RefCostCenterId, @January, @February, @March, @April, @May, @June, @July, @August, @September, @October, @November, @December",
                        CostCenterBudget);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete CostCenterBudget by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @CostCenterBudgetId", new { CostCenterBudgetId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        public void AddReferences()
        {
            AddCostCentersReference();
        }

        private void AddCostCentersReference()
        {
            var refTable = "CostCenters";

            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_{refTable}', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_{refTable} FOREIGN KEY(RefCostCenterId) REFERENCES {refTable}(CostCenterId) ON DELETE CASCADE";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and {refTable}",
                    e);
            }
        }
    }
}