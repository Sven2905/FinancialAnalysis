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
    public class PurchaseTypes : ITable
    {
        private readonly PurchaseTypesStoredProcedures sp = new PurchaseTypesStoredProcedures();

        public PurchaseTypes()
        {
            TableName = "PurchaseTypes";
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
                                 "(PurchaseTypeId int IDENTITY(1,1) PRIMARY KEY, " +
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
        ///     Returns all PurchaseType records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PurchaseType> GetAll()
        {
            IEnumerable<PurchaseType> output = new List<PurchaseType>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<PurchaseType>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the PurchaseType item
        /// </summary>
        /// <param name="PurchaseType"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(PurchaseType PurchaseType)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Name, @Description ",
                        PurchaseType);
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
        ///     Inserts the list of PurchaseType items
        /// </summary>
        /// <param name="PurchaseTypes"></param>
        public void Insert(IEnumerable<PurchaseType> PurchaseTypes)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var PurchaseType in PurchaseTypes) Insert(PurchaseType);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns PurchaseType by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PurchaseType GetById(int id)
        {
            var output = new PurchaseType();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<PurchaseType>(
                        $"dbo.{TableName}_GetById @PurchaseTypeId", new {PurchaseTypeId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update PurchaseType, if not exist, insert it
        /// </summary>
        /// <param name="PurchaseType"></param>
        public void UpdateOrInsert(PurchaseType PurchaseType)
        {
            if (PurchaseType.PurchaseTypeId == 0 ||
                GetById(PurchaseType.PurchaseTypeId) is null)
            {
                Insert(PurchaseType);
                return;
            }

            Update(PurchaseType);
        }

        /// <summary>
        ///     Update PurchaseTypes, if not exist insert them
        /// </summary>
        /// <param name="PurchaseTypes"></param>
        public void UpdateOrInsert(IEnumerable<PurchaseType> PurchaseTypes)
        {
            foreach (var PurchaseType in PurchaseTypes) UpdateOrInsert(PurchaseType);
        }

        /// <summary>
        ///     Update PurchaseType
        /// </summary>
        /// <param name="PurchaseType"></param>
        public void Update(PurchaseType PurchaseType)
        {
            if (PurchaseType.PurchaseTypeId == 0 ||
                GetById(PurchaseType.PurchaseTypeId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @PurchaseTypeId, @Name, @Description ",
                        PurchaseType);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete PurchaseType by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @PurchaseTypeId", new {PurchaseTypeId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete PurchaseType by Item
        /// </summary>
        /// <param name="id"></param>
        public void Delete(PurchaseType PurchaseType)
        {
            Delete(PurchaseType.PurchaseTypeId);
        }
    }
}