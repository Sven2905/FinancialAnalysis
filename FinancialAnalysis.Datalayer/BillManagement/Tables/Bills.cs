using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.BillManagement;
using Serilog;

namespace FinancialAnalysis.Datalayer.Accounting
{
    public class Bills : ITable
    {
        private readonly BillsStoredProcedures sp = new BillsStoredProcedures();

        public Bills()
        {
            TableName = "Bills";
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
                                 $"CREATE TABLE {TableName}(BillId int IDENTITY(1,1) PRIMARY KEY, " +
                                 "CreditorInvoiceNumber nvarchar(150) NOT NULL UNIQUE, " +
                                 "BillDate datetime, " +
                                 "BillDueDate datetime, " +
                                 "Content varbinary(MAX), " +
                                 "RefBillTypeId int )";

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
        ///     Returns all Bill records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Bill> GetAll()
        {
            IEnumerable<Bill> output = new List<Bill>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<Bill, BillType, Bill>($"dbo.{TableName}_GetAll",
                        (objBill, objBillType) =>
                        {
                            objBill.RefBillType = objBillType;
                            return objBill;
                        }, splitOn: "BillId, BillTypeId",
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
        ///     Inserts the Bill item
        /// </summary>
        /// <param name="Bill"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(Bill Bill)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result =
                        con.Query<int>(
                            $"dbo.{TableName}_Insert @CreditorInvoiceNumber, @BillDate, @BillDueDate, @Content, @RefBillTypeId ",
                            Bill);
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
        ///     Inserts the list of Bill items
        /// </summary>
        /// <param name="Bills"></param>
        public void Insert(IEnumerable<Bill> Bills)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var Bill in Bills) Insert(Bill);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns Bill by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Bill GetById(int id)
        {
            var output = new Bill();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<Bill>($"dbo.{TableName}_GetById @BillId",
                        new {BillId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Returns Bill by CreditorInvoiceNumber
        /// </summary>
        /// <param name="CreditorInvoiceNumber"></param>
        /// <returns></returns>
        public int GetByCreditorInvoiceNumber(string CreditorInvoiceNumber)
        {
            var output = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<int>($"dbo.{TableName}_GetByCreditorInvoiceNumber @CreditorInvoiceNumber",
                        new { CreditorInvoiceNumber });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetByAccountNumber' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update Bill, if not exist, insert it
        /// </summary>
        /// <param name="Bill"></param>
        public void UpdateOrInsert(Bill Bill)
        {
            if (Bill.BillId == 0 || GetById(Bill.BillId) is null)
            {
                Insert(Bill);
                return;
            }

            Update(Bill);
        }

        /// <summary>
        ///     Update Bills, if not exist insert them
        /// </summary>
        /// <param name="Bills"></param>
        public void UpdateOrInsert(IEnumerable<Bill> Bills)
        {
            foreach (var Bill in Bills) UpdateOrInsert(Bill);
        }

        /// <summary>
        ///     Update Bill
        /// </summary>
        /// <param name="Bill"></param>
        public void Update(Bill Bill)
        {
            if (Bill.BillId == 0 || GetById(Bill.BillId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute(
                        $"dbo.{TableName}_Update @BillId, @CreditorInvoiceNumber, @BillDate, @BillDueDate, @Content, @RefBillTypeId",
                        Bill);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete Bill by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @BillId", new {BillId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete Bill by Item
        /// </summary>
        /// <param name="id"></param>
        public void Delete(Bill Bill)
        {
            Delete(Bill.BillId);
        }


        public void AddReferences()
        {
            AddBillTypesReference();
        }

        private void AddBillTypesReference()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_BillTypes', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_BillTypes FOREIGN KEY(RefBillTypeId) REFERENCES BillTypes(BillTypeId) ON DELETE CASCADE";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and BillTypes",
                    e);
            }
        }
    }
}