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
    public class CarMakes : ITable
    {
        private readonly CarMakesStoredProcedures sp = new CarMakesStoredProcedures();

        public CarMakes()
        {
            TableName = "CarMakes";
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
                    "(CarMakeId int IDENTITY(1,1) PRIMARY KEY," +
                    "Name nvarchar(150) NOT NULL)";

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
        ///     Returns all CarMake records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CarMake> GetAll()
        {
            IEnumerable<CarMake> output = new List<CarMake>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<CarMake>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the CarMake item
        /// </summary>
        /// <param name="CarMake"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(CarMake CarMake)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Name ", CarMake);
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
        ///     Inserts the list of CarMake items
        /// </summary>
        /// <param name="creditor"></param>
        public void Insert(IEnumerable<CarMake> CarMakes)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var CarMake in CarMakes) Insert(CarMake);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns CarMake by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CarMake GetById(int id)
        {
            var output = new CarMake();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<CarMake>(
                        $"dbo.{TableName}_GetById @CarMakeId", new { CarMakeId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update CarMake, if not exist, insert it
        /// </summary>
        /// <param name="CarMake"></param>
        public void UpdateOrInsert(CarMake CarMake)
        {
            if (CarMake.CarMakeId == 0 ||
                GetById(CarMake.CarMakeId) is null)
            {
                Insert(CarMake);
                return;
            }

            Update(CarMake);
        }

        /// <summary>
        ///     Update CarMakes, if not exist insert them
        /// </summary>
        /// <param name="User"></param>
        public void UpdateOrInsert(IEnumerable<CarMake> CarMakes)
        {
            foreach (var CarMake in CarMakes) UpdateOrInsert(CarMake);
        }

        /// <summary>
        ///     Update CarMake
        /// </summary>
        /// <param name="CarMake"></param>
        public void Update(CarMake CarMake)
        {
            if (CarMake.CarMakeId == 0 ||
                GetById(CarMake.CarMakeId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @CarMakeId, @Name",
                        CarMake);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete CarMake by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @CarMakeId", new {CarMakeId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }
    }
}