using Dapper;
using FinancialAnalysis.Models.ProjectManagement;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FinancialAnalysis.Datalayer.ProjectManagement
{
    public class ProjectRoles : ITable
    {
        public string TableName { get; }
        private HealthInsurancesStoredProcedures sp = new HealthInsurancesStoredProcedures();

        public ProjectRoles()
        {
            TableName = "ProjectRoles";
            CheckAndCreateTable();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs\\Tables.txt", rollingInterval: RollingInterval.Month)
                .CreateLogger();
        }

        public void CheckAndCreateTable()
        {
            try
            {
                SqlConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr = $"If not exists (select name from sysobjects where name = '{TableName}') CREATE TABLE {TableName}(" +
                                 $"ProjectRoleId int IDENTITY(1,1) PRIMARY KEY," +
                                 $"Name nvarchar(150))";

                using (SqlCommand command = new SqlCommand(commandStr, con))
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
        /// Returns all ProjectRole records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProjectRole> GetAll()
        {
            IEnumerable<ProjectRole> output = new List<ProjectRole>();
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<ProjectRole>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }
            return output;
        }

        /// <summary>
        /// Inserts the ProjectRole item
        /// </summary>
        /// <param name="ProjectRole"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(ProjectRole ProjectRole)
        {
            int id = 0;
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Name", ProjectRole);
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
        /// Inserts the list of ProjectRole items
        /// </summary>
        /// <param name="ProjectRole"></param>
        public void Insert(IEnumerable<ProjectRole> ProjectRoles)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var ProjectRole in ProjectRoles)
                    {
                        Insert(ProjectRole);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        /// Returns ProjectRole by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProjectRole GetById(int id)
        {
            ProjectRole output = new ProjectRole();
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<ProjectRole>($"dbo.{TableName}_GetById @ProjectRoleId", new { ProjectRoleId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }
            return output;
        }
    }
}