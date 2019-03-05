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
    public class PaymentConditions : ITable
    {
        private readonly PaymentConditionsStoredProcedures sp = new PaymentConditionsStoredProcedures();

        public PaymentConditions()
        {
            TableName = "PaymentConditions";
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
                    "PaymentConditionId int IDENTITY(1,1) PRIMARY KEY," +
                    "Name nvarchar(150) NOT NULL," +
                    "Percentage money NOT NULL," +
                    "TimeValue int NOT NULL," +
                    "PayType int NOT NULL)";

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
        ///     Returns all PaymentCondition records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PaymentCondition> GetAll()
        {
            IEnumerable<PaymentCondition> output = new List<PaymentCondition>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<PaymentCondition>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the PaymentCondition item
        /// </summary>
        /// <param name="PaymentCondition"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(PaymentCondition PaymentCondition)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Name, @Percentage, @TimeValue, @PayType",
                        PaymentCondition);
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
        ///     Inserts the list of PaymentCondition items
        /// </summary>
        /// <param name="creditor"></param>
        public void Insert(IEnumerable<PaymentCondition> PaymentConditions)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var PaymentCondition in PaymentConditions)
                    {
                        Insert(PaymentCondition);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns PaymentCondition by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PaymentCondition GetById(int id)
        {
            var output = new PaymentCondition();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<PaymentCondition>($"dbo.{TableName}_GetById @PaymentConditionId",
                        new { PaymentConditionId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        /// Update PaymentCondition, if not exist, insert it
        /// </summary>
        /// <param name="PaymentCondition"></param>
        public void UpdateOrInsert(PaymentCondition PaymentCondition)
        {
            if (PaymentCondition.PaymentConditionId == 0)
            {
                Insert(PaymentCondition);
                return;
            }

            Update(PaymentCondition);
        }

        /// <summary>
        /// Update PaymentConditions, if not exist insert them
        /// </summary>
        /// <param name="PaymentCondition"></param>
        public void UpdateOrInsert(IEnumerable<PaymentCondition> PaymentConditions)
        {
            foreach (var PaymentCondition in PaymentConditions)
            {
                UpdateOrInsert(PaymentCondition);
            }
        }

        /// <summary>
        ///     Update PaymentCondition
        /// </summary>
        /// <param name="PaymentCondition"></param>
        public void Update(PaymentCondition PaymentCondition)
        {
            if (PaymentCondition.PaymentConditionId == 0)
            {
                return;
            }

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute(
                        $"dbo.{TableName}_Update @PaymentConditionId, @Name, @Percentage, @TimeValue, @PayType",
                        PaymentCondition);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete PaymentCondition by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int PaymentConditionId)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @PaymentConditionId", new { PaymentConditionId });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete PaymentCondition by PaymentCondition item
        /// </summary>
        /// <param name="id"></param>
        public void Delete(PaymentCondition paymentCondition)
        {
            Delete(paymentCondition.PaymentConditionId);
        }
    }
}