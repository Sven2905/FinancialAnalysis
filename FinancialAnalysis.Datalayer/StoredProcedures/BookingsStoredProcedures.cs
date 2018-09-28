using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Datalayer.StoredProcedures
{
    class BookingsStoredProcedures : IStoredProcedures
    {
        public string TableName { get; }

        public BookingsStoredProcedures()
        {
            TableName = "Bookings";
        }

        /// <summary>
        /// Check if all Stored Procedures are created, otherwise create them
        /// </summary>
        public void CheckAndCreateProcedures()
        {
            GetAllData();
            InsertData();
            GetById();
        }

        private void GetAllData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAll", DatabaseNames.FinancialAnalysisDB))
            {
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_GetAll] AS BEGIN SET NOCOUNT ON; " +
                    $"SELECT b.BookingId, b.Description, b.Amount, b.Date, " +
                    $"s.ScannedDocumentId, s.Content, s.Date, s.FileName, s.RefBookingId, " +
                    $"c.CreditId, c.Amount, c.RefCostAccountId, c.RefBookingId, " +
                    $"d.DebitId, d.Amount, d.RefCostAccountId, d.RefBookingId FROM {TableName} b " +
                    $"INNER JOIN Credits c ON b.BookingId = c.RefBookingId " +
                    $"INNER JOIN Debits d ON b.BookingId = d.RefBookingId " +
                    $"INNER JOIN ScannedDocuments s ON b.BookingId = s.RefBookingId " +
                    $"END");
                using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (SqlCommand cmd = new SqlCommand(sbSP.ToString(), connection))
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
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_Insert] @Description nvarchar(150), @Amount money, @Date datetime " +
                                $"AS BEGIN SET NOCOUNT ON; " +
                                $"INSERT into {TableName} (Description, Amount, Date) " +
                                $"VALUES (@Description, @Amount, @Date); " +
                                $"SELECT CAST(SCOPE_IDENTITY() as int) END");
                using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (SqlCommand cmd = new SqlCommand(sbSP.ToString(), connection))
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
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_GetById] @BookingId int AS BEGIN SET NOCOUNT ON; SELECT BookingId, Description, Amount, Date " +
                    $"FROM {TableName} " +
                    $"WHERE BookingId = @BookingId END");
                using (SqlConnection connection =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (SqlCommand cmd = new SqlCommand(sbSP.ToString(), connection))
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
