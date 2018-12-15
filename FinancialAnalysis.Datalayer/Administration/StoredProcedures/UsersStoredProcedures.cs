using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.Administration
{
    public class UsersStoredProcedures : IStoredProcedures
    {
        public UsersStoredProcedures()
        {
            TableName = "Users";
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
            GetUserByNameAndPassword();
        }

        private void GetAllData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAll", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_GetAll] AS BEGIN SET NOCOUNT ON; " +
                                "SELECT UserId, " +
                                "Picture, " +
                                "Firstname, " +
                                "Lastname, " +
                                "Contraction, " +
                                "Mail, " +
                                "LoginUser, " +
                                "Password " +
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

        private void InsertData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_Insert", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_Insert] @Picture varbinary(MAX), @Firstname nvarchar(150), @Lastname nvarchar(150), @Contraction nvarchar(150), @Mail nvarchar(150), @LoginUser nvarchar(150), @Password nvarchar(150) AS BEGIN SET NOCOUNT ON; " +
                    $"INSERT into {TableName} (Picture, Firstname, Lastname, Contraction, Mail, LoginUser, Password ) " +
                    "VALUES (@Picture, @Firstname, @Lastname, @Contraction, @Mail, @LoginUser, @Password ); " +
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
                    $"CREATE PROCEDURE [{TableName}_GetById] @UserId int AS BEGIN SET NOCOUNT ON; SELECT UserId, Picture, Firstname, Lastname, Contraction, Mail, LoginUser, Password " +
                    $"FROM {TableName} " +
                    "WHERE UserId = @UserId END");
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

        private void GetUserByNameAndPassword()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetUserByNameAndPassword", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_GetUserByNameAndPassword] @LoginUser nvarchar(150), @Password nvarchar(150) AS BEGIN SET NOCOUNT ON; SELECT UserId, Picture, Firstname, Lastname, Contraction, Mail, LoginUser, Password " +
                    $"FROM {TableName} " +
                    "WHERE LoginUser = @LoginUser " +
                    "AND Password = @Password END");
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