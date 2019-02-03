using Dapper;
using FinancialAnalysis.Models.SalesManagement;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FinancialAnalysis.Datalayer.SalesManagement
{
    public class Invoices : ITable
    {
        private readonly InvoicesStoredProcedures sp = new InvoicesStoredProcedures();

        public Invoices()
        {
            TableName = "Invoices";
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
                                 $"CREATE TABLE {TableName}(" +
                                 $"InvoiceId int IDENTITY(1,1) PRIMARY KEY, " +
                                 "InvoiceDate datetime, " +
                                 "InvoiceDueDate datetime, " +
                                 "RefInvoiceTypeId int, " +
                                 "RefPaymentConditionId int, " +
                                 "RefInvoicePositionId int, " +
                                 "PaidAmount money, " +
                                 "IsPaid bit)";

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
        ///     Returns all Invoice records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Invoice> GetAll()
        {
            IEnumerable<Invoice> output = new List<Invoice>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<Invoice, InvoiceType, Invoice>($"dbo.{TableName}_GetAll",
                        (objInvoice, objInvoiceType) =>
                        {
                            objInvoice.InvoiceType = objInvoiceType;
                            return objInvoice;
                        }, splitOn: "InvoiceId, InvoiceTypeId",
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
        ///     Inserts the Invoice item
        /// </summary>
        /// <param name="Invoice"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(Invoice Invoice)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result =
                        con.Query<int>(
                            $"dbo.{TableName}_Insert @InvoiceDate, @InvoiceDueDate, @RefInvoiceTypeId, @RefPaymentConditionId, @RefInvoicePositionId, @PaidAmount, @IsPaid ",
                            Invoice);
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
        ///     Inserts the list of Invoice items
        /// </summary>
        /// <param name="Invoices"></param>
        public void Insert(IEnumerable<Invoice> Invoices)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var Invoice in Invoices)
                    {
                        Insert(Invoice);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns Invoice by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Invoice GetById(int id)
        {
            var output = new Invoice();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<Invoice>($"dbo.{TableName}_GetById @InvoiceId",
                        new { InvoiceId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update Invoice, if not exist, insert it
        /// </summary>
        /// <param name="Invoice"></param>
        public void UpdateOrInsert(Invoice Invoice)
        {
            if (Invoice.InvoiceId == 0 || GetById(Invoice.InvoiceId) is null)
            {
                Insert(Invoice);
                return;
            }

            Update(Invoice);
        }

        /// <summary>
        ///     Update Invoices, if not exist insert them
        /// </summary>
        /// <param name="Invoices"></param>
        public void UpdateOrInsert(IEnumerable<Invoice> Invoices)
        {
            foreach (var Invoice in Invoices)
            {
                UpdateOrInsert(Invoice);
            }
        }

        /// <summary>
        ///     Update Invoice
        /// </summary>
        /// <param name="Invoice"></param>
        public void Update(Invoice Invoice)
        {
            if (Invoice.InvoiceId == 0 || GetById(Invoice.InvoiceId) is null)
            {
                return;
            }

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute(
                        $"dbo.{TableName}_Update @InvoiceId, @InvoiceDate, @InvoiceDueDate, @RefInvoiceTypeId, @RefPaymentConditionId, @RefInvoicePositionId, @PaidAmount, @IsPaid",
                        Invoice);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete Invoice by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @InvoiceId", new { InvoiceId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete Invoice by Item
        /// </summary>
        /// <param name="id"></param>
        public void Delete(Invoice Invoice)
        {
            Delete(Invoice.InvoiceId);
        }

        public void AddReferences()
        {
            AddPaymentConditionsReference();
            AddInvoiceTypesReference();
        }

        private void AddInvoiceTypesReference()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_InvoiceTypes', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_InvoiceTypes FOREIGN KEY(RefInvoiceTypeId) REFERENCES InvoiceTypes(InvoiceTypeId) ON DELETE CASCADE";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and InvoiceTypes",
                    e);
            }
        }

        private void AddPaymentConditionsReference()
        {
            string refTable = "PaymentConditions";

            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_{refTable}', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_{refTable} FOREIGN KEY(RefPaymentConditionId) REFERENCES {refTable}(PaymentConditionId) ON DELETE CASCADE";

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