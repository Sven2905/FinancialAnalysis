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
    public class Vehicles : ITable
    {
        private readonly VehiclesStoredProcedures sp = new VehiclesStoredProcedures();

        public Vehicles()
        {
            TableName = "Vehicles";
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
                    "(VehicleId int IDENTITY(1,1) PRIMARY KEY," +
                    "LicenseNumber nvarchar(150), " + 
                    "VehicleNumber nvarchar(150), " +
                    "Color nvarchar(150), " +
                    "AcquisitionDate datetime, " +
                    "FirstRegistrationDate datetime, " +
                    "MilageOnAcquisition money, " +
                    "CurrentMilage money, " +
                    "RefCarEngineId int)";

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
        ///     Returns all Vehicle records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Vehicle> GetAll()
        {
            IEnumerable<Vehicle> output = new List<Vehicle>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<Vehicle>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the Vehicle item
        /// </summary>
        /// <param name="Vehicle"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(Vehicle Vehicle)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @LicenseNumber, @VehicleNumber, @Color, @AcquisitionDate, @FirstRegistrationDate, @MilageOnAcquisition, @CurrentMilage, @RefCarEngineId ", Vehicle);
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
        ///     Inserts the list of Vehicle items
        /// </summary>
        /// <param name="creditor"></param>
        public void Insert(IEnumerable<Vehicle> Vehicles)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var Vehicle in Vehicles) Insert(Vehicle);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns Vehicle by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Vehicle GetById(int id)
        {
            var output = new Vehicle();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<Vehicle>(
                        $"dbo.{TableName}_GetById @VehicleId", new { VehicleId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update Vehicle, if not exist, insert it
        /// </summary>
        /// <param name="Vehicle"></param>
        public void UpdateOrInsert(Vehicle Vehicle)
        {
            if (Vehicle.VehicleId == 0 ||
                GetById(Vehicle.VehicleId) is null)
            {
                Insert(Vehicle);
                return;
            }

            Update(Vehicle);
        }

        /// <summary>
        ///     Update Vehicles, if not exist insert them
        /// </summary>
        /// <param name="User"></param>
        public void UpdateOrInsert(IEnumerable<Vehicle> Vehicles)
        {
            foreach (var Vehicle in Vehicles) UpdateOrInsert(Vehicle);
        }

        /// <summary>
        ///     Update Vehicle
        /// </summary>
        /// <param name="Vehicle"></param>
        public void Update(Vehicle Vehicle)
        {
            if (Vehicle.VehicleId == 0
                || GetById(Vehicle.VehicleId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @VehicleId, @LicenseNumber, @VehicleNumber, @Color, @AcquisitionDate, @FirstRegistrationDate, @MilageOnAcquisition, @CurrentMilage, @RefCarEngineId",
                        Vehicle);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete Vehicle by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @VehicleId", new {VehicleId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }
    }
}