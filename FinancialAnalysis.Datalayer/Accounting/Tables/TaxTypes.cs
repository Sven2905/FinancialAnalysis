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
    public class TaxTypes : ITable
    {
        private readonly TaxTypesStoredProcedures sp = new TaxTypesStoredProcedures();

        public TaxTypes()
        {
            TableName = "TaxTypes";
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
                                 "TaxTypeId int IDENTITY(1,1) PRIMARY KEY, " +
                                 "Description nvarchar(50) NOT NULL, " +
                                 "DescriptionShort nvarchar(50) NOT NULL, " +
                                 "AmountOfTax decimal NOT NULL, " +
                                 "TaxCategory int NOT NULL, " +
                                 "RefCostAccount int, " +
                                 "RefAccountNotPayable int )";

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
        ///     Returns all TaxType records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TaxType> GetAll()
        {
            IEnumerable<TaxType> output = new List<TaxType>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<TaxType>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the TaxType item
        /// </summary>
        /// <param name="taxType"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(TaxType taxType)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result =
                        con.Query<int>(
                            $"dbo.{TableName}_Insert @Description, @DescriptionShort, @AmountOfTax, @TaxCategory, @RefCostAccount, @RefAccountNotPayable",
                            taxType);
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
        ///     Inserts the list of TaxType items
        /// </summary>
        /// <param name="taxTypes"></param>
        public void Insert(IEnumerable<TaxType> taxTypes)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var taxType in taxTypes) Insert(taxType);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns TaxType by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TaxType GetById(int id)
        {
            var output = new TaxType();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<TaxType>($"dbo.{TableName}_GetById @TaxTypeId",
                        new {TaxTypeId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update TaxType, if not exist, insert it
        /// </summary>
        /// <param name="taxType"></param>
        public void UpdateOrInsert(TaxType taxType)
        {
            if (taxType.TaxTypeId == 0 || GetById(taxType.TaxTypeId) is null)
            {
                Insert(taxType);
                return;
            }

            Update(taxType);
        }

        /// <summary>
        ///     Update TaxTypes, if not exist insert them
        /// </summary>
        /// <param name="taxTypes"></param>
        public void UpdateOrInsert(IEnumerable<TaxType> taxTypes)
        {
            foreach (var taxType in taxTypes) UpdateOrInsert(taxType);
        }

        /// <summary>
        ///     Update TaxType
        /// </summary>
        /// <param name="taxType"></param>
        public void Update(TaxType taxType)
        {
            if (taxType.TaxTypeId == 0 || GetById(taxType.TaxTypeId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute(
                        $"dbo.{TableName}_Update @TaxTypeId, @Description, @DescriptionShort, @AmountOfTax, @TaxCategory, @RefCostAccount, @RefAccountNotPayable",
                        taxType);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete TaxType by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @TaxTypeId", new {TaxTypeId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete TaxType by Item
        /// </summary>
        /// <param name="id"></param>
        public void Delete(TaxType taxType)
        {
            Delete(taxType.TaxTypeId);
        }
    }
}