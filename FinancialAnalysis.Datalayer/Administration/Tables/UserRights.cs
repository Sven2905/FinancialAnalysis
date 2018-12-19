using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models.Administration;
using Serilog;

namespace FinancialAnalysis.Datalayer.Administration
{
    public class UserRights : ITable
    {
        private readonly UserRightsStoredProcedures sp = new UserRightsStoredProcedures();

        public UserRights()
        {
            TableName = "UserRights";
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
                    "UserRightId int IDENTITY(1,1) PRIMARY KEY, " +
                    "Name nvarchar(150) NOT NULL, " +
                    "Description nvarchar(150) NOT NULL, " +
                    "Permission int NOT NULL)"; 

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
        ///     Returns all UserRight records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserRight> GetAll()
        {
            IEnumerable<UserRight> output = new List<UserRight>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<UserRight>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the UserRight item
        /// </summary>
        /// <param name="UserRight"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(UserRight UserRight)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result =
                        con.Query<int>(
                            $"dbo.{TableName}_Insert @Name, @Description, @Permission ",
                            UserRight);
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
        ///     Inserts the list of UserRight items
        /// </summary>
        /// <param name="UserRights"></param>
        public void Insert(IEnumerable<UserRight> UserRights)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var UserRight in UserRights) Insert(UserRight);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns UserRight by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserRight GetById(int id)
        {
            var output = new UserRight();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<UserRight>($"dbo.{TableName}_GetById @UserRightId",
                        new { UserRightId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update UserRight, if not exist, insert it
        /// </summary>
        /// <param name="UserRight"></param>
        public void UpdateOrInsert(UserRight UserRight)
        {
            if (UserRight.UserRightId == 0 || GetById(UserRight.UserRightId) is null)
            {
                Insert(UserRight);
                return;
            }

            Update(UserRight);
        }

        /// <summary>
        ///     Update UserRights, if not exist insert them
        /// </summary>
        /// <param name="UserRight"></param>
        public void UpdateOrInsert(IEnumerable<UserRight> UserRights)
        {
            foreach (var UserRight in UserRights) UpdateOrInsert(UserRight);
        }

        /// <summary>
        ///     Update UserRight
        /// </summary>
        /// <param name="UserRight"></param>
        public void Update(UserRight UserRight)
        {
            if (UserRight.UserRightId == 0 || GetById(UserRight.UserRightId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @UserRightId, @Name, @Description, @Permission", UserRight);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete UserRight by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @UserRightId", new { UserRightId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }
    }
}