using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.InvoiceManagement;
using Serilog;

namespace FinancialAnalysis.Datalayer.InvoiceManagement
{
    public class InvoiceTypes : ITable
    {
        private readonly InvoiceTypesStoredProcedures sp = new InvoiceTypesStoredProcedures();

        public InvoiceTypes()
        {
            TableName = "InvoiceTypes";
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
                                 "(InvoiceTypeId int IDENTITY(1,1) PRIMARY KEY, " +
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
        ///     Returns all InvoiceType records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<InvoiceType> GetAll()
        {
            IEnumerable<InvoiceType> output = new List<InvoiceType>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<InvoiceType>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the InvoiceType item
        /// </summary>
        /// <param name="InvoiceType"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(InvoiceType InvoiceType)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Name, @Description ",
                        InvoiceType);
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
        ///     Inserts the list of InvoiceType items
        /// </summary>
        /// <param name="InvoiceTypes"></param>
        public void Insert(IEnumerable<InvoiceType> InvoiceTypes)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var InvoiceType in InvoiceTypes) Insert(InvoiceType);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns InvoiceType by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InvoiceType GetById(int id)
        {
            var output = new InvoiceType();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<InvoiceType>(
                        $"dbo.{TableName}_GetById @InvoiceTypeId", new {InvoiceTypeId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update InvoiceType, if not exist, insert it
        /// </summary>
        /// <param name="InvoiceType"></param>
        public void UpdateOrInsert(InvoiceType InvoiceType)
        {
            if (InvoiceType.InvoiceTypeId == 0 ||
                GetById(InvoiceType.InvoiceTypeId) is null)
            {
                Insert(InvoiceType);
                return;
            }

            Update(InvoiceType);
        }

        /// <summary>
        ///     Update InvoiceTypes, if not exist insert them
        /// </summary>
        /// <param name="InvoiceTypes"></param>
        public void UpdateOrInsert(IEnumerable<InvoiceType> InvoiceTypes)
        {
            foreach (var InvoiceType in InvoiceTypes) UpdateOrInsert(InvoiceType);
        }

        /// <summary>
        ///     Update InvoiceType
        /// </summary>
        /// <param name="InvoiceType"></param>
        public void Update(InvoiceType InvoiceType)
        {
            if (InvoiceType.InvoiceTypeId == 0 ||
                GetById(InvoiceType.InvoiceTypeId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @InvoiceTypeId, @Name, @Description ",
                        InvoiceType);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete InvoiceType by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @InvoiceTypeId", new {InvoiceTypeId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete InvoiceType by Item
        /// </summary>
        /// <param name="id"></param>
        public void Delete(InvoiceType InvoiceType)
        {
            Delete(InvoiceType.InvoiceTypeId);
        }
    }
}