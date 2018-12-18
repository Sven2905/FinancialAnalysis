using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.ProjectManagement
{
    public class ProjectWorkingTimesStoredProcedures : IStoredProcedures
    {
        public ProjectWorkingTimesStoredProcedures()
        {
            TableName = "ProjectWorkingTimes";
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
                                "SELECT ProjectWorkingTimeId, Description, StartTime, EndTime, Breaktime, RefEmployeeId, RefProjectId " +
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
                    $"CREATE PROCEDURE [{TableName}_Insert] @Description nvarchar(MAX), @StartTime datetime, @EndTime datetime, @Breaktime int, @RefEmployeeId int, @RefProjectId int AS BEGIN SET NOCOUNT ON; " +
                    $"INSERT into {TableName} (Description, StartTime, EndTime, Breaktime, RefEmployeeId, RefProjectId) " +
                    "VALUES (@Description, @StartTime, @EndTime, @Breaktime, @RefEmployeeId, @RefProjectId); " +
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
                    $"CREATE PROCEDURE [{TableName}_GetById] @ProjectWorkingTimeId int AS BEGIN SET NOCOUNT ON; SELECT ProjectWorkingTimeId, Description, StartTime, EndTime, Breaktime, RefEmployeeId, RefProjectId " +
                    $"FROM {TableName} " +
                    "WHERE ProjectWorkingTimeId = @ProjectWorkingTimeId END");
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
                    $"CREATE PROCEDURE [{TableName}_Update] @ProjectWorkingTimeId int, @Description nvarchar(MAX), @StartTime datetime, @EndTime datetime, @Breaktime int, @RefEmployeeId int, @RefProjectId int " +
                    "AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} " +
                    "SET Description = @Description, " +
                    "StartTime = @StartTime, " +
                    "EndTime = @EndTime, " +
                    "Breaktime = @Breaktime, " +
                    "RefEmployeeId = @RefEmployeeId, " +
                    "RefProjectId = @RefProjectId " +
                    "WHERE ProjectWorkingTimeId = @ProjectWorkingTimeId END");
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
                    $"CREATE PROCEDURE [{TableName}_Delete] @ProjectWorkingTimeId int AS BEGIN SET NOCOUNT ON; DELETE FROM {TableName} WHERE ProjectWorkingTimeId = @ProjectWorkingTimeId END");
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