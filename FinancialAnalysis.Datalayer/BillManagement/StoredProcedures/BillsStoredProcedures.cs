using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.Accounting
{
    internal class BillsStoredProcedures : IStoredProcedures
    {
        public BillsStoredProcedures()
        {
            TableName = "Bills";
        }

        public string TableName { get; }

        /// <summary>
        ///     Check if all Stored Procedures are created, otherwise create them
        /// </summary>
        public void CheckAndCreateProcedures()
        {
            GetAllData();
            GetAllVisibleData();
            InsertData();
            GetById();
            GetByCreditorInvoiceNumber();
            UpdateData();
            DeleteData();
        }

        private void GetAllData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAll", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_GetAll] AS BEGIN SET NOCOUNT ON; " +
                                "SELECT b.BillId, b.CreditorInvoiceNumber, b.BillDate, b.BillDueDate, a.Content, b.RefBillTypeId, " +
                                $"t.BillTypeId, t.Name, t.Description " +
                                $"FROM {TableName} b " +
                                "LEFT JOIN BillTypes t ON b.RefBillTypeId = t.BillTypeId " +
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

        private void GetAllVisibleData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAllVisible", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_GetAllVisible] AS BEGIN SET NOCOUNT ON; " +
                                "SELECT b.BillId, b.CreditorInvoiceNumber, b.BillDate, b.BillDueDate, a.Content, b.RefBillTypeId, " +
                                $"t.BillTypeId, t.Name, t.Description " +
                                $"FROM {TableName} b " +
                                "LEFT JOIN BillTypes t ON b.RefBillTypeId = t.BillTypeId " +
                                "WHERE IsVisible = 1 END");
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
                    $"CREATE PROCEDURE [{TableName}_Insert] @CreditorInvoiceNumber nvarchar(150), @BillDate datetime, @BillDueDate datetime, @Content varbinary(MAX), @RefBillTypeId int AS BEGIN SET NOCOUNT ON; " +
                    $"INSERT into {TableName} (CreditorInvoiceNumber, BillDate, BillDueDate, Content, RefBillTypeId) " +
                    "VALUES (@CreditorInvoiceNumber, @BillDate, @BillDueDate, @Content, @RefBillTypeId); " +
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
                    $"CREATE PROCEDURE [{TableName}_GetById] @BillId int AS BEGIN SET NOCOUNT ON; SELECT BillId, CreditorInvoiceNumber, BillDate, BillDueDate, Content, RefBillTypeId " +
                    $"FROM {TableName} " +
                    "WHERE BillId = @BillId END");
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

        private void GetByCreditorInvoiceNumber()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetByCreditorInvoiceNumber", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_GetByCreditorInvoiceNumber] @CreditorInvoiceNumber int AS BEGIN SET NOCOUNT ON; " +
                    $"SELECT BillId, CreditorInvoiceNumber, BillDate, BillDueDate, Content, RefBillTypeId " +
                    $"FROM {TableName} " +
                    "WHERE CreditorInvoiceNumber = @CreditorInvoiceNumber END");
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
                    $"CREATE PROCEDURE [{TableName}_Update] @BillId int, @CreditorInvoiceNumber nvarchar(150), @BillDate datetime, @BillDueDate datetime, @Content varbinary(MAX), @RefBillTypeId int " +
                    "AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} " +
                    "SET CreditorInvoiceNumber = @CreditorInvoiceNumber, " +
                    "BillDate = @BillDate, " +
                    "BillDueDate = @BillDueDate, " +
                    "Content = @Content, " +
                    "RefBillTypeId = @RefBillTypeId " +
                    "WHERE BillId = @BillId END");
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
                    $"CREATE PROCEDURE [{TableName}_Delete] @BillId int AS BEGIN SET NOCOUNT ON; DELETE FROM {TableName} WHERE BillId = @BillId END");
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