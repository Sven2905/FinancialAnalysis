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
    public class UserRightUserMappings : ITable
    {
        private readonly UserRightUserMappingsStoredProcedures sp = new UserRightUserMappingsStoredProcedures();

        public UserRightUserMappings()
        {
            TableName = "UserRightUserMappings";
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
                    "UserRightUserMappingId int IDENTITY(1,1) PRIMARY KEY, " +
                    "IsGranted bit, " +
                    "RefUserId int NOT NULL, " +
                    "RefUserRightId int NOT NULL)";

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
        ///     Returns all UserRightUserMapping records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserRightUserMapping> GetAll()
        {
            IEnumerable<UserRightUserMapping> output = new List<UserRightUserMapping>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<UserRightUserMapping>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the UserRightUserMapping item
        /// </summary>
        /// <param name="UserRightUserMapping"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(UserRightUserMapping UserRightUserMapping)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result =
                        con.Query<int>(
                            $"dbo.{TableName}_Insert @IsGranted, @RefUserId, @RefUserRightId ",
                            UserRightUserMapping);
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
        ///     Inserts the list of UserRightUserMapping items
        /// </summary>
        /// <param name="UserRightUserMapping"></param>
        private void Insert(IEnumerable<UserRightUserMapping> UserRightUserMappings)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var UserRightUserMapping in UserRightUserMappings) Insert(UserRightUserMapping);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns UserRightUserMapping by UserRightId and UserId
        /// </summary>
        /// <param name="RefUserRightId"></param>
        /// <param name="RefUserId"></param>
        /// <returns></returns>
        public UserRightUserMapping GetByIds(int RefUserRightId, int RefUserId)
        {
            var output = new UserRightUserMapping();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<UserRightUserMapping>(
                        $"dbo.{TableName}_GetByIds @RefUserRightId, @RefUserId",
                        new {RefUserRightId, RefUserId});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetByIds' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update UserRightUserMapping, if not exist, insert it
        /// </summary>
        /// <param name="UserRight"></param>
        public UserRightUserMapping UpdateOrInsert(UserRightUserMapping UserRightUserMapping)
        {
            if (UserRightUserMapping.UserRightUserMappingId != 0) UserRightUserMapping = Update(UserRightUserMapping);

            var temp = GetByIds(UserRightUserMapping.RefUserRightId, UserRightUserMapping.RefUserId);
            if (temp is null)
            {
                UserRightUserMapping.UserRightUserMappingId = Insert(UserRightUserMapping);
                return UserRightUserMapping;
            }

            UserRightUserMapping = Update(temp);
            return UserRightUserMapping;
        }

        /// <summary>
        ///     Update UserRightUserMappings, if not exist insert them
        /// </summary>
        /// <param name="UserRightUserMappings"></param>
        public List<UserRightUserMapping> UpdateOrInsert(List<UserRightUserMapping> UserRightUserMappings)
        {
            for (var i = 0; i < UserRightUserMappings.Count; i++)
                UserRightUserMappings[i] = UpdateOrInsert(UserRightUserMappings[i]);
            return UserRightUserMappings;
        }

        /// <summary>
        ///     Update UserRightUserMapping
        /// </summary>
        /// <param name="UserRightUserMapping"></param>
        private UserRightUserMapping Update(UserRightUserMapping UserRightUserMapping)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute(
                        $"dbo.{TableName}_Update @UserRightUserMappingId, @IsGranted, @RefUserId, @RefUserRightId",
                        UserRightUserMapping);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }

            return UserRightUserMapping;
        }

        /// <summary>
        ///     Delete UserRightUserMapping by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int UserId)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @RefUserId", new {RefUserId = UserId});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        public void AddReferences()
        {
            AddUsersReference();
            AddUserRightsReference();
        }

        private void AddUsersReference()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_Users', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_Users FOREIGN KEY(RefUserId) REFERENCES Users(UserId)";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and Users", e);
            }
        }

        private void AddUserRightsReference()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_UserRights', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_UserRights FOREIGN KEY(RefUserRightId) REFERENCES UserRights(UserRightId)";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and UserRights", e);
            }
        }
    }
}