using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.ProjectManagement
{
    public class ProjectsStoredProcedures : IStoredProcedures
    {
        public ProjectsStoredProcedures()
        {
            TableName = "Projects";
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
        }

        private void GetAllData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAll", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_GetAll] AS BEGIN SET NOCOUNT ON; " +
                                "SELECT ProjectId, Name, Description, Budget, StartDate, ExpectedEndDate, TotalEndDate, IsEnded, RefCostCenterId, RefEmployeeId " +
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
                    $"CREATE PROCEDURE [{TableName}_Insert] @Name nvarchar(150), @Description nvarchar(MAX), @Budget money, @StartDate datetime, @ExpectedEndDate datetime, @TotalEndDate datetime, @IsEnded bit, @RefCostCenterId int, @RefEmployeeId int AS BEGIN SET NOCOUNT ON; " +
                    $"INSERT into {TableName} (Name, Description, Budget, StartDate, ExpectedEndDate, TotalEndDate, IsEnded, RefCostCenterId, RefEmployeeId) " +
                    "VALUES (@Name, @Description, @Budget, @StartDate, @ExpectedEndDate, @TotalEndDate, @IsEnded, @RefCostCenterId, @RefEmployeeId); " +
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
                    $"CREATE PROCEDURE [{TableName}_GetById] @ProjectId int AS BEGIN SET NOCOUNT ON; SELECT ProjectId, Name, Description, Budget, StartDate, " +
                    $"ExpectedEndDate, TotalEndDate, IsEnded, RefCostCenterId, RefEmployeeId " +
                    $"FROM {TableName} " +
                    "WHERE ProjectId = @ProjectId END");
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
                    $"CREATE PROCEDURE [{TableName}_Update] @ProjectId int, @Name nvarchar(150), @Description nvarchar(MAX), @Budget money, @StartDate datetime, @ExpectedEndDate datetime, @TotalEndDate datetime, @IsEnded bit, @RefCostCenterId int, @RefEmployeeId int " +
                    "AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} " +
                    "SET Name = @Name, " +
                    "Description = @Description, " +
                    "Budget = @Budget, " +
                    "StartDate = @StartDate, " +
                    "ExpectedEndDate = @ExpectedEndDate, " +
                    "TotalEndDate = @TotalEndDate, " +
                    "RefCostCenterId = @RefCostCenterId, " +
                    "RefEmployeeId = @RefEmployeeId, " +
                    "IsEnded = @IsEnded " +
                    "WHERE ProjectId = @ProjectId END");
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
                    $"CREATE PROCEDURE [{TableName}_Delete] @ProjectId int AS BEGIN SET NOCOUNT ON; DELETE FROM {TableName} WHERE ProjectId = @ProjectId END");
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