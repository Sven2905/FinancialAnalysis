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
            UpdateData();
            DeleteData();
            UpdatePasswordData();
        }

        private void GetAllData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAll", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_GetAll] AS BEGIN SET NOCOUNT ON; " +
                                 $"SELECT u.UserId, u.Picture, u.Firstname, u.Lastname, u.Contraction, u.Mail, u.IsActive, u.IsAdministrator, u.LoginUser, u.Password, " +
                    $"m.UserRightUserMappingId, m.RefUserId, m.RefUserRightId, m.IsGranted, " +
                    $"r.UserRightId, r.Name, r.Description, r.ParentCategory, r.Permission " +
                    $"FROM {TableName} u " +
                    "LEFT JOIN UserRightUserMappings m ON u.UserId = m.RefUserId " +
                    "LEFT JOIN UserRights r ON m.RefUserRightId = r.UserRightId " +
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
                    $"CREATE PROCEDURE [{TableName}_Insert] @Picture varbinary(MAX), @Firstname nvarchar(150), @Lastname nvarchar(150), @Contraction nvarchar(150), @Mail nvarchar(150), @IsActive bit, @IsAdministrator bit, @LoginUser nvarchar(150), @Password nvarchar(150) AS BEGIN SET NOCOUNT ON; " +
                    $"INSERT into {TableName} (Picture, Firstname, Lastname, Contraction, Mail, IsActive, IsAdministrator, LoginUser, Password ) " +
                    "VALUES (@Picture, @Firstname, @Lastname, @Contraction, @Mail, @IsActive, @IsAdministrator, @LoginUser, @Password ); " +
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
                    $"CREATE PROCEDURE [{TableName}_GetById] @UserId int AS BEGIN SET NOCOUNT ON; " +
                     $"SELECT u.UserId, u.Picture, u.Firstname, u.Lastname, u.Contraction, u.Mail, u.IsActive, u.IsAdministrator, u.LoginUser, u.Password, " +
                    $"m.UserRightUserMappingId, m.RefUserId, m.RefUserRightId, m.IsGranted, " +
                    $"r.UserRightId, r.Name, r.Description, r.ParentCategory, r.Permission " +
                    $"FROM {TableName} u " +
                    "LEFT JOIN UserRightUserMappings m ON u.UserId = m.RefUserId " +
                    "LEFT JOIN UserRights r ON m.RefUserRightId = r.UserRightId " +
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
                    $"CREATE PROCEDURE [{TableName}_GetUserByNameAndPassword] @LoginUser nvarchar(150), @Password nvarchar(150) AS BEGIN SET NOCOUNT ON; " +
                    $"SELECT u.UserId, u.Picture, u.Firstname, u.Lastname, u.Contraction, u.Mail, u.IsActive, u.IsAdministrator, u.LoginUser, u.Password, " +
                    $"m.UserRightUserMappingId, m.RefUserId, m.RefUserRightId, m.IsGranted, " +
                    $"r.UserRightId, r.Name, r.Description, r.ParentCategory, r.Permission " +
                    $"FROM {TableName} u " +
                    "LEFT JOIN UserRightUserMappings m ON u.UserId = m.RefUserId " +
                    "LEFT JOIN UserRights r ON m.RefUserRightId = r.UserRightId " +
                    "WHERE u.LoginUser = @LoginUser " +
                    "AND u.Password = @Password END");
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
                    $"CREATE PROCEDURE [{TableName}_Update] @UserId int, @Picture varbinary(MAX), @Firstname nvarchar(150), @Lastname nvarchar(150), @Contraction nvarchar(150), @Mail nvarchar(150), @IsActive bit, @IsAdministrator bit, @LoginUser nvarchar(150) " +
                    "AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} " +
                    "SET Picture = @Picture, " +
                    "Firstname = @Firstname, " +
                    "Lastname = @Lastname, " +
                    "Contraction = @Contraction, " +
                    "Mail = @Mail, " +
                    "IsActive = @IsActive, " +
                    "IsAdministrator = @IsAdministrator, " +
                    "LoginUser = @LoginUser " +
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

        private void UpdatePasswordData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_UpdatePassword", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_UpdatePassword] @UserId int, @Password nvarchar(150) " +
                    "AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} " +
                    "SET Password = @Password " +
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

        private void DeleteData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_Delete", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_Delete] @UserId int AS BEGIN SET NOCOUNT ON; DELETE FROM {TableName} WHERE UserId = @UserId END");
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