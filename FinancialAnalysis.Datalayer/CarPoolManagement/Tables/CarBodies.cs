using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.CarPoolManagement;
using Serilog;

namespace FinancialAnalysis.Datalayer.Accounting
{
    public class CarBodies : ITable
    {
        private readonly CarBodiesStoredProcedures sp = new CarBodiesStoredProcedures();

        public CarBodies()
        {
            TableName = "CarBodies";
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
                    "(CarBodyId int IDENTITY(1,1) PRIMARY KEY," +
                    "Name nvarchar(150) NOT NULL, " +
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
        ///     Returns all CarBody records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CarBody> GetAll()
        {
            IEnumerable<CarBody> output = new List<CarBody>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<CarBody>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
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
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Name, @RefCarModelId ", CarBody);
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
        public void Insert(IEnumerable<CarBody> CarBodies)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var CarBody in CarBodies) Insert(CarBody);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns CarBody by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<CarBody> GetByRefCarModelId(int id)
        {
            IEnumerable<CarBody> output = new List<CarBody>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<CarBody>(
                        $"dbo.{TableName}_GetByRefCarModelId @RefCarModelId", new { RefCarModelId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetByRefCarModelId' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update CarBody, if not exist, insert it
        /// </summary>
        /// <param name="CarBody"></param>
        public void UpdateOrInsert(CarBody CarBody)
        {
            if (CarBody.CarBodyId == 0)
            {
                Insert(CarBody);
                return;
            }

            Update(CarBody);
        }

        /// <summary>
        ///     Update CarBodies, if not exist insert them
        /// </summary>
        /// <param name="User"></param>
        public void UpdateOrInsert(IEnumerable<CarBody> CarBodies)
        {
            foreach (var CarBody in CarBodies) UpdateOrInsert(CarBody);
        }

        /// <summary>
        ///     Update CarBody
        /// </summary>
        /// <param name="CarBody"></param>
        public void Update(CarBody CarBody)
        {
            if (CarBody.CarBodyId == 0) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @CarBodyId, @Name, @RefCarModelId",
                        CarBody);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete CarBody by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @CarBodyId", new {CarBodyId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }
    }
}