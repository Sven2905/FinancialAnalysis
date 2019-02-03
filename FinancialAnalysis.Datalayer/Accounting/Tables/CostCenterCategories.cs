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
    public class CostCenterCategories : ITable
    {
        private readonly CostCenterCategoriesStoredProcedures sp = new CostCenterCategoriesStoredProcedures();

        public CostCenterCategories()
        {
            TableName = "CostCenterCategories";
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
                    "(CostCenterCategoryId int IDENTITY(1,1) PRIMARY KEY," +
                    "Name nvarchar(150) NOT NULL," +
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
        ///     Returns all CostCenterCategory records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CostCenterCategory> GetAll()
        {
            var output = new Dictionary<int, CostCenterCategory>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var query = con.Query<CostCenterCategory, CostCenter, CostCenterCategory>($"dbo.{TableName}_GetAll",
                        (objCostCenterCategory, objCostCenter) =>
                        {
                            if (!output.TryGetValue(objCostCenterCategory.CostCenterCategoryId,
                                out var CostCenterCategoryEntry))
                            {
                                CostCenterCategoryEntry = objCostCenterCategory;
                                output.Add(objCostCenterCategory.CostCenterCategoryId, objCostCenterCategory);
                            }

                            CostCenterCategoryEntry.CostCenters.Add(objCostCenter);

                            return objCostCenterCategory;
                        }, splitOn: "CostCenterCategoryId, CostCenterId",
                        commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output.Values;
        }

        /// <summary>
        ///     Inserts the CostCenterCategory item
        /// </summary>
        /// <param name="CostCenterCategory"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(CostCenterCategory CostCenterCategory)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Name, @Description ", CostCenterCategory);
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
        ///     Inserts the list of CostCenterCategory items
        /// </summary>
        /// <param name="creditor"></param>
        public void Insert(IEnumerable<CostCenterCategory> CostCenterCategories)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var CostCenterCategory in CostCenterCategories) Insert(CostCenterCategory);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns CostCenterCategory by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CostCenterCategory GetById(int id)
        {
            var output = new Dictionary<int, CostCenterCategory>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var query = con.Query<CostCenterCategory, CostCenter, CostCenterCategory>(
                        $"dbo.{TableName}_GetById @CostCenterCategoryId",
                        (objCostCenterCategory, objCostCenter) =>
                        {
                            if (!output.TryGetValue(objCostCenterCategory.CostCenterCategoryId,
                                out var CostCenterCategoryEntry))
                            {
                                CostCenterCategoryEntry = objCostCenterCategory;
                                output.Add(objCostCenterCategory.CostCenterCategoryId, objCostCenterCategory);
                            }

                            CostCenterCategoryEntry.CostCenters.Add(objCostCenter);

                            return objCostCenterCategory;
                        }, new {CostCenterCategoryId = id}, splitOn: "CostCenterCategoryId, CostCenterId",
                        commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output.Values.FirstOrDefault();
        }

        /// <summary>
        ///     Update CostCenterCategory, if not exist, insert it
        /// </summary>
        /// <param name="CostCenterCategory"></param>
        public void UpdateOrInsert(CostCenterCategory CostCenterCategory)
        {
            if (CostCenterCategory.CostCenterCategoryId == 0 ||
                GetById(CostCenterCategory.CostCenterCategoryId) is null)
            {
                Insert(CostCenterCategory);
                return;
            }

            Update(CostCenterCategory);
        }

        /// <summary>
        ///     Update CostCenterCategories, if not exist insert them
        /// </summary>
        /// <param name="User"></param>
        public void UpdateOrInsert(IEnumerable<CostCenterCategory> CostCenterCategories)
        {
            foreach (var CostCenterCategory in CostCenterCategories) UpdateOrInsert(CostCenterCategory);
        }

        /// <summary>
        ///     Update CostCenterCategory
        /// </summary>
        /// <param name="CostCenterCategory"></param>
        public void Update(CostCenterCategory CostCenterCategory)
        {
            if (CostCenterCategory.CostCenterCategoryId == 0 ||
                GetById(CostCenterCategory.CostCenterCategoryId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @CostCenterCategoryId, @Name,@Description",
                        CostCenterCategory);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete CostCenterCategory by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @CostCenterCategoryId", new {CostCenterCategoryId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }
    }
}