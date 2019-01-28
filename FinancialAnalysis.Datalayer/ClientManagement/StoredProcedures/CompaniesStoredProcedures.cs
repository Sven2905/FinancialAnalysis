using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.ClientManagement
{
    internal class CompaniesStoredProcedures : IStoredProcedures
    {
        public CompaniesStoredProcedures()
        {
            TableName = "Companies";
        }

        public string TableName { get; }

        public void CheckAndCreateProcedures()
        {
            InsertData();
            GetAllData();
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
                                "SELECT CompanyId, ContactPerson, UStID, TaxNumber, Website, CEO, Logo " +
                                $"FROM {TableName} " +
                                $"WHERE CompanyId > 1" +
                                "ORDER BY Name END");
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

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_Insert] " +
                                "@ContactPerson nvarchar(50), @UStID nvarchar(50), @TaxNumber nvarchar(50), @Website nvarchar(50), @CEO nvarchar(150), @Logo varbinary(MAX) " +
                                $"AS BEGIN SET NOCOUNT ON; INSERT into {TableName} (ContactPerson, UStID, TaxNumber, Website, CEO, Logo) " +
                                "VALUES (@ContactPerson, @UStID, @TaxNumber, @Website, @CEO, @Logo); " +
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
                    $"CREATE PROCEDURE [{TableName}_GetById] @CompanyId int AS BEGIN SET NOCOUNT ON; SELECT * FROM {TableName} WHERE CompanyId = @CompanyId END");
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
                    $"CREATE PROCEDURE [{TableName}_Update] @CompanyId int, @ContactPerson nvarchar(50), @UStID nvarchar(50), @TaxNumber nvarchar(50), @Website nvarchar(50), @CEO nvarchar(150), @Logo varbinary(MAX)  " +
                    "AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} SET " +
                    "ContactPerson = @ContactPerson, " +
                    "UStID = @UStID, " +
                    "TaxNumber = @TaxNumber, " +
                    "Website = @Website, " +
                    "CEO = @CEO, " +
                    "Logo = @Logo " +
                    "WHERE CompanyId = @CompanyId END");
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
                    $"CREATE PROCEDURE [{TableName}_Delete] @CompanyId int AS BEGIN SET NOCOUNT ON; " +
                    $"DELETE FROM {TableName} WHERE CompanyId = @CompanyId END");
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