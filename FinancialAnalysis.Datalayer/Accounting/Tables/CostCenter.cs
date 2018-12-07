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
        public string TableName { get; }
        private CostCentersStoredProcedures sp = new CostCentersStoredProcedures();

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

        public void CheckAndCreateTable()
        {
            try
            {
                SqlConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr = $"If not exists (select name from sysobjects where name = '{TableName}') CREATE TABLE {TableName}" +
                                $"(CostCenterId int IDENTITY(1,1) PRIMARY KEY," +
                                 $"Description nvarchar(150) NOT NULL)";

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

        public void CheckAndCreateStoredProcedures()
        {
            sp.CheckAndCreateProcedures();
        }

        /// <summary>
        /// Returns all CostCenter records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CostCenter> GetAll()
        {
            IEnumerable<CostCenter> output = new List<CostCenter>();
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<CostCenter>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }
            return output;
        }

        /// <summary>
        /// Inserts the CostCenter item
        /// </summary>
        /// <param name="CostCenter"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(CostCenter CostCenter)
        {
            int id = 0;
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Description", CostCenter);
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
        /// Inserts the list of CostCenter items
        /// </summary>
        /// <param name="creditor"></param>
        public void Insert(IEnumerable<CostCenter> CostCenters)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
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
        /// Returns CostCenter by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CostCenter GetById(int id)
        {
            CostCenter output = new CostCenter();
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<CostCenter>($"dbo.{TableName}_GetById @CreditId", new { CostCenterId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }
            return output;
        }
    }
}