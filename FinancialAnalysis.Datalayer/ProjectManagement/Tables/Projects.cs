using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.ProjectManagement;
using Serilog;

namespace FinancialAnalysis.Datalayer.ProjectManagement
{
    public class Projects : ITable
    {
        private readonly ProjectsStoredProcedures sp = new ProjectsStoredProcedures();

        public Projects()
        {
            TableName = "Projects";
            CheckAndCreateTable();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs\\Tables.txt", rollingInterval: RollingInterval.Month)
                .CreateLogger();
        }

        public string TableName { get; }

        public void CheckAndCreateTable()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"If not exists (select name from sysobjects where name = '{TableName}') CREATE TABLE {TableName}(" +
                    "ProjectId int IDENTITY(1,1) PRIMARY KEY," +
                    "Name nvarchar(150) NOT NULL," +
                    "Description nvarchar(MAX)," +
                    "StartDate datetime NOT NULL," +
                    "ExpectedEndDate datetime NOT NULL," +
                    "TotalEndDate datetime NOT NULL," +
                    "IsEnded bit," +
                    "Budget money," +
                    "RefCostCenterId int NOT NULL," +
                    "Identifier nvarchar(150) NOT NULL, " +
                    "RefEmployeeId int)";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating table '{TableName}'", e);
            }
        }

        public void CheckAndCreateStoredProcedures()
        {
            sp.CheckAndCreateProcedures();
        }

        /// <summary>
        ///     Returns all Project records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Project> GetAll()
        {
            var projectDictionary = new Dictionary<int, Project>();

            using (var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                var query = con.Query<Project, ProjectEmployeeMapping, Employee, CostCenter, ProjectRole, ProjectWorkingTime, Project>
                    ($"dbo.{TableName}_GetAll",
                        (p, m, e, c, r, t) =>
                        {
                            Project projectEntry = new Project();

                            if (!projectDictionary.TryGetValue(p.ProjectId, out projectEntry))
                            {
                                projectEntry = p;
                                projectEntry.CostCenter = c;
                                if (m != null)
                                {
                                    m.ProjectRole = r;
                                    m.Project = p;
                                    m.Employee = e;
                                }
                                projectEntry.ProjectEmployeeMappings = new List<ProjectEmployeeMapping>();
                                projectEntry.ProjectWorkingTimes = new List<ProjectWorkingTime>();
                                projectDictionary.Add(projectEntry.ProjectId, projectEntry);
                            }

                            projectEntry.ProjectEmployeeMappings.Add(m);
                            projectEntry.ProjectWorkingTimes.Add(t);
                            return p;

                        }, splitOn: "ProjectId, ProjectEmployeeMappingId, EmployeeId, CostCenterId, ProjectRoleId, ProjectWorkingTimeId")
                    .AsQueryable();
                return query.ToList();
            }
        }

        /// <summary>
        ///     Inserts the Project item
        /// </summary>
        /// <param name="Project"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(Project Project)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result =
                        con.Query<int>(
                            $"dbo.{TableName}_Insert @Name, @Description, @Budget, @StartDate, @ExpectedEndDate, @TotalEndDate, @IsEnded, @RefCostCenterId, @Identifier, @RefEmployeeId",
                            Project);
                    id = result.Single();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }

            return id;
        }

        /// <summary>
        ///     Inserts the list of Project items
        /// </summary>
        /// <param name="creditor"></param>
        public void Insert(IEnumerable<Project> Projects)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var Project in Projects) Insert(Project);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns Project by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Project GetById(int id)
        {
            var output = new Project();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<Project>($"dbo.{TableName}_GetById @ProjectId",
                        new { ProjectId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        public void AddReferences()
        {
            AddCostCentersReference();
        }

        private void AddCostCentersReference()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_CostCenter', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_CostCenter FOREIGN KEY(RefCostCenterId) REFERENCES CostCenters(CostCenterId)";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and CostCenters", e);
            }
        }

        /// <summary>
        ///     Update Project, if not exist, insert it
        /// </summary>
        /// <param name="User"></param>
        public void UpdateOrInsert(Project Project)
        {
            if (Project.ProjectId == 0 || GetById(Project.ProjectId) is null)
            {
                Insert(Project);
                return;
            }

            Update(Project);
        }

        /// <summary>
        ///     Update Projects, if not exist insert them
        /// </summary>
        /// <param name="Project"></param>
        public void UpdateOrInsert(IEnumerable<Project> Projects)
        {
            foreach (var Project in Projects) UpdateOrInsert(Project);
        }

        /// <summary>
        ///     Update Project
        /// </summary>
        /// <param name="Project"></param>
        public void Update(Project Project)
        {
            if (Project.ProjectId == 0 || GetById(Project.ProjectId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @ProjectId, @Name, @Description, @Budget, @StartDate, @ExpectedEndDate, @TotalEndDate, @IsEnded, @RefCostCenterId, @Identifier, @RefEmployeeId", Project);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete User by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @ProjectId", new { ProjectId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }
    }
}