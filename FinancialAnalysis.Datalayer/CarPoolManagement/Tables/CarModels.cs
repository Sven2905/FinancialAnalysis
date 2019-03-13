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
    public class CarModels : ITable
    {
        private readonly CarModelsStoredProcedures sp = new CarModelsStoredProcedures();

        public CarModels()
        {
            TableName = "CarModels";
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
                    "(CarModelId int IDENTITY(1,1) PRIMARY KEY," +
                    "Name nvarchar(150) NOT NULL, " +
                    "RefCarMakeId int)";

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
        ///     Returns all CarModel records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CarModel> GetAll()
        {
            IEnumerable<CarModel> output = new List<CarModel>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<CarModel>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the CarModel item
        /// </summary>
        /// <param name="CarModel"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(CarModel CarModel)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Name, @RefCarMakeId ", CarModel);
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
        ///     Inserts the list of CarModel items
        /// </summary>
        /// <param name="creditor"></param>
        public void Insert(IEnumerable<CarModel> CarModels)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var CarModel in CarModels) Insert(CarModel);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns CarModel by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<CarModel> GetByRefCarMakeId(int id)
        {
            IEnumerable<CarModel> output = new List<CarModel>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<CarModel>(
                        $"dbo.{TableName}_GetByRefCarMakeId @RefCarMakeId", new { RefCarMakeId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetByRefCarMakeId' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update CarModel, if not exist, insert it
        /// </summary>
        /// <param name="CarModel"></param>
        public void UpdateOrInsert(CarModel CarModel)
        {
            if (CarModel.CarModelId == 0)
            {
                Insert(CarModel);
                return;
            }

            Update(CarModel);
        }

        /// <summary>
        ///     Update CarModels, if not exist insert them
        /// </summary>
        /// <param name="User"></param>
        public void UpdateOrInsert(IEnumerable<CarModel> CarModels)
        {
            foreach (var CarModel in CarModels) UpdateOrInsert(CarModel);
        }

        /// <summary>
        ///     Update CarModel
        /// </summary>
        /// <param name="CarModel"></param>
        public void Update(CarModel CarModel)
        {
            if (CarModel.CarModelId == 0) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @CarModelId, @Name, @RefCarMakeId",
                        CarModel);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete CarModel by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @CarModelId", new {CarModelId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }
    }
}