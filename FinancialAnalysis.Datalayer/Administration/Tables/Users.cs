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
    public class Users : ITable
    {
        private readonly UsersStoredProcedures sp = new UsersStoredProcedures();

        public Users()
        {
            TableName = "Users";
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
                    "UserId int IDENTITY(1,1) PRIMARY KEY, " +
                    "Picture varbinary(MAX), " +
                    "Firstname nvarchar(150) NOT NULL, " +
                    "Lastname nvarchar(150) NOT NULL, " +
                    "Contraction nvarchar(150) NOT NULL, " +
                    "Mail nvarchar(150), " +
                    "IsActive bit, " +
                    "LoginUser nvarchar(150) NOT NULL, " +
                    "Password nvarchar(150) NOT NULL)"; 

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
        ///     Returns all User records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetAll()
        {
            IEnumerable<User> output = new List<User>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<User>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the User item
        /// </summary>
        /// <param name="User"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(User User)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result =
                        con.Query<int>(
                            $"dbo.{TableName}_Insert @Picture,@Firstname, @Lastname, @Contraction, @Mail, @IsActive, @LoginUser, @Password ",
                            User);
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
        ///     Inserts the list of User items
        /// </summary>
        /// <param name="ProductPrototype"></param>
        public void Insert(IEnumerable<User> Users)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var User in Users) Insert(User);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns User by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetById(int id)
        {
            var output = new User();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<User>($"dbo.{TableName}_GetById @UserId",
                        new { UserId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Returns User by Name and Password
        /// </summary>
        public User GetUserByNameAndPassword(string LoginUser, string Password)
        {
            var output = new User();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<User>($"dbo.{TableName}_GetUserByNameAndPassword @LoginUser, @Password",
                        new {  LoginUser, Password });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update User, if not exist, insert it
        /// </summary>
        /// <param name="User"></param>
        public void UpdateOrInsert(User User)
        {
            if (User.UserId == 0 || GetById(User.UserId) is null)
            {
                Insert(User);
                return;
            }

            Update(User);
        }

        /// <summary>
        ///     Update Users, if not exist insert them
        /// </summary>
        /// <param name="User"></param>
        public void UpdateOrInsert(IEnumerable<User> Users)
        {
            foreach (var User in Users) UpdateOrInsert(User);
        }

        /// <summary>
        ///     Update User
        /// </summary>
        /// <param name="User"></param>
        public void Update(User User)
        {
            if (User.UserId == 0 || GetById(User.UserId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @UserId, @Picture, @Firstname, @Lastname, @Contraction, @Mail, @IsActive, @LoginUser", User);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Update Users Password
        /// </summary>
        /// <param name="User"></param>
        public void UpdatePassword(User User)
        {
            if (User.UserId == 0 || GetById(User.UserId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_UpdatePassword @UserId, @Password", User);
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
                    con.Execute($"dbo.{TableName}_Delete @UserId", new { UserId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }
    }
}