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
        }

        private void GetAllData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAll", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_GetAll] AS BEGIN SET NOCOUNT ON; " +
                    "SELECT so.SalesOrderId, so.RefDebitorId, so.OrderDate, so.RefSalesTypeId, so.Remarks, so.IsClosed, " +
                    "st.SalesTypeId, st.Name, st.Description, " +
                    "i.InvoiceId, i.InvoiceDate, i.InvoiceDueDate, i.Content, i.RefSalesOrderId, i.RefInvoiceTypeId, i.IsPaid" +
                    "sh.ShipmentId, sh.ShipmentNumber, sh.RefSalesOrderId, sh.ShipmentDate, sh.ShipmentTypeId, " +
                    "d.DebitorId, d.RefCompanyId, d.RefCostAccountId, " +
                    "sop.SalesOrderPositionId, sop.RefSalesOrderId, sop.RefProductId, sop.RefTaxTypeId, sop.Description, sop.Quantity, sop.Price, sop.DiscountPercentage, sop.IsDelivered, sop.IsCanceled , " +
                    "p.ProductId, p.Name, p.Description, p.Barcode, p.DimensionX, p.DimensionY, p.DimensionZ, p.Weight, p.IsStackable, p.Picture, p.PackageUnit, p.BuyingPrice, p.SalePrice, p.RefProductCategoryId " +
                    $"FROM {TableName} so " +
                    $"LEFT JOIN SalesTypes st ON so.RefSalesTypeId = st.SalesTypeId " +
                    $"LEFT JOIN Invoices i ON so.SalesOrderId = i.RefSalesOrderId " +
                    $"LEFT JOIN Shipments sh ON so.SalesOrderId = sh.RefSalesOrderId " +
                    $"LEFT JOIN Debitors d ON so.RefDebitorId = d.DebitorId " +
                    $"LEFT JOIN SalesOrderPositions sop ON so.SalesOrderId = sop.RefSalesOrderId " +
                    $"LEFT JOIN Product p ON sop.RefProductID = p.ProductId " +
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
                    $"CREATE PROCEDURE [{TableName}_Insert] @RefDebitorId int, @OrderDate datetime, @RefSalesTypeId int, @Remarks nvarchar(150), @DiscountPercentage money, @IsClosed bit AS BEGIN SET NOCOUNT ON; " +
                    $"INSERT into {TableName} (RefDebitorId, OrderDate, RefSalesTypeId, Remarks, IsClosed ) " +
                    "VALUES (@RefDebitorId, @OrderDate, @RefSalesTypeId, @Remarks, @IsClosed ); " +
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
                    "SELECT so.SalesOrderId, so.RefDebitorId, so.OrderDate, so.RefSalesTypeId, so.Remarks, so.IsClosed, " +
                    "st.SalesTypeId, st.Name, st.Description, " +
                    "i.InvoiceId, i.InvoiceDate, i.InvoiceDueDate, i.Content, i.RefSalesOrderId, i.RefInvoiceTypeId, i.IsPaid" +
                    "sh.ShipmentId, sh.ShipmentNumber, sh.RefSalesOrderId, sh.ShipmentDate, sh.ShipmentTypeId, " +
                    "d.DebitorId, d.RefCompanyId, d.RefCostAccountId, " +
                    "sop.SalesOrderPositionId, sop.RefSalesOrderId, sop.RefProductId, sop.RefTaxTypeId, sop.Description, sop.Quantity, sop.Price, sop.DiscountPercentage, sop.IsDelivered, sop.IsCanceled , " +
                    "p.ProductId, p.Name, p.Description, p.Barcode, p.DimensionX, p.DimensionY, p.DimensionZ, p.Weight, p.IsStackable, p.Picture, p.PackageUnit, p.BuyingPrice, p.SalePrice, p.RefProductCategoryId " +
                    $"FROM {TableName} so " +
                    $"LEFT JOIN SalesTypes st ON so.RefSalesTypeId = st.SalesTypeId " +
                    $"LEFT JOIN Invoices i ON so.SalesOrderId = i.RefSalesOrderId " +
                    $"LEFT JOIN Shipments sh ON so.SalesOrderId = sh.RefSalesOrderId " +
                    $"LEFT JOIN Debitors d ON so.RefDebitorId = d.DebitorId " +
                    $"LEFT JOIN SalesOrderPositions sop ON so.SalesOrderId = sop.RefSalesOrderId " +
                    $"LEFT JOIN Product p ON sop.RefProductID = p.ProductId " +
                    $"WHERE pt.SalesOrderId = @SalesOrderId " +
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
                    $"CREATE PROCEDURE [{TableName}_Update] @SalesOrderId int, @RefDebitorId int, @OrderDate datetime, @RefSalesTypeId int, @Remarks nvarchar(150), @IsClosed bit " +
                    "AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} " +
                    "SET RefDebitorId = @RefDebitorId, " +
                    "OrderDate = @OrderDate, " +
                    "RefSalesTypeId = @RefSalesTypeId, " +
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