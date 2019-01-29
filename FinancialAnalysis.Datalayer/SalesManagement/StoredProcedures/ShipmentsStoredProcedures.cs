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
                                "SELECT s.*, sp.*, spos.*, so.*, p.*, d.*, cl.*, c.* " +
                                $"FROM {TableName} s " +
                                "LEFT JOIN ShippedProducts sp ON s.ShipmentId = sp.RefShipmentId " +
                                "LEFT JOIN SalesOrderPositions spos ON sp.RefSalesOrderPositionId = spos.SalesOrderPositionId " +
                                "LEFT JOIN SalesOrders so ON spos.RefSalesOrderId = so.SalesOrderId " +
                                "LEFT JOIN Products p ON spos.RefProductId = p.ProductId " +
                                "LEFT JOIN Debitors d ON so.RefDebitorId = d.DebitorId " +
                                "LEFT JOIN Clients cl ON d.RefClientId = d.RefClientId " +
                                "LEFT JOIN Company c ON cl.ClientId = c.RefClientId " +
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
                    $"CREATE PROCEDURE [{TableName}_Insert] @ShipmentDate datetime, @ShipmentNumber nvarchar(150) AS BEGIN SET NOCOUNT ON; " +
                    $"INSERT into {TableName} (ShipmentDate, ShipmentNumber) " +
                    "VALUES (@ShipmentDate, @ShipmentNumber); " +
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
                    "SELECT s.*, sp.*, spos.*, so.*, p.*, d.*, cl.*, c.* " +
                    $"FROM {TableName} s " +
                    "LEFT JOIN ShippedProducts sp ON s.ShipmentId = sp.RefShipmentId " +
                    "LEFT JOIN SalesOrderPositions spos ON sp.RefSalesOrderPositionId = spos.SalesOrderPositionId " +
                    "LEFT JOIN SalesOrders so ON spos.RefSalesOrderId = so.SalesOrderId " +
                    "LEFT JOIN Products p ON spos.RefProductId = p.ProductId " +
                    "LEFT JOIN Debitors d ON so.RefDebitorId = d.DebitorId " +
                    "LEFT JOIN Clients cl ON d.RefClientId = d.RefClientId " +
                    "LEFT JOIN Company c ON cl.ClientId = c.RefClientId " +
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
                    $"CREATE PROCEDURE [{TableName}_Update] @ShipmentId int, @ShipmentDate datetime, @ShipmentNumber nvarchar(150) " +
                    "AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} " +
                    "SET ShipmentDate = @ShipmentDate, " +
                    "ShipmentNumber = @ShipmentNumber " +
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