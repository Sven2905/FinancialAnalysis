using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models.ProjectManagement;
using Serilog;

namespace FinancialAnalysis.Datalayer.ProjectManagement
{
    public class ProjectEmployeeMappings : ITable
    {
        private readonly ProjectsStoredProcedures sp = new ProjectsStoredProcedures();

        public ProjectEmployeeMappings()
        {
            TableName = "ProjectEmployeeMappings";
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
                    "ProjectEmployeeMappingId int IDENTITY(1,1) PRIMARY KEY," +
                    "RefEmployeeId int NOT NULL," +
                    "RefProjectId int NOT NULL," +
                    "RefProjectRoleId int NOT NULL)";

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
        ///     Returns all ProjectEmployeeMapping records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProjectEmployeeMapping> GetAll()
        {
            IEnumerable<ProjectEmployeeMapping> output = new List<ProjectEmployeeMapping>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<ProjectEmployeeMapping>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the ProjectEmployeeMapping item
        /// </summary>
        /// <param name="ProjectEmployeeMapping"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(ProjectEmployeeMapping ProjectEmployeeMapping)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result =
                        con.Query<int>($"dbo.{TableName}_Insert @RefEmployeeId, @RefProjectId, @RefProjectRoleId",
                            ProjectEmployeeMapping);
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
        ///     Inserts the list of ProjectEmployeeMapping items
        /// </summary>
        /// <param name="ProjectEmployeeMapping"></param>
        public void Insert(IEnumerable<ProjectEmployeeMapping> ProjectEmployeeMappings)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var ProjectEmployeeMapping in ProjectEmployeeMappings) Insert(ProjectEmployeeMapping);
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
        public ProjectEmployeeMapping GetById(int id)
        {
            var output = new ProjectEmployeeMapping();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<ProjectEmployeeMapping>(
                        $"dbo.{TableName}_GetById @ProjectEmployeeMappingId", new {ProjectEmployeeMappingId = id});
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
            AddEmployeesReference();
            AddProjectsReference();
            AddProjectRolesReference();
        }

        private void AddEmployeesReference()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_Employees', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_Employees FOREIGN KEY(RefEmployeeId) REFERENCES Employees(EmployeeId)";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and Employees", e);
            }
        }

        private void AddProjectsReference()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_Projects', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_Projects FOREIGN KEY(RefProjectId) REFERENCES Projects(ProjectId)";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and Projects", e);
            }
        }

        private void AddProjectRolesReference()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_Employees', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_ProjectRoles FOREIGN KEY(RefProjectRoleId) REFERENCES ProjectRoles(ProjectRoleId)";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and ProjectRoles", e);
            }
        }
    }
}