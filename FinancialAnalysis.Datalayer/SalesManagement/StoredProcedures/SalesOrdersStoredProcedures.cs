using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.SalesManagement
{
    public class SalesOrdersStoredProcedures : IStoredProcedures
    {
        public SalesOrdersStoredProcedures()
        {
            TableName = "SalesOrders";
        }

        public string TableName { get; }

        /// <summary>
        /// Check if all Stored Procedures are created, otherwise create them
        /// </summary>
        public void CheckAndCreateProcedures()
        {
            GetAllData();
            InsertData();
            GetById();
            UpdateData();
            DeleteData();
            GetOpenedSalesOrders();
        }

        private void GetAllData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAll", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_GetAll] AS BEGIN SET NOCOUNT ON; " +
                    "SELECT so.*, t.*, spo.*, d.*, cl.*, c.*, p.*, ipos.*, i.*, sp.*, s.* " +
                    $"FROM {TableName} so " +
                    "LEFT JOIN SalesOrderTypes t ON so.RefSalesOrderTypeId = t.SalesOrderTypeId " +
                    "LEFT JOIN SalesOrderPositions spos ON so.SalesOrderId = spos.RefSalesOrderId " +
                    "LEFT JOIN Debitors d ON so.RefDebitorId = d.DebitorId " +
                    "LEFT JOIN Clients cl ON d.RefClientId = d.RefClientId " +
                    "LEFT JOIN Company c ON cl.ClientId = c.RefClientId " +
                    "LEFT JOIN Products p ON spos.RefProductId = p.ProductId " +
                    "LEFT JOIN InvoicePositions ipos ON spos.SalesOrderPositionId = ipos.RefSalesOrderPositionId " +
                    "LEFT JOIN Invoices i ON ipos.RefInvoiceId = i.InvoiceId " +
                    "LEFT JOIN ShippedProducts sp ON spos.SalesOrderPositionId = sp.RefSalesOrderPositionId " +
                    "LEFT JOIN Shipments s ON sp.RefShipmentId = s.ShipmentId " +

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
                    $"CREATE PROCEDURE [{TableName}_Insert] @RefDebitorId int, @OrderDate datetime, @RefSalesTypeId int, @RefShipmentTypeId int, @Remarks nvarchar(150), @IsClosed bit AS BEGIN SET NOCOUNT ON; " +
                    $"INSERT into {TableName} (RefDebitorId, OrderDate, RefSalesTypeId, RefShipmentTypeId, Remarks, IsClosed ) " +
                    "VALUES (@RefDebitorId, @OrderDate, @RefSalesTypeId, @RefShipmentTypeId, @Remarks, @IsClosed ); " +
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
                    $"CREATE PROCEDURE [{TableName}_GetById] @SalesOrderId int AS BEGIN SET NOCOUNT ON; " +
                   "SELECT so.*, t.*, spo.*, d.*, cl.*, c.*, p.*, ipos.*, i.*, sp.*, s.* " +
                    $"FROM {TableName} so " +
                    "LEFT JOIN SalesOrderTypes t ON so.RefSalesOrderTypeId = t.SalesOrderTypeId " +
                    "LEFT JOIN SalesOrderPositions spos ON so.SalesOrderId = spos.RefSalesOrderId " +
                    "LEFT JOIN Debitors d ON so.RefDebitorId = d.DebitorId " +
                    "LEFT JOIN Clients cl ON d.RefClientId = d.RefClientId " +
                    "LEFT JOIN Company c ON cl.ClientId = c.RefClientId " +
                    "LEFT JOIN Products p ON spos.RefProductId = p.ProductId " +
                    "LEFT JOIN InvoicePositions ipos ON spos.SalesOrderPositionId = ipos.RefSalesOrderPositionId " +
                    "LEFT JOIN Invoices i ON ipos.RefInvoiceId = i.InvoiceId " +
                    "LEFT JOIN ShippedProducts sp ON spos.SalesOrderPositionId = sp.RefSalesOrderPositionId " +
                    "LEFT JOIN Shipments s ON sp.RefShipmentId = s.ShipmentId " +
                    $"WHERE so.SalesOrderId = @SalesOrderId " +
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

        private void GetOpenedSalesOrders()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetById", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_GetById] @SalesOrderId int AS BEGIN SET NOCOUNT ON; " +
                   "SELECT so.*, t.*, spo.*, d.*, cl.*, c.*, p.*, ipos.*, i.*, sp.*, s.* " +
                    $"FROM {TableName} so " +
                    "LEFT JOIN SalesOrderTypes t ON so.RefSalesOrderTypeId = t.SalesOrderTypeId " +
                    "LEFT JOIN SalesOrderPositions spos ON so.SalesOrderId = spos.RefSalesOrderId " +
                    "LEFT JOIN Debitors d ON so.RefDebitorId = d.DebitorId " +
                    "LEFT JOIN Clients cl ON d.RefClientId = d.RefClientId " +
                    "LEFT JOIN Company c ON cl.ClientId = c.RefClientId " +
                    "LEFT JOIN Products p ON spos.RefProductId = p.ProductId " +
                    "LEFT JOIN InvoicePositions ipos ON spos.SalesOrderPositionId = ipos.RefSalesOrderPositionId " +
                    "LEFT JOIN Invoices i ON ipos.RefInvoiceId = i.InvoiceId " +
                    "LEFT JOIN ShippedProducts sp ON spos.SalesOrderPositionId = sp.RefSalesOrderPositionId " +
                    "LEFT JOIN Shipments s ON sp.RefShipmentId = s.ShipmentId " +
                    $"WHERE so.IsOpened = 0 " +
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

        private void UpdateData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_Update", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_Update] @SalesOrderId int, @RefDebitorId int, @OrderDate datetime, @RefSalesTypeId int, @RefShipmentTypeId int, @Remarks nvarchar(150), @IsClosed bit " +
                    "AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} " +
                    "SET RefDebitorId = @RefDebitorId, " +
                    "OrderDate = @OrderDate, " +
                    "RefSalesTypeId = @RefSalesTypeId, " +
                    "RefShipmentTypeId = @RefShipmentTypeId, " +
                    "Remarks = @Remarks, " +
                    "IsClosed = @IsClosed " +
                    "WHERE SalesOrderId = @SalesOrderId END");
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
                    $"CREATE PROCEDURE [{TableName}_Delete] @SalesOrderId int AS BEGIN SET NOCOUNT ON; " +
                    $"DELETE FROM {TableName} WHERE SalesOrderId = @SalesOrderId END");
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