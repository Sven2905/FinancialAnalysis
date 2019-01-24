using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.PurchaseManagement;
using Serilog;

namespace FinancialAnalysis.Datalayer.PurchaseManagement
{
    public class BillTypes : ITable
    {
        private readonly BillTypesStoredProcedures sp = new BillTypesStoredProcedures();

        public BillTypes()
        {
            TableName = "BillTypes";
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
                                 $"CREATE TABLE {TableName}" +
                                 "(BillTypeId int IDENTITY(1,1) PRIMARY KEY, " +
                                 "Name nvarchar(150) NOT NULL, " +
                                 "Description nvarchar(150))";

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
        ///     Returns all BillType records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BillType> GetAll()
        {
            IEnumerable<BillType> output = new List<BillType>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<BillType>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the BillType item
        /// </summary>
        /// <param name="BillType"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(BillType BillType)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Name, @Description ",
                        BillType);
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
        ///     Inserts the list of BillType items
        /// </summary>
        /// <param name="BillTypes"></param>
        public void Insert(IEnumerable<BillType> BillTypes)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var BillType in BillTypes) Insert(BillType);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns BillType by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BillType GetById(int id)
        {
            var output = new BillType();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<BillType>(
                        $"dbo.{TableName}_GetById @BillTypeId", new {BillTypeId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update BillType, if not exist, insert it
        /// </summary>
        /// <param name="BillType"></param>
        public void UpdateOrInsert(BillType BillType)
        {
            if (BillType.BillTypeId == 0 ||
                GetById(BillType.BillTypeId) is null)
            {
                Insert(BillType);
                return;
            }

            Update(BillType);
        }

        /// <summary>
        ///     Update BillTypes, if not exist insert them
        /// </summary>
        /// <param name="BillTypes"></param>
        public void UpdateOrInsert(IEnumerable<BillType> BillTypes)
        {
            foreach (var BillType in BillTypes) UpdateOrInsert(BillType);
        }

        /// <summary>
        ///     Update BillType
        /// </summary>
        /// <param name="BillType"></param>
        public void Update(BillType BillType)
        {
            if (BillType.BillTypeId == 0 ||
                GetById(BillType.BillTypeId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @BillTypeId, @Name, @Description ",
                        BillType);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete BillType by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @BillTypeId", new {BillTypeId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete BillType by Item
        /// </summary>
        /// <param name="id"></param>
        public void Delete(BillType BillType)
        {
            Delete(BillType.BillTypeId);
        }
    }
}