using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.Accounting
{
    public class CostCenterBudgetsStoredProcedures : IStoredProcedures
    {
        public CostCenterBudgetsStoredProcedures()
        {
            TableName = "CostCenterBudgets";
        }

        public string TableName { get; }

        /// <summary>
        ///     Check if all Stored Procedures are created, otherwise create them
        /// </summary>
        public void CheckAndCreateProcedures()
        {
            InsertData();
            GetAllData();
            GetAnnuallyCosts();
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
                                "SELECT * " +
                                $"FROM {TableName} " +
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

        private void GetById()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetById", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_GetById] @CostCenterBudgetId int AS BEGIN SET NOCOUNT ON; " +
                    $"SELECT * " +
                    $"FROM {TableName} " +
                    "WHERE CostCenterBudgetId = @CostCenterBudgetId END");
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

        private void GetAnnuallyCosts()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAnnuallyCosts", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_GetAnnuallyCosts] @RefCostCenterId int, @Year int AS BEGIN SET NOCOUNT ON; " +
                    $"SELECT SUM(b.Amount) AS Amount, b.RefCostCenterId, MONTH(b.Date) AS MonthIndex " +
                    $"FROM Bookings b " +
                    "INNER JOIN Debits d ON b.BookingId = d.RefBookingId " +
                    "WHERE YEAR(b.Date) = @Year " +
                    "AND b.RefCostCenterId = @RefCostCenterId " +
                    "GROUP BY b.RefCostCenterId, MONTH(b.Date) END");
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
                    $"CREATE PROCEDURE [{TableName}_Insert] @Year int, @RefCostCenterId int, @January money, @February money, @March money, @April money, @May money, @June money, @July money, @August money, @September money, @October money, @November money, @December money AS BEGIN SET NOCOUNT ON; " +
                    $"INSERT into {TableName} (Year, RefCostCenterId, January, February, March, April, May, June, July, August, September, October, November, December) " +
                    "VALUES (@Year, @RefCostCenterId, @January, @February, @March, @April, @May, @June, @July, @August, @September, @October, @November, @December); " +
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

        private void UpdateData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_Update", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_Update] @CostCenterBudgetId int, @Year int, @RefCostCenterId int, @January money, @February money, @March money, @April money, @May money, @June money, @July money, @August money, @September money, @October money, @November money, @December money " +
                    "AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} " +
                    "SET Year = @Year, " +
                    "RefCostCenterId = @RefCostCenterId, " +
                    "January = @January, " +
                    "February = @February, " +
                    "March = @March, " +
                    "April = @April, " +
                    "May = @May, " +
                    "June = @June, " +
                    "July = @July, " +
                    "August = @August, " +
                    "September = @September, " +
                    "October = @October, " +
                    "November = @November, " +
                    "December = @December " +
                    "WHERE CostCenterBudgetId = @CostCenterBudgetId " +
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

        private void DeleteData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_Delete", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_Delete] @CostCenterBudgetId int AS BEGIN SET NOCOUNT ON; " +
                    $"DELETE FROM {TableName} " +
                    "WHERE CostCenterBudgetId = @CostCenterBudgetId END");
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