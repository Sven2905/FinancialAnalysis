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
    public class FixedCostAllocations : ITable
    {
        private readonly FixedCostAllocationsStoredProcedures sp = new FixedCostAllocationsStoredProcedures();

        public FixedCostAllocations()
        {
            TableName = "FixedCostAllocations";
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

        /// <summary>
        ///     Checks if table exists otherwise creates it
        /// </summary>
        public void CheckAndCreateTable()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr = $"If not exists (select name from sysobjects where name = '{TableName}') " +
                                 $"CREATE TABLE {TableName}(" +
                                 "FixedCostAllocationId int IDENTITY(1,1) PRIMARY KEY, " +
                                 "Shares decimal NOT NULL, " +
                                 "RefCostCenterId int )";

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

        public string TableName { get; }

        /// <summary>
        ///     Returns all FixedCostAllocation records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FixedCostAllocation> GetAll()
        {
            IEnumerable<FixedCostAllocation> output = new List<FixedCostAllocation>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<FixedCostAllocation, CostCenter, CostCenterCategory, FixedCostAllocation>($"dbo.{TableName}_GetAll",
                        (objFixedCostAllocation, objCostCenter, objCostCenterCategory) =>
                        {
                            objFixedCostAllocation.CostCenter = objCostCenter;
                            if (objCostCenter != null)
                            {
                                objFixedCostAllocation.CostCenter.CostCenterCategory = objCostCenterCategory;
                            }
                            return objFixedCostAllocation;
                        }, splitOn: "FixedCostAllocationId, CostCenterId, CostCenterCategoryId",
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
        ///     Inserts the FixedCostAllocation item
        /// </summary>
        /// <param name="FixedCostAllocation"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(FixedCostAllocation FixedCostAllocation)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result =
                        con.Query<int>(
                            $"dbo.{TableName}_Insert @Shares, @RefCostCenterId",
                            FixedCostAllocation);
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
        ///     Inserts the list of FixedCostAllocation items
        /// </summary>
        /// <param name="FixedCostAllocations"></param>
        public void Insert(IEnumerable<FixedCostAllocation> FixedCostAllocations)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var FixedCostAllocation in FixedCostAllocations) Insert(FixedCostAllocation);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns FixedCostAllocation by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FixedCostAllocation GetById(int id)
        {
            var output = new FixedCostAllocation();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<FixedCostAllocation>($"dbo.{TableName}_GetById @FixedCostAllocationId",
                        new {FixedCostAllocationId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update FixedCostAllocation, if not exist, insert it
        /// </summary>
        /// <param name="FixedCostAllocation"></param>
        public void UpdateOrInsert(FixedCostAllocation FixedCostAllocation)
        {
            if (FixedCostAllocation.FixedCostAllocationId == 0 || GetById(FixedCostAllocation.FixedCostAllocationId) is null)
            {
                Insert(FixedCostAllocation);
                return;
            }

            Update(FixedCostAllocation);
        }

        /// <summary>
        ///     Update FixedCostAllocations, if not exist insert them
        /// </summary>
        /// <param name="FixedCostAllocations"></param>
        public void UpdateOrInsert(IEnumerable<FixedCostAllocation> FixedCostAllocations)
        {
            foreach (var FixedCostAllocation in FixedCostAllocations) UpdateOrInsert(FixedCostAllocation);
        }

        /// <summary>
        ///     Update FixedCostAllocation
        /// </summary>
        /// <param name="FixedCostAllocation"></param>
        public void Update(FixedCostAllocation FixedCostAllocation)
        {
            if (FixedCostAllocation.FixedCostAllocationId == 0 || GetById(FixedCostAllocation.FixedCostAllocationId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute(
                        $"dbo.{TableName}_Update @FixedCostAllocationId, @Shares, @RefCostCenterId",
                        FixedCostAllocation);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete FixedCostAllocation by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @FixedCostAllocationId", new {FixedCostAllocationId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete FixedCostAllocation by Item
        /// </summary>
        /// <param name="id"></param>
        public void Delete(FixedCostAllocation FixedCostAllocation)
        {
            Delete(FixedCostAllocation.FixedCostAllocationId);
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
                Log.Error($"Exception occured while creating reference between '{TableName}' and {TableName}",
                    e);
            }
        }
    }
}