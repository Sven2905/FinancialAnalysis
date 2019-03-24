using Dapper;
using FinancialAnalysis.Models.CarPoolManagement;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Z.Dapper.Plus;

namespace FinancialAnalysis.Datalayer.Accounting
{
    public class CarModelBodyMappings : ITable
    {
        private readonly CarModelBodyMappingsStoredProcedures sp = new CarModelBodyMappingsStoredProcedures();

        public CarModelBodyMappings()
        {
            TableName = "CarModelBodyMappings";
            DapperPlusManager.Entity<CarModelBodyMapping>().Table(TableName).Identity(x => x.CarModelBodyMappingId).BatchSize(500);
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
                    $"If not exists (select name from sysobjects where name = '{TableName}') CREATE TABLE {TableName}" +
                    "(CarModelBodyMappingId int IDENTITY(1,1) PRIMARY KEY," +
                    "RefCarBodyId int, " +
                    "RefCarModelId int)";

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
        ///     Inserts the CarBody item
        /// </summary>
        /// <param name="CarBody"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(CarBody CarBody)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @RefCarBodyId, @RefCarModelId ", CarBody);
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
        ///     Inserts the list of CarBody items
        /// </summary>
        /// <param name="creditor"></param>
        public void Insert(IEnumerable<CarBody> CarModelBodyMappings)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var CarBody in CarModelBodyMappings)
                    {
                        Insert(CarBody);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        /// Bulk insert of CarBody items
        /// </summary>
        /// <param name="CarModelBodyMappings"></param>
        public void BulkInsert(IEnumerable<CarModelBodyMapping> CarModelBodyMappings)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.BulkInsert(CarModelBodyMappings);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'BulkInsert' into table '{TableName}'", e);
            }
        }
    }
}