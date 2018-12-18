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
                                "SELECT p.ProjectId, p.Name, p.Description, p.Budget, p.StartDate, p.ExpectedEndDate, p.TotalEndDate, p.IsEnded, p.RefCostCenterId, p.Identifier, p.RefEmployeeId, " +
                                "m.ProjectEmployeeMappingId, m.RefEmployeeId as ProjectEmployeeMappings_RefEmployeeId, m.RefProjectId as ProjectEmployeeMappings_RefProjectId, m.RefProjectRoleId, " +
                                "e.EmployeeId, e.Firstname, e.Lastname, e.Birthdate, e.Street, e.City, e.Postcode, e.Gender, e.CivilStatus, e.RefTariffId, e.TaxId, e.RefHealthInsuranceId, e.HasDrivingLicence, e.Nationality, e.Confession, e.BankName, e.BIC, e.IBAN, e.NationalInsuranceNumber, e.Salary, e.WorkHoursPerWeek, e.VacationDays, e.Picture, e.Mail, e.Phone, " +
                                "c.CostCenterId, c.Name as CostCenters_name, c.Identifier as CostCenters_Identifier, c.Description as CostCenters_Description, " +
                                "r.ProjectRoleId, r.Name as ProjectRoles_Name, " +
                                "t.ProjectWorkingTimeId, t.Description as ProjectWorkingTimes_Description, t.StartTime, t.EndTime, t.Breaktime, t.RefEmployeeId as ProjectWorkingTimes_RefEmployeeId, t.RefProjectId as ProjectWorkingTimes_RefProjectId " +
                                $"FROM {TableName} as p " +
                                "LEFT JOIN ProjectEmployeeMappings m ON p.ProjectId = m.RefProjectId " +
                                "LEFT JOIN Employees e ON m.RefEmployeeId = e.EmployeeId " +
                                "LEFT JOIN CostCenters c ON p.RefCostCenterId = c.CostCenterId " +
                                "LEFT JOIN ProjectWorkingTimes t ON p.ProjectId = t.RefProjectId " +
                                "LEFT JOIN ProjectRoles r ON m.RefProjectRoleId = r.ProjectRoleId " +
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
                    $"CREATE PROCEDURE [{TableName}_Insert] @Name nvarchar(150), @Description nvarchar(MAX), @Budget money, @StartDate datetime, @ExpectedEndDate datetime, @TotalEndDate datetime, @IsEnded bit, @RefCostCenterId int, @Identifier nvarchar(150), @RefEmployeeId int AS BEGIN SET NOCOUNT ON; " +
                    $"INSERT into {TableName} (Name, Description, Budget, StartDate, ExpectedEndDate, TotalEndDate, IsEnded, RefCostCenterId, Identifier, RefEmployeeId) " +
                    "VALUES (@Name, @Description, @Budget, @StartDate, @ExpectedEndDate, @TotalEndDate, @IsEnded, @RefCostCenterId, @Identifier, @RefEmployeeId); " +
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
                    $"ExpectedEndDate, TotalEndDate, IsEnded, RefCostCenterId, Identifier, RefEmployeeId " +
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
                    $"CREATE PROCEDURE [{TableName}_Update] @ProjectId int, @Name nvarchar(150), @Description nvarchar(MAX), @Budget money, @StartDate datetime, @ExpectedEndDate datetime, @TotalEndDate datetime, @IsEnded bit, @RefCostCenterId int, @Identifier nvarchar(150), @RefEmployeeId int " +
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
                    "IsEnded = @IsEnded, " +
                    "Identifier = @Identifier " +
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