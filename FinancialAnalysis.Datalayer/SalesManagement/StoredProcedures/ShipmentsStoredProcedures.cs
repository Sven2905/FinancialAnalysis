using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.SalesManagement
{
    internal class ShipmentsStoredProcedures : IStoredProcedures
    {
        public ShipmentsStoredProcedures()
        {
            TableName = "Shipments";
        }

        public string TableName { get; }

        /// <summary>
        ///     Check if all Stored Procedures are created, otherwise create them
        /// </summary>
        public void CheckAndCreateProcedures()
        {
            GetAllData();
            InsertData();
            GetById();
            UpdateData();
            DeleteData();
        }

        private void GetAllData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAll", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_GetAll] AS BEGIN SET NOCOUNT ON; " +
                                "SELECT s.ShipmentId, s.ShipmentDate, s.ShipmentNumber, s.RefSalesOrderId, s.RefShipmentTypeId, " +
                                $"t.ShipmentTypeId, t.Name, t.Description " +
                                $"FROM {TableName} s " +
                                "LEFT JOIN ShipmentTypes t ON s.RefShipmentTypeId = t.ShipmentTypeId " +
                                "END");
                using (var connection =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (var cmd = new SqlCommand(sbSP.ToString(), connection))
                    {
                        connection.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
        }

        private void InsertData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_Insert", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_Insert] @ShipmentDate datetime, @ShipmentNumber nvarchar(150), @RefSalesOrderId int, @RefShipmentTypeId int AS BEGIN SET NOCOUNT ON; " +
                    $"INSERT into {TableName} (ShipmentDate, ShipmentNumber, RefSalesOrderId, RefShipmentTypeId) " +
                    "VALUES (@ShipmentDate, @ShipmentNumber, @RefSalesOrderId, @RefShipmentTypeId); " +
                    "SELECT CAST(SCOPE_IDENTITY() as int) END");
                using (var connection =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (var cmd = new SqlCommand(sbSP.ToString(), connection))
                    {
                        connection.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
        }

        private void GetById()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetById", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_GetById] @ShipmentId int AS BEGIN SET NOCOUNT ON; " +
                    $"SELECT ShipmentId, ShipmentDate, ShipmentNumber, RefSalesOrderId, RefShipmentTypeId " +
                    $"FROM {TableName} " +
                    "WHERE ShipmentId = @ShipmentId END");
                using (var connection =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (var cmd = new SqlCommand(sbSP.ToString(), connection))
                    {
                        connection.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
        }

        private void UpdateData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_Update", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_Update] @ShipmentId int, @ShipmentDate datetime, @ShipmentNumber nvarchar(150), @RefSalesOrderId int, @RefShipmentTypeId int " +
                    "AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} " +
                    "SET ShipmentDate = @ShipmentDate, " +
                    "ShipmentNumber = @ShipmentNumber, " +
                    "RefSalesOrderId = @RefSalesOrderId, " +
                    "RefShipmentTypeId = @RefShipmentTypeId " +
                    "WHERE ShipmentId = @ShipmentId END");
                using (var connection =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (var cmd = new SqlCommand(sbSP.ToString(), connection))
                    {
                        connection.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
        }

        private void DeleteData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_Delete", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_Delete] @ShipmentId int AS BEGIN SET NOCOUNT ON; " +
                    $"DELETE FROM {TableName} WHERE ShipmentId = @ShipmentId END");
                using (var connection =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (var cmd = new SqlCommand(sbSP.ToString(), connection))
                    {
                        connection.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
        }
    }
}