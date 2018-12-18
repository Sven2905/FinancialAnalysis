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
    public class ProjectWorkingTimes : ITable
    {
        private readonly ProjectWorkingTimesStoredProcedures sp = new ProjectWorkingTimesStoredProcedures();

        public ProjectWorkingTimes()
        {
            TableName = "ProjectWorkingTimes";
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
                    "ProjectWorkingTimeId int IDENTITY(1,1) PRIMARY KEY," +
                    "Description nvarchar(MAX)," +
                    "StartTime datetime NOT NULL," +
                    "EndTime datetime NOT NULL," +
                    "Breaktime int NOT NULL," +
                    "RefEmployeeId int NOT NULL," +
                    "RefProjectId int NOT NULL)";

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
        ///     Returns all ProjectWorkingTime records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProjectWorkingTime> GetAll()
        {
            IEnumerable<ProjectWorkingTime> output = new List<ProjectWorkingTime>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<ProjectWorkingTime>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the ProjectWorkingTime item
        /// </summary>
        /// <param name="ProjectWorkingTime"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(ProjectWorkingTime ProjectWorkingTime)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result =
                        con.Query<int>(
                            $"dbo.{TableName}_Insert @Description, @StartTime, @EndTime, @Breaktime, @RefEmployeeId, @RefProjectId ",
                            ProjectWorkingTime);
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
        ///     Inserts the list of ProjectWorkingTime items
        /// </summary>
        /// <param name="ProjectWorkingTime"></param>
        public void Insert(IEnumerable<ProjectWorkingTime> ProjectWorkingTimes)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var ProjectWorkingTime in ProjectWorkingTimes) Insert(ProjectWorkingTime);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns ProjectWorkingTime by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProjectWorkingTime GetById(int id)
        {
            var output = new ProjectWorkingTime();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<ProjectWorkingTime>($"dbo.{TableName}_GetById @ProjectWorkingTimeId",
                        new { ProjectWorkingTimeId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update ProjectWorkingTime, if not exist, insert it
        /// </summary>
        /// <param name="ProjectWorkingTime"></param>
        public void UpdateOrInsert(ProjectWorkingTime ProjectWorkingTime)
        {
            if (ProjectWorkingTime.ProjectWorkingTimeId == 0 || GetById(ProjectWorkingTime.ProjectWorkingTimeId) is null)
            {
                Insert(ProjectWorkingTime);
                return;
            }

            Update(ProjectWorkingTime);
        }

        /// <summary>
        ///     Update ProjectWorkingTimes, if not exist insert them
        /// </summary>
        /// <param name="ProjectWorkingTimes"></param>
        public void UpdateOrInsert(IEnumerable<ProjectWorkingTime> ProjectWorkingTimes)
        {
            foreach (var ProjectWorkingTime in ProjectWorkingTimes) UpdateOrInsert(ProjectWorkingTime);
        }

        /// <summary>
        ///     Update ProjectWorkingTime
        /// </summary>
        /// <param name="ProjectWorkingTime"></param>
        public void Update(ProjectWorkingTime ProjectWorkingTime)
        {
            if (ProjectWorkingTime.ProjectWorkingTimeId == 0 || GetById(ProjectWorkingTime.ProjectWorkingTimeId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @ProjectWorkingTimeId, @Description, @StartTime, @EndTime, @Breaktime, @RefEmployeeId, @RefProjectId", ProjectWorkingTime);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete ProjectWorkingTime by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @ProjectWorkingTimeId", new { ProjectWorkingTimeId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        public void AddReferences()
        {
            AddEmployeesReference();
            AddProjectsReference();
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
    }
}