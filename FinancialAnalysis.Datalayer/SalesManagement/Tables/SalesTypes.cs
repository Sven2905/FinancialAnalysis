using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.SalesManagement;
using Serilog;

namespace FinancialAnalysis.Datalayer.SalesManagement
{
    public class SalesTypes : ITable
    {
        private readonly SalesTypesStoredProcedures sp = new SalesTypesStoredProcedures();

        public SalesTypes()
        {
            TableName = "SalesTypes";
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
                                 "(SalesTypeId int IDENTITY(1,1) PRIMARY KEY, " +
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
        ///     Returns all SalesType records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SalesType> GetAll()
        {
            IEnumerable<SalesType> output = new List<SalesType>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<SalesType>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the SalesType item
        /// </summary>
        /// <param name="SalesType"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(SalesType SalesType)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Name, @Description ",
                        SalesType);
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
        ///     Inserts the list of SalesType items
        /// </summary>
        /// <param name="SalesTypes"></param>
        public void Insert(IEnumerable<SalesType> SalesTypes)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var SalesType in SalesTypes) Insert(SalesType);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns SalesType by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SalesType GetById(int id)
        {
            var output = new SalesType();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<SalesType>(
                        $"dbo.{TableName}_GetById @SalesTypeId", new {SalesTypeId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update SalesType, if not exist, insert it
        /// </summary>
        /// <param name="SalesType"></param>
        public void UpdateOrInsert(SalesType SalesType)
        {
            if (SalesType.SalesTypeId == 0 ||
                GetById(SalesType.SalesTypeId) is null)
            {
                Insert(SalesType);
                return;
            }

            Update(SalesType);
        }

        /// <summary>
        ///     Update SalesTypes, if not exist insert them
        /// </summary>
        /// <param name="SalesTypes"></param>
        public void UpdateOrInsert(IEnumerable<SalesType> SalesTypes)
        {
            foreach (var SalesType in SalesTypes) UpdateOrInsert(SalesType);
        }

        /// <summary>
        ///     Update SalesType
        /// </summary>
        /// <param name="SalesType"></param>
        public void Update(SalesType SalesType)
        {
            if (SalesType.SalesTypeId == 0 ||
                GetById(SalesType.SalesTypeId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @SalesTypeId, @Name, @Description ",
                        SalesType);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete SalesType by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @SalesTypeId", new {SalesTypeId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete SalesType by Item
        /// </summary>
        /// <param name="id"></param>
        public void Delete(SalesType SalesType)
        {
            Delete(SalesType.SalesTypeId);
        }
    }
}