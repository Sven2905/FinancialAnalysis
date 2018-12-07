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
        }

        private void GetAllData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAll", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_GetAll] AS BEGIN SET NOCOUNT ON; " +
                                "SELECT ProjectId, Name, Description, Budget, StartDate, ExpectedEndDate, TotalEndDate, IsEnded, RefCostCenterId, RefCustomerId " +
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
                    $"CREATE PROCEDURE [{TableName}_Insert] @Name nvarchar(150), @Description nvarchar(150), @Budget money, @StartDate datetime, @ExpectedEndDate datetime, @TotalEndDate datetime, @IsEnded bit, @RefCostCenterId int, @RefCustomerId int AS BEGIN SET NOCOUNT ON; " +
                    $"INSERT into {TableName} (Name, Description, Budget, StartDate, ExpectedEndDate, TotalEndDate, IsEnded, RefCostCenterId, RefCustomerId) " +
                    "VALUES (@Name, @Description, @Budget, @StartDate, @ExpectedEndDate, @TotalEndDate, @IsEnded, @RefCostCenterId, @RefCustomerId); " +
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
                    $"CREATE PROCEDURE [{TableName}_GetById] @ProjectId int AS BEGIN SET NOCOUNT ON; SELECT ProjectId, Name, Description, Budget, StartDate, ExpectedEndDate, TotalEndDate, IsEnded, RefCostCenterId, RefCustomerId " +
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
    }
}