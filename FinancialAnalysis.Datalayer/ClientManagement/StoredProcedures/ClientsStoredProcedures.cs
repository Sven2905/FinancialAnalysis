using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.ClientManagement
{
    internal class ClientsStoredProcedures : IStoredProcedures
    {
        public ClientsStoredProcedures()
        {
            TableName = "Clients";
        }

        public string TableName { get; }

        public void CheckAndCreateProcedures()
        {
            InsertData();
            GetAllData();
            GetById();
            UpdateData();
            DeleteData();
            IsClientInUse();
        }

        private void GetAllData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAll", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_GetAll] AS BEGIN SET NOCOUNT ON; " +
                                "SELECT ClientId, Name, Street, Postcode, City, Phone, Fax, Mail, IBAN, BIC, BankName, FederalState " +
                                $"FROM {TableName} " +
                                $"WHERE ClientId > 1" +
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
                                "@Name nvarchar(50), @Street nvarchar(50), @Postcode int, @City nvarchar(50), @Phone nvarchar(50), @Fax nvarchar(50), @Mail nvarchar(50), @IBAN nvarchar(50), @BIC nvarchar(50), @BankName nvarchar(50), @FederalState int " +
                                $"AS BEGIN SET NOCOUNT ON; INSERT into {TableName} (Name, Street, Postcode, City, Phone, Fax, Mail, IBAN, BIC, BankName, FederalState) " +
                                "VALUES (@Name, @Street, @Postcode, @City, @Phone, @Fax, @Mail, @IBAN, @BIC, @BankName, @FederalState); " +
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
                    $"CREATE PROCEDURE [{TableName}_GetById] @ClientId int AS BEGIN SET NOCOUNT ON; SELECT * FROM {TableName} WHERE ClientId = @ClientId END");
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
                    $"CREATE PROCEDURE [{TableName}_Update] @ClientId int, @Name nvarchar(50), @Street nvarchar(50), @Postcode int, @City nvarchar(50), @Phone nvarchar(50), @Fax nvarchar(50), @Mail nvarchar(50), @IBAN nvarchar(50), @BIC nvarchar(50), @BankName nvarchar(50), @FederalState int " +
                    "AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} SET " +
                    "Name = @Name, " +
                    "Street = @Street, " +
                    "Postcode = @Postcode, " +
                    "City = @City, " +
                    "Phone = @Phone, " +
                    "Fax = @Fax, " +
                    "Mail = @Mail, " +
                    "IBAN = @IBAN, " +
                    "BIC = @BIC, " +
                    "BankName = @BankName, " +
                    "FederalState = @FederalState " +
                    "WHERE ClientId = @ClientId END");
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
                    $"CREATE PROCEDURE [{TableName}_Delete] @ClientId int AS BEGIN SET NOCOUNT ON; " +
                    $"DELETE FROM {TableName} WHERE ClientId = @ClientId END");
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

        private void IsClientInUse()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_IsClientInUse", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_IsClientInUse] @ClientId int AS " +
                    "SELECT CASE WHEN EXISTS ( " +
                    $"SELECT * FROM {TableName} " +
                    $"RIGHT JOIN Creditors ON {TableName}.ClientId = Creditors.RefClientId " +
                    $"RIGHT JOIN Debitors ON {TableName}.ClientId = Debitors.RefClientId " +
                    "WHERE ClientId = @ClientId) " +
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