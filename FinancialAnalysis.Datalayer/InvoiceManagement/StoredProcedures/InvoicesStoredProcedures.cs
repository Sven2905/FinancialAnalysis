using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.Accounting
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
                                "SELECT i.InvoiceId, i.InvoiceDate, i.InvoiceDueDate, i.RefShipmentId, i.RefInvoiceTypeId, " +
                                $"t.InvoiceTypeId, t.Name, t.Description " +
                                $"FROM {TableName} i " +
                                "LEFT JOIN InvoiceTypes t ON i.RefInvoiceTypeId = t.InvoiceTypeId " +
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
                    $"CREATE PROCEDURE [{TableName}_Insert] @InvoiceDate datetime, @InvoiceDueDate datetime, @RefShipmentId int, @RefInvoiceTypeId int AS BEGIN SET NOCOUNT ON; " +
                    $"INSERT into {TableName} (InvoiceDate, InvoiceDueDate, RefShipmentId, RefInvoiceTypeId) " +
                    "VALUES (@InvoiceDate, @InvoiceDueDate, @RefShipmentId, @RefInvoiceTypeId); " +
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
                    $"CREATE PROCEDURE [{TableName}_GetById] @InvoiceId int AS BEGIN SET NOCOUNT ON; SELECT InvoiceId, InvoiceDate, InvoiceDueDate, RefShipmentId, RefInvoiceTypeId " +
                    $"FROM {TableName} " +
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

        private void UpdateData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_Update", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_Update] @InvoiceId int, @InvoiceDate datetime, @InvoiceDueDate datetime, @RefShipmentId int, @RefInvoiceTypeId int " +
                    "AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} " +
                    "SET InvoiceDate = @InvoiceDate, " +
                    "InvoiceDueDate = @InvoiceDueDate, " +
                    "RefShipmentId = @RefShipmentId, " +
                    "RefInvoiceTypeId = @RefInvoiceTypeId " +
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
                    $"CREATE PROCEDURE [{TableName}_Delete] @InvoiceId int AS BEGIN SET NOCOUNT ON; DELETE FROM {TableName} WHERE InvoiceId = @InvoiceId END");
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