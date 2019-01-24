using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.PaymentManagement;
using Serilog;

namespace FinancialAnalysis.Datalayer.PaymentManagement
{
    public class PaymentTypes : ITable
    {
        private readonly PaymentTypesStoredProcedures sp = new PaymentTypesStoredProcedures();

        public PaymentTypes()
        {
            TableName = "PaymentTypes";
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
                                 "(PaymentTypeId int IDENTITY(1,1) PRIMARY KEY, " +
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
        ///     Returns all PaymentType records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PaymentType> GetAll()
        {
            IEnumerable<PaymentType> output = new List<PaymentType>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<PaymentType>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the PaymentType item
        /// </summary>
        /// <param name="PaymentType"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(PaymentType PaymentType)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Name, @Description ",
                        PaymentType);
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
        ///     Inserts the list of PaymentType items
        /// </summary>
        /// <param name="PaymentTypes"></param>
        public void Insert(IEnumerable<PaymentType> PaymentTypes)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var PaymentType in PaymentTypes) Insert(PaymentType);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns PaymentType by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PaymentType GetById(int id)
        {
            var output = new PaymentType();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<PaymentType>(
                        $"dbo.{TableName}_GetById @PaymentTypeId", new {PaymentTypeId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update PaymentType, if not exist, insert it
        /// </summary>
        /// <param name="PaymentType"></param>
        public void UpdateOrInsert(PaymentType PaymentType)
        {
            if (PaymentType.PaymentTypeId == 0 ||
                GetById(PaymentType.PaymentTypeId) is null)
            {
                Insert(PaymentType);
                return;
            }

            Update(PaymentType);
        }

        /// <summary>
        ///     Update PaymentTypes, if not exist insert them
        /// </summary>
        /// <param name="PaymentTypes"></param>
        public void UpdateOrInsert(IEnumerable<PaymentType> PaymentTypes)
        {
            foreach (var PaymentType in PaymentTypes) UpdateOrInsert(PaymentType);
        }

        /// <summary>
        ///     Update PaymentType
        /// </summary>
        /// <param name="PaymentType"></param>
        public void Update(PaymentType PaymentType)
        {
            if (PaymentType.PaymentTypeId == 0 ||
                GetById(PaymentType.PaymentTypeId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @PaymentTypeId, @Name, @Description ",
                        PaymentType);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete PaymentType by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @PaymentTypeId", new {PaymentTypeId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete PaymentType by Item
        /// </summary>
        /// <param name="id"></param>
        public void Delete(PaymentType PaymentType)
        {
            Delete(PaymentType.PaymentTypeId);
        }
    }
}