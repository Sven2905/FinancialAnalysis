using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models.SalesManagement;
using Serilog;

namespace FinancialAnalysis.Datalayer.SalesManagement
{
    public class ShipmentTypes : ITable
    {
        private readonly ShipmentTypesStoredProcedures sp = new ShipmentTypesStoredProcedures();

        public ShipmentTypes()
        {
            TableName = "ShipmentTypes";
            CheckAndCreateTable();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs\\Tables.txt", rollingInterval: RollingInterval.Month)
                .CreateLogger();
        }

        public string TableName { get; }

        public void CheckAndCreateStoredProcedures()
        {
            sp.CheckAndCreateProcedures();
        }

        public void CheckAndCreateTable()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr = $"If not exists (select name from sysobjects where name = '{TableName}') " +
                                 $"CREATE TABLE {TableName}" +
                                 "(ShipmentTypeId int IDENTITY(1,1) PRIMARY KEY, " +
                                 "Name nvarchar(150) NOT NULL, " +
                                 "Description nvarchar(150))";

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

        /// <summary>
        ///     Returns all ShipmentType records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ShipmentType> GetAll()
        {
            IEnumerable<ShipmentType> output = new List<ShipmentType>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<ShipmentType>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the ShipmentType item
        /// </summary>
        /// <param name="ShipmentType"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(ShipmentType ShipmentType)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Name, @Description ",
                        ShipmentType);
                    return result.Single();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' from table '{TableName}'", e);
            }

            return id;
        }

        /// <summary>
        ///     Inserts the list of ShipmentType items
        /// </summary>
        /// <param name="ShipmentTypes"></param>
        public void Insert(IEnumerable<ShipmentType> ShipmentTypes)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var ShipmentType in ShipmentTypes) Insert(ShipmentType);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns ShipmentType by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ShipmentType GetById(int id)
        {
            var output = new ShipmentType();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<ShipmentType>(
                        $"dbo.{TableName}_GetById @ShipmentTypeId", new {ShipmentTypeId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update ShipmentType, if not exist, insert it
        /// </summary>
        /// <param name="ShipmentType"></param>
        public void UpdateOrInsert(ShipmentType ShipmentType)
        {
            if (ShipmentType.ShipmentTypeId == 0 ||
                GetById(ShipmentType.ShipmentTypeId) is null)
            {
                Insert(ShipmentType);
                return;
            }

            Update(ShipmentType);
        }

        /// <summary>
        ///     Update ShipmentTypes, if not exist insert them
        /// </summary>
        /// <param name="ShipmentTypes"></param>
        public void UpdateOrInsert(IEnumerable<ShipmentType> ShipmentTypes)
        {
            foreach (var ShipmentType in ShipmentTypes) UpdateOrInsert(ShipmentType);
        }

        /// <summary>
        ///     Update ShipmentType
        /// </summary>
        /// <param name="ShipmentType"></param>
        public void Update(ShipmentType ShipmentType)
        {
            if (ShipmentType.ShipmentTypeId == 0 ||
                GetById(ShipmentType.ShipmentTypeId) is null)
                return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @ShipmentTypeId, @Name, @Description ",
                        ShipmentType);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete ShipmentType by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @ShipmentTypeId", new {ShipmentTypeId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete ShipmentType by Item
        /// </summary>
        /// <param name="id"></param>
        public void Delete(ShipmentType ShipmentType)
        {
            Delete(ShipmentType.ShipmentTypeId);
        }
    }
}