using Dapper;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.ClientManagement;
using FinancialAnalysis.Models.ProductManagement;
using FinancialAnalysis.Models.ProjectManagement;
using FinancialAnalysis.Models.SalesManagement;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Utilities;

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
                                 "PaidDate datetime, " +
                                 "RefEmployeeId int, " +
                                 "RefInvoiceTypeId int, " +
                                 "RefPaymentConditionId int, " +
                                 "TotalAmount money, " +
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
            var InvoiceDictionary = new Dictionary<int, Invoice>();
            using (var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                con.Query(
                    $"dbo.{TableName}_GetAll",
                    new[]
                    {
                        typeof(Invoice),
                        typeof(InvoiceType),
                        typeof(InvoicePosition),
                        typeof(PaymentCondition),
                        typeof(SalesOrderPosition),
                        typeof(SalesOrder),
                        typeof(Debitor),
                        typeof(Client),
                        typeof(Company),
                        typeof(Product),
                        typeof(TaxType),
                        typeof(Employee),
                        typeof(InvoiceReminder)
                    },
                    objects =>
                    {
                        var Invoice = objects[0] as Invoice;
                        var InvoiceType = objects[1] as InvoiceType;
                        var InvoicePosition = objects[2] as InvoicePosition;
                        var PaymentCondition = objects[3] as PaymentCondition;
                        var SalesOrderPosition = objects[4] as SalesOrderPosition;
                        var SalesOrder = objects[5] as SalesOrder;
                        var Debitor = objects[6] as Debitor;
                        var Client = objects[7] as Client;
                        var Company = objects[8] as Company;
                        var Product = objects[9] as Product;
                        var TaxType = objects[10] as TaxType;
                        var Employee = objects[11] as Employee;
                        var InvoiceReminder = objects[12] as InvoiceReminder;

                        if (!InvoiceDictionary.TryGetValue(SalesOrder.SalesOrderId, out Invoice InvoiceEntry))
                        {
                            InvoiceEntry = Invoice;
                            InvoiceDictionary.Add(InvoiceEntry.InvoiceId, InvoiceEntry);
                        }

                        InvoiceEntry.InvoiceType = InvoiceType;
                        Client.Company = Company;
                        Debitor.Client = Client;
                        InvoiceEntry.Debitor = Debitor;
                        InvoiceEntry.Employee = Employee;

                        if (InvoiceEntry.InvoicePositions.SingleOrDefault(x => x.InvoicePositionId == InvoicePosition.InvoicePositionId) == null)
                        {
                            if (Product != null)
                            {
                                Product.TaxType = TaxType;
                            }
                            InvoicePosition.Product = Product;
                            InvoicePosition.SalesOrderPosition = SalesOrderPosition;
                            InvoiceEntry.InvoicePositions.Add(InvoicePosition);
                        }

                        if (InvoiceEntry.InvoiceReminders.SingleOrDefault(x => x.InvoiceReminderId == InvoiceReminder.InvoiceReminderId) == null)
                        {
                            InvoiceEntry.InvoiceReminders.Add(InvoiceReminder);
                        }

                        return InvoiceEntry;
                    },
                    splitOn:
                    "InvoiceId, InvoiceTypeId, InvoicePositionId, PaymentConditionId, SalesOrderPositionId, SalesOrderId, DebitorId, ClientId, CompanyId, ProductId, TaxTypeId, EmployeeId, InvoiceReminderId");
            }

            return InvoiceDictionary.Values;
        }

        /// <summary>
        ///     Returns all Invoice records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Invoice> GetOpenInvoices()
        {
            var InvoiceDictionary = new Dictionary<int, Invoice>();
            using (var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                con.Query(
                    $"dbo.{TableName}_GetOpenInvoices",
                    new[]
                    {
                        typeof(Invoice),
                        typeof(InvoiceType),
                        typeof(InvoicePosition),
                        typeof(PaymentCondition),
                        typeof(SalesOrderPosition),
                        typeof(SalesOrder),
                        typeof(Debitor),
                        typeof(Client),
                        typeof(Company),
                        typeof(Product),
                        typeof(TaxType),
                        typeof(Employee),
                        typeof(InvoiceReminder)
                    },
                    objects =>
                    {
                        var Invoice = objects[0] as Invoice;
                        var InvoiceType = objects[1] as InvoiceType;
                        var InvoicePosition = objects[2] as InvoicePosition;
                        var PaymentCondition = objects[3] as PaymentCondition;
                        var SalesOrderPosition = objects[4] as SalesOrderPosition;
                        var SalesOrder = objects[5] as SalesOrder;
                        var Debitor = objects[6] as Debitor;
                        var Client = objects[7] as Client;
                        var Company = objects[8] as Company;
                        var Product = objects[9] as Product;
                        var TaxType = objects[10] as TaxType;
                        var Employee = objects[11] as Employee;
                        var InvoiceReminder = objects[12] as InvoiceReminder;

                        if (!InvoiceDictionary.TryGetValue(SalesOrder.SalesOrderId, out Invoice InvoiceEntry))
                        {
                            InvoiceEntry = Invoice;
                            InvoiceDictionary.Add(InvoiceEntry.InvoiceId, InvoiceEntry);
                        }

                        InvoiceEntry.InvoiceType = InvoiceType;
                        Client.Company = Company;
                        Debitor.Client = Client;
                        InvoiceEntry.Debitor = Debitor;
                        InvoiceEntry.Employee = Employee;

                        if (InvoiceEntry.InvoicePositions.SingleOrDefault(x => x.InvoicePositionId == InvoicePosition.InvoicePositionId) == null)
                        {
                            if (Product != null)
                            {
                                Product.TaxType = TaxType;
                            }
                            InvoicePosition.Product = Product;
                            InvoicePosition.SalesOrderPosition = SalesOrderPosition;
                            InvoiceEntry.InvoicePositions.Add(InvoicePosition);
                        }

                        if (InvoiceEntry.InvoiceReminders.SingleOrDefault(x => x.InvoiceReminderId == InvoiceReminder.InvoiceReminderId) == null)
                        {
                            InvoiceEntry.InvoiceReminders.Add(InvoiceReminder);
                        }

                        return InvoiceEntry;
                    },
                    splitOn:
                    "InvoiceId, InvoiceTypeId, InvoicePositionId, PaymentConditionId, SalesOrderPositionId, SalesOrderId, DebitorId, ClientId, CompanyId, ProductId, TaxTypeId, EmployeeId, InvoiceReminderId");
            }

            return InvoiceDictionary.Values;
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
                            $"dbo.{TableName}_Insert @InvoiceDate, @InvoiceDueDate, @PaidDate, @RefEmployeeId, @RefInvoiceTypeId, @RefPaymentConditionId, @TotalAmount, @PaidAmount, @IsPaid ",
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
            var InvoiceDictionary = new Dictionary<int, Invoice>();
            using (var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                con.Query(
                    $"dbo.{TableName}_GetById @InvoiceId",
                    new[]
                    {
                        typeof(Invoice),
                        typeof(InvoiceType),
                        typeof(InvoicePosition),
                        typeof(PaymentCondition),
                        typeof(SalesOrderPosition),
                        typeof(SalesOrder),
                        typeof(Debitor),
                        typeof(Client),
                        typeof(Company),
                        typeof(Product),
                        typeof(TaxType),
                        typeof(Employee),
                        typeof(InvoiceReminder)
                    },
                    objects =>
                    {
                        var Invoice = objects[0] as Invoice;
                        var InvoiceType = objects[1] as InvoiceType;
                        var InvoicePosition = objects[2] as InvoicePosition;
                        var PaymentCondition = objects[3] as PaymentCondition;
                        var SalesOrderPosition = objects[4] as SalesOrderPosition;
                        var SalesOrder = objects[5] as SalesOrder;
                        var Debitor = objects[6] as Debitor;
                        var Client = objects[7] as Client;
                        var Company = objects[8] as Company;
                        var Product = objects[9] as Product;
                        var TaxType = objects[10] as TaxType;
                        var Employee = objects[11] as Employee;
                        var InvoiceReminder = objects[12] as InvoiceReminder;

                        if (!InvoiceDictionary.TryGetValue(SalesOrder.SalesOrderId, out Invoice InvoiceEntry))
                        {
                            InvoiceEntry = Invoice;
                            InvoiceDictionary.Add(InvoiceEntry.InvoiceId, InvoiceEntry);
                        }

                        InvoiceEntry.InvoiceType = InvoiceType;
                        Client.Company = Company;
                        Debitor.Client = Client;
                        InvoiceEntry.Debitor = Debitor;
                        InvoiceEntry.Employee = Employee;

                        if (InvoiceEntry.InvoicePositions.SingleOrDefault(x => x.InvoicePositionId == InvoicePosition.InvoicePositionId) == null)
                        {
                            if (Product != null)
                            {
                                Product.TaxType = TaxType;
                            }
                            InvoicePosition.Product = Product;
                            InvoicePosition.SalesOrderPosition = SalesOrderPosition;
                            InvoiceEntry.InvoicePositions.Add(InvoicePosition);
                        }

                        if (InvoiceEntry.InvoiceReminders.SingleOrDefault(x => x.InvoiceReminderId == InvoiceReminder.InvoiceReminderId) == null)
                        {
                            InvoiceEntry.InvoiceReminders.Add(InvoiceReminder);
                        }

                        return InvoiceEntry;
                    },
                    new { InvoiceId = id },
                    splitOn:
                    "InvoiceId, InvoiceTypeId, InvoicePositionId, PaymentConditionId, SalesOrderPositionId, SalesOrderId, DebitorId, ClientId, CompanyId, ProductId, TaxTypeId, EmployeeId, InvoiceReminderId");
            }

            return InvoiceDictionary.Values.FirstOrDefault();
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
                        $"dbo.{TableName}_Update @InvoiceId, @InvoiceDate, @InvoiceDueDate, @PaidDate, @RefEmployeeId, @RefInvoiceTypeId, @RefPaymentConditionId, @TotalAmount, @PaidAmount, @IsPaid",
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
            AddEmployeesReference();
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
                Log.Error($"Exception occured while creating reference between '{TableName}' and {refTable}",
                    e);
            }
        }

        private void AddEmployeesReference()
        {
            const string refTable = "Employees";

            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_{refTable}', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_{refTable} FOREIGN KEY(RefEmployeeId) REFERENCES {refTable}(EmployeeId) ON DELETE CASCADE";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and {refTable}",
                    e);
            }
        }
    }
}