using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.Accounting
{
    internal class DebitorsStoredProcedures : IStoredProcedures
    {
        public DebitorsStoredProcedures()
        {
            TableName = "Debitors";
        }

        public string TableName { get; }

        /// <summary>
        ///     Check if all Stored Procedures are created, otherwise create them
        /// </summary>
        public void CheckAndCreateProcedures()
        {
            InsertData();
            GetAllData();
            GetById();
            UpdateData();
            DeleteData();
            IsDebitorInUse();
        }

        private void GetAllData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAll", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_GetAll] AS BEGIN SET NOCOUNT ON; " +
                                "SELECT d.DebitorId, d.RefCompanyId, d.RefCostAccountId, " +
                                "co.CompanyId, co.Name, co.Street, co.Postcode, co.City, co.ContactPerson, co.UStID, co.TaxNumber, co.Phone, co.Fax, co.eMail, co.Website, co.IBAN, co.BIC, co.BankName, co.FederalState, " +
                                $"a.CostAccountId, a.Description, a.AccountNumber, a.RefTaxTypeId, a.RefCostAccountCategoryId, a.IsVisible FROM {TableName} d " +
                                "JOIN Companies co ON d.RefCompanyId = co.CompanyId " +
                                "JOIN CostAccounts a ON d.RefCostAccountId = a.CostAccountId " +
                                "ORDER BY a.AccountNumber END");
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
                    $"CREATE PROCEDURE [{TableName}_Insert] @RefCompanyId int, @RefCostAccountId int AS BEGIN SET NOCOUNT ON; " +
                    $"INSERT into {TableName} (RefCompanyId, RefCostAccountId) " +
                    "VALUES (@RefCompanyId, @RefCostAccountId); " +
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
                    $"CREATE PROCEDURE [{TableName}_GetById] @DebitorId int AS BEGIN SET NOCOUNT ON; SELECT DebitorId, RefCompanyId, RefCostAccountId " +
                    $"FROM {TableName} " +
                    "WHERE DebitorId = @DebitorId END");
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
                    $"CREATE PROCEDURE [{TableName}_Update] @DebitorId int, @RefCompanyId int, @RefCostAccountId int " +
                    "AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} " +
                    "SET RefCompanyId = @RefCompanyId, RefCostAccountId = @RefCostAccountId " +
                    "WHERE DebitorId = @DebitorId END");
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
                    $"CREATE PROCEDURE [{TableName}_Delete] @DebitorId int AS BEGIN SET NOCOUNT ON; DELETE FROM {TableName} WHERE DebitorId = @DebitorId END");
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

        private void IsDebitorInUse()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_IsDebitorInUse", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_IsDebitorInUse] @DebitorId int AS " +
                    "SELECT CASE WHEN EXISTS ( " +
                    $"SELECT * FROM {TableName} " +
                    $"RIGHT JOIN CostAccounts ON {TableName}.RefCostAccountId = CostAccounts.CostAccountId " +
                    "WHERE DebitorId = @DebitorId) " +
                    "THEN CAST(1 AS BIT) " +
                    "ELSE CAST(0 AS BIT) END");
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