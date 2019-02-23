using Dapper;
using FinancialAnalysis.Models.Accounting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FinancialAnalysis.Datalayer.Accounting
{
    public class CostCenters : ITable
    {
        private readonly CostCentersStoredProcedures sp = new CostCentersStoredProcedures();

        public CostCenters()
        {
            TableName = "CostCenters";
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
                    $"If not exists (select name from sysobjects where name = '{TableName}') CREATE TABLE {TableName}" +
                    "(CostCenterId int IDENTITY(1,1) PRIMARY KEY," +
                    "Name nvarchar(150) NOT NULL," +
                    "Identifier nvarchar(150) NOT NULL, " +
                    "RefCostCenterCategoryId int, " +
                    "CostCenterType int, " +
                    "Description nvarchar(MAX))";

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
        ///     Returns all CostCenter records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CostCenter> GetAll(int year = 0)
        {
            if (Equals(year, 0))
            {
                year = DateTime.Now.Year;
            }
            var CostCenterDictionary = new Dictionary<int, CostCenter>();
            IEnumerable<CostCenter> output = new List<CostCenter>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<CostCenter, CostCenterCategory, CostCenterBudget, CostCenter>($"dbo.{TableName}_GetAll",
                        (objCostCenter, objCostCenterCategory, objCostCenterBudget) =>
                        {
                            if (!CostCenterDictionary.TryGetValue(objCostCenter.RefCostCenterBudgetId, out CostCenter CostCenterEntry))
                            {
                                CostCenterEntry = objCostCenter;
                                CostCenterEntry.CostCenterCategory = objCostCenterCategory;
                                CostCenterDictionary.Add(objCostCenter.CostCenterId, objCostCenter);
                                CostCenterEntry.ScheduledBudget = objCostCenterBudget;
                            }
                            return objCostCenter;
                        }, splitOn: "CostCenterCategoryId, CostCenterBudgetId",
                        commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return CostCenterDictionary.Values;
        }

        /// <summary>
        ///     Returns CostCenter by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CostCenter GetById(int id)
        {
            var CostCenterDictionary = new Dictionary<int, CostCenter>();
            IEnumerable<CostCenter> output = new List<CostCenter>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<CostCenter, CostCenterCategory, CostCenterBudget, CostCenter>(
                        $"dbo.{TableName}_GetById @CostCenterId",
                        (objCostCenter, objCostCenterCategory, objCostCenterBudget) =>
                        {
                            if (!CostCenterDictionary.TryGetValue(objCostCenter.RefCostCenterBudgetId, out CostCenter CostCenterEntry))
                            {
                                CostCenterEntry = objCostCenter;
                                CostCenterEntry.CostCenterCategory = objCostCenterCategory;
                                CostCenterEntry.ScheduledBudget = objCostCenterBudget;
                            }
                            return objCostCenter;
                        }, new { CostCenterId = id }, splitOn: "CostCenterCategoryId, CostCenterBudgetId",
                        commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return CostCenterDictionary.Values.FirstOrDefault();
        }

        /// <summary>
        ///     Inserts the CostCenter item
        /// </summary>
        /// <param name="CostCenter"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(CostCenter CostCenter)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result =
                        con.Query<int>(
                            $"dbo.{TableName}_Insert @Name, @Identifier, @RefCostCenterCategoryId, @CostCenterType, @Description ",
                            CostCenter);
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
        ///     Inserts the list of CostCenter items
        /// </summary>
        /// <param name="creditor"></param>
        public void Insert(IEnumerable<CostCenter> CostCenters)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var CostCenter in CostCenters)
                    {
                        Insert(CostCenter);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Update CostCenter, if not exist, insert it
        /// </summary>
        /// <param name="CostCenter"></param>
        public void UpdateOrInsert(CostCenter CostCenter)
        {
            if (CostCenter.CostCenterId == 0 || GetById(CostCenter.CostCenterId) is null)
            {
                Insert(CostCenter);
                return;
            }

            Update(CostCenter);
        }

        /// <summary>
        ///     Update CostCenters, if not exist insert them
        /// </summary>
        /// <param name="User"></param>
        public void UpdateOrInsert(IEnumerable<CostCenter> CostCenters)
        {
            foreach (var CostCenter in CostCenters)
            {
                UpdateOrInsert(CostCenter);
            }
        }

        /// <summary>
        ///     Update CostCenter
        /// </summary>
        /// <param name="CostCenter"></param>
        public void Update(CostCenter CostCenter)
        {
            if (CostCenter.CostCenterId == 0)
            {
                return;
            }

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute(
                        $"dbo.{TableName}_Update @CostCenterId, @Name, @Identifier, @RefCostCenterCategoryId, @CostCenterType, @Description",
                        CostCenter);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete CostCenter by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @CostCenterId", new { CostCenterId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        public void AddReferences()
        {
            AddCostCenterCategoriesReference();
        }

        private void AddCostCenterCategoriesReference()
        {
            var refTable = "CostCenterCategories";

            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_{refTable}', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_{refTable} FOREIGN KEY(RefCostCenterCategoryId) REFERENCES {refTable}(CostCenterCategoryId) ON DELETE CASCADE";

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