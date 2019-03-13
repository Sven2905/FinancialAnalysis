using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.Accounting
{
    public class BalanceAccountsStoredProcedures : IStoredProcedures
    {
        public BalanceAccountsStoredProcedures()
        {
            TableName = "BalanceAccounts";
        }

        public string TableName { get; }

        /// <summary>
        ///     Check if all Stored Procedures are created, otherwise create them
        /// </summary>
        public void CheckAndCreateProcedures()
        {
            InsertData();
            GetAllData();
            GetAllActive();
            GetAllPassiva();
            GetById();
            UpdateData();
            DeleteData();
        }

        private void GetAllData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAll", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_GetAll] AS BEGIN SET NOCOUNT ON; " +
                    $"SELECT * " +
                    $"FROM {TableName} END");
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

        private void GetAllActive()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAllActive", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_GetAllActive] AS BEGIN SET NOCOUNT ON; " +
                    "SELECT b.*, ca.*, SUM(c.Amount) as TotalCreditAmount, SUM(d.Amount) as TotalDebitAmount " +
                    $"FROM {TableName} b " +
                    "LEFT JOIN CostAccounts ca ON b.BalanceAccountId = ca.RefActiveBalanceAccountId " +
                    "LEFT JOIN Credits c ON c.RefCostAccountId = ca.CostAccountId " +
                    "LEFT JOIN Debits d ON d.RefCostAccountId = ca.CostAccountId " +
                    "GROUP BY b.AccountType, b.BalanceAccountId, b.Name, b.ParentId, " +
                    "ca.AccountNumber, ca.CostAccountId, ca.Description, ca.IsEditable, ca.IsVisible, ca.RefActiveBalanceAccountId, " +
                    "ca.RefCostAccountCategoryId, ca.RefGainAndLossAccountId, ca.RefPassiveBalanceAccountId, ca.RefTaxTypeId " +
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

        private void GetAllPassiva()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAllPassiva", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_GetAllPassiva] AS BEGIN SET NOCOUNT ON; " +
                    "SELECT b.*, ca.*, SUM(c.Amount) as TotalCreditAmount, SUM(d.Amount) as TotalDebitAmount  " +
                    $"FROM {TableName} b " +
                    "LEFT JOIN CostAccounts ca ON b.BalanceAccountId = ca.RefPassiveBalanceAccountId " +
                    "LEFT JOIN Credits c ON ca.CostAccountId = c.RefCostAccountId " +
                    "LEFT JOIN Debits d ON ca.CostAccountId = d.RefCostAccountId " +
                    "GROUP BY b.AccountType, b.BalanceAccountId, b.Name, b.ParentId, " +
                    "ca.AccountNumber, ca.CostAccountId, ca.Description, ca.IsEditable, ca.IsVisible, ca.RefActiveBalanceAccountId, " +
                    "ca.RefCostAccountCategoryId, ca.RefGainAndLossAccountId, ca.RefPassiveBalanceAccountId, ca.RefTaxTypeId " +
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
                    $"CREATE PROCEDURE [{TableName}_Insert] @Name nvarchar(MAX), @ParentId int, @AccountType int AS BEGIN SET NOCOUNT ON; " +
                    $"INSERT into {TableName} (Name, ParentId, AccountType) " +
                    "VALUES (@Name, @ParentId, @AccountType); " +
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
                    $"CREATE PROCEDURE [{TableName}_GetById] @BalanceAccountId int AS BEGIN SET NOCOUNT ON; " +
                    $"SELECT * " +
                    $"FROM {TableName} " +
                    "WHERE BalanceAccountId = @BalanceAccountId END");
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
                    $"CREATE PROCEDURE [{TableName}_Update] @BalanceAccountId int, @Name nvarchar(MAX), @ParentId int, @AccountType int " +
                    "AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} " +
                    "SET Name = @Name, " +
                    "ParentId = @ParentId, " +
                    "AccountType = @AccountType " +
                    "WHERE BalanceAccountId = @BalanceAccountId END");
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
                    $"CREATE PROCEDURE [{TableName}_Delete] @BalanceAccountId int AS BEGIN SET NOCOUNT ON; " +
                    $"DELETE FROM {TableName} " +
                    $"WHERE BalanceAccountId = @BalanceAccountId " +
                    $"END");
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