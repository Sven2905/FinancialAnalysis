using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.SalesManagement
{
    internal class InvoicesStoredProcedures : IStoredProcedures
    {
        public InvoicesStoredProcedures()
        {
            TableName = "Invoices";
        }

        public string TableName { get; }

        /// <summary>
        ///     Check if all Stored Procedures are created, otherwise create them
        /// </summary>
        public void CheckAndCreateProcedures()
        {
            GetAllData();
            InsertData();
            GetById();
            UpdateData();
            DeleteData();
        }

        private void GetAllData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAll", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_GetAll] AS BEGIN SET NOCOUNT ON; " +
                                "SELECT i.*, t.*, pos.*, pay.*, spos.*, s.*, p.*, d.* " +
                                $"FROM {TableName} i " +
                                "LEFT JOIN InvoiceTypes t ON i.RefInvoiceTypeId = t.InvoiceTypeId " +
                                "LEFT JOIN InvoicePositions pos ON i.RefInvoicePositionId = pos.InvoicePositionId " +
                                "LEFT JOIN PaymentConditions pay ON i.RefPaymentConditionId = pay.PaymentConditionId " +
                                "LEFT JOIN SalesOrderPositions spos ON pos.RefSalesOrderPositionId = spos.SalesOrderPositionId " +
                                "LEFT JOIN SalesOrders s ON spos.RefSalesOrderId = s.SalesOrderId " +
                                "LEFT JOIN Debitors d ON s.RefDebitorId = d.DebitorId" +
                                "LEFT JOIN Products p ON spos.RefProductId = p.ProductId " +
                                "END");
                using (var connection =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (var cmd = new SqlCommand(sbSP.ToString(), connection))
                    {
                        connection.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
        }

        private void InsertData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_Insert", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_Insert] @InvoiceDate datetime, @InvoiceDueDate datetime, @RefInvoiceTypeId int, @RefPaymentConditionId int, @RefInvoicePositionId int, @PaidAmount money, @IsPaid bit AS BEGIN SET NOCOUNT ON; " +
                    $"INSERT into {TableName} (InvoiceDate, InvoiceDueDate, RefInvoiceTypeId, RefPaymentConditionId, RefInvoicePositionId, PaidAmount, IsPaid) " +
                    "VALUES (@InvoiceDate, @InvoiceDueDate, @RefInvoiceTypeId, @RefPaymentConditionId, @RefInvoicePositionId, @PaidAmount, @IsPaid); " +
                    "SELECT CAST(SCOPE_IDENTITY() as int) END");
                using (var connection =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (var cmd = new SqlCommand(sbSP.ToString(), connection))
                    {
                        connection.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
        }

        private void GetById()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetById", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_GetById] @InvoiceId int AS BEGIN SET NOCOUNT ON; " +
                    "SELECT i.*, t.*, pos.*, pay.*, spos.*, s.*, p.*, d.* " +
                    $"FROM {TableName} i " +
                    "LEFT JOIN InvoiceTypes t ON i.RefInvoiceTypeId = t.InvoiceTypeId " +
                    "LEFT JOIN InvoicePositions pos ON i.RefInvoicePositionId = pos.InvoicePositionId " +
                    "LEFT JOIN PaymentConditions pay ON i.RefPaymentConditionId = pay.PaymentConditionId " +
                    "LEFT JOIN SalesOrderPositions spos ON pos.RefSalesOrderPositionId = spos.SalesOrderPositionId " +
                    "LEFT JOIN SalesOrders s ON spos.RefSalesOrderId = s.SalesOrderId " +
                    "LEFT JOIN Debitors d ON s.RefDebitorId = d.DebitorId" +
                    "LEFT JOIN Products p ON spos.RefProductId = p.ProductId " +
                    "WHERE i.InvoiceId = @InvoiceId END");
                using (var connection =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (var cmd = new SqlCommand(sbSP.ToString(), connection))
                    {
                        connection.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
        }

        private void GetOpenInvoices()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetOpenInvoices", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_GetOpenInvoices] AS BEGIN SET NOCOUNT ON; " +
                    "SELECT i.*, t.*, pos.*, pay.*, spos.*, s.*, p.*, d.* " +
                    $"FROM {TableName} i " +
                    "LEFT JOIN InvoiceTypes t ON i.RefInvoiceTypeId = t.InvoiceTypeId " +
                    "LEFT JOIN InvoicePositions pos ON i.RefInvoicePositionId = pos.InvoicePositionId " +
                    "LEFT JOIN PaymentConditions pay ON i.RefPaymentConditionId = pay.PaymentConditionId " +
                    "LEFT JOIN SalesOrderPositions spos ON pos.RefSalesOrderPositionId = spos.SalesOrderPositionId " +
                    "LEFT JOIN SalesOrders s ON spos.RefSalesOrderId = s.SalesOrderId " +
                    "LEFT JOIN Debitors d ON s.RefDebitorId = d.DebitorId" +
                    "LEFT JOIN Products p ON spos.RefProductId = p.ProductId " +
                    "WHERE i.IsClosed = 1 END");
                using (var connection =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (var cmd = new SqlCommand(sbSP.ToString(), connection))
                    {
                        connection.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
        }

        private void UpdateData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_Update", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_Update] @InvoiceId int, @InvoiceDate datetime, @InvoiceDueDate datetime, @RefInvoiceTypeId int, @RefPaymentConditionId int, @RefInvoicePositionId int, @PaidAmount money, @IsPaid bit " +
                    "AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} " +
                    "SET InvoiceDate = @InvoiceDate, " +
                    "InvoiceDueDate = @InvoiceDueDate, " +
                    "RefInvoiceTypeId = @RefInvoiceTypeId, " +
                    "RefPaymentConditionId = @RefPaymentConditionId, " +
                    "RefInvoicePositionId = @RefInvoicePositionId, " +
                    "PaidAmount = @PaidAmount, " +
                    "IsPaid = @IsPaid " +
                    "WHERE InvoiceId = @InvoiceId END");
                using (var connection =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (var cmd = new SqlCommand(sbSP.ToString(), connection))
                    {
                        connection.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
        }

        private void DeleteData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_Delete", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_Delete] @InvoiceId int AS BEGIN SET NOCOUNT ON; " +
                    $"DELETE FROM {TableName} WHERE InvoiceId = @InvoiceId END");
                using (var connection =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (var cmd = new SqlCommand(sbSP.ToString(), connection))
                    {
                        connection.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
        }
    }
}