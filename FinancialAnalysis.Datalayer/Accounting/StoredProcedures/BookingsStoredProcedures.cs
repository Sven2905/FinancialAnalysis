using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.Accounting
{
    internal class BookingsStoredProcedures : IStoredProcedures
    {
        public BookingsStoredProcedures()
        {
            TableName = "Bookings";
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
            GetByConditions();
        }

        private void GetAllData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAll", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_GetAll] AS BEGIN SET NOCOUNT ON; " +
                                "SELECT b.*, s.*, c.*, d.* " +
                                $"FROM {TableName} b " +
                                "INNER JOIN Credits c ON b.BookingId = c.RefBookingId " +
                                "INNER JOIN Debits d ON b.BookingId = d.RefBookingId " +
                                "LEFT JOIN ScannedDocuments s ON b.BookingId = s.RefBookingId " +
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
                    $"CREATE PROCEDURE [{TableName}_Insert] @Description nvarchar(150), @Amount money, @RefCostCenterId int, @Date datetime " +
                    "AS BEGIN SET NOCOUNT ON; " +
                    $"INSERT into {TableName} (Description, Amount, RefCostCenterId, Date) " +
                    "VALUES (@Description, @Amount, @RefCostCenterId, @Date); " +
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
                    $"CREATE PROCEDURE [{TableName}_GetById] @BookingId int AS BEGIN SET NOCOUNT ON; " +
                    "SELECT b.*, s.*, c.*, d.* " +
                    $"FROM {TableName} b " +
                    "INNER JOIN Credits c ON b.BookingId = c.RefBookingId " +
                    "INNER JOIN Debits d ON b.BookingId = d.RefBookingId " +
                    "LEFT JOIN ScannedDocuments s ON b.BookingId = s.RefBookingId " +
                    "WHERE BookingId = @BookingId END");
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

        private void GetByConditions()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetByConditions", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_GetByConditions] @StartDate datetime, @EndDate datetime, @CreditId int, @DebitId int AS BEGIN SET NOCOUNT ON; " +
                    "SELECT b.*, s.*, c.*, d.* " +
                    $"FROM {TableName} b " +
                    "INNER JOIN Credits c ON b.BookingId = c.RefBookingId " +
                    "INNER JOIN Debits d ON b.BookingId = d.RefBookingId " +
                    "LEFT JOIN ScannedDocuments s ON b.BookingId = s.RefBookingId " +
                    "WHERE " +
                    "b.Date >= @StartDate " +
                    "AND b.Date <= @EndDate " +
                    "AND c.CreditId = isnull(@CreditId,CreditId) " +
                    "AND d.DebitId = isnull(@DebitId,DebitId) " +
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
    }
}