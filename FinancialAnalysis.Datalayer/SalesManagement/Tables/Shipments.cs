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
    public class Shipments : ITable
    {
        private readonly ShipmentsStoredProcedures sp = new ShipmentsStoredProcedures();

        public Shipments()
        {
            TableName = "Shipments";
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
                                 $"CREATE TABLE {TableName}(" +
                                 $"ShipmentId int IDENTITY(1,1) PRIMARY KEY, " +
                                 "ShipmentNumber nvarchar(150), " +
                                 "ShipmentDate datetime, " +
                                 "RefSalesOrderId int, " +
                                 "RefShipmentTypeId int )";

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
        ///     Returns all Shipment records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Shipment> GetAll()
        {
            IEnumerable<Shipment> output = new List<Shipment>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<Shipment, ShipmentType, Shipment>($"dbo.{TableName}_GetAll",
                        (objShipment, objShipmentType) =>
                        {
                            //objShipment.ShipmentType = objShipmentType;
                            return objShipment;
                        }, splitOn: "ShipmentId, ShipmentTypeId",
                        commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the Shipment item
        /// </summary>
        /// <param name="Shipment"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(Shipment Shipment)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result =
                        con.Query<int>(
                            $"dbo.{TableName}_Insert @ShipmentDate, @ShipmentNumber, @RefSalesOrderId, @RefShipmentTypeId ",
                            Shipment);
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
        ///     Inserts the list of Shipment items
        /// </summary>
        /// <param name="Shipments"></param>
        public void Insert(IEnumerable<Shipment> Shipments)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var Shipment in Shipments) Insert(Shipment);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns Shipment by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Shipment GetById(int id)
        {
            var output = new Shipment();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<Shipment>($"dbo.{TableName}_GetById @ShipmentId",
                        new {ShipmentId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update Shipment, if not exist, insert it
        /// </summary>
        /// <param name="Shipment"></param>
        public void UpdateOrInsert(Shipment Shipment)
        {
            if (Shipment.ShipmentId == 0 || GetById(Shipment.ShipmentId) is null)
            {
                Insert(Shipment);
                return;
            }

            Update(Shipment);
        }

        /// <summary>
        ///     Update Shipments, if not exist insert them
        /// </summary>
        /// <param name="Shipments"></param>
        public void UpdateOrInsert(IEnumerable<Shipment> Shipments)
        {
            foreach (var Shipment in Shipments) UpdateOrInsert(Shipment);
        }

        /// <summary>
        ///     Update Shipment
        /// </summary>
        /// <param name="Shipment"></param>
        public void Update(Shipment Shipment)
        {
            if (Shipment.ShipmentId == 0 || GetById(Shipment.ShipmentId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute(
                        $"dbo.{TableName}_Update @ShipmentId, @ShipmentDate, @ShipmentNumber, @RefSalesOrderId, @RefShipmentTypeId",
                        Shipment);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete Shipment by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @ShipmentId", new {ShipmentId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete Shipment by Item
        /// </summary>
        /// <param name="id"></param>
        public void Delete(Shipment Shipment)
        {
            Delete(Shipment.ShipmentId);
        }


        public void AddReferences()
        {
            AddShipmentTypesReference();
            AddSalesOrdersReference();
        }

        private void AddShipmentTypesReference()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_ShipmentTypes', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_ShipmentTypes FOREIGN KEY(RefShipmentTypeId) REFERENCES ShipmentTypes(ShipmentTypeId) ON DELETE CASCADE";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and ShipmentTypes",
                    e);
            }
        }

        private void AddSalesOrdersReference()
        {
            string refTable = "SalesOrders";

            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_{refTable}', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_{refTable} FOREIGN KEY(RefSalesOrderId) REFERENCES {refTable}(SalesOrderId) ON DELETE CASCADE";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and {TableName}",
                    e);
            }
        }
    }
}