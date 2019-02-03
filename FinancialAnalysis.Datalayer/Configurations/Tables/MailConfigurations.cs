using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models.Mail;
using Serilog;

namespace FinancialAnalysis.Datalayer.Configurations
{
    public class MailConfigurations : ITable
    {
        private readonly MailConfigurationsStoredProcedures sp = new MailConfigurationsStoredProcedures();

        public MailConfigurations()
        {
            TableName = "MailConfigurations";
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
                    "MailConfigurationId int IDENTITY(1,1) PRIMARY KEY," +
                    "Server nvarchar(150) NOT NULL," +
                    "Address nvarchar(150) NOT NULL," +
                    "LoginUser nvarchar(150) NOT NULL," +
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
        ///     Returns all MailConfiguration records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MailConfiguration> GetAll()
        {
            IEnumerable<MailConfiguration> output = new List<MailConfiguration>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<MailConfiguration>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the MailConfiguration item
        /// </summary>
        /// <param name="MailConfiguration"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(MailConfiguration MailConfiguration)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result =
                        con.Query<int>(
                            $"dbo.{TableName}_Insert @Server, @Address, @LoginUser, @Password ",
                            MailConfiguration);
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
        ///     Inserts the list of MailConfiguration items
        /// </summary>
        /// <param name="ProductPrototype"></param>
        public void Insert(IEnumerable<MailConfiguration> MailConfigurations)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var MailConfiguration in MailConfigurations) Insert(MailConfiguration);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns MailConfiguration by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MailConfiguration GetById(int id)
        {
            var output = new MailConfiguration();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<MailConfiguration>(
                        $"dbo.{TableName}_GetById @MailConfigurationId",
                        new {MailConfigurationId = id});
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