using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.PurchaseManagement
{
    public class PurchaseOrdersStoredProcedures : IStoredProcedures
    {
        public PurchaseOrdersStoredProcedures()
        {
            TableName = "PurchaseOrders";
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

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_GetAll] AS BEGIN SET NOCOUNT ON; " +
                    "SELECT po.PurchaseOrderId, po.PurchaseInvoiceNumber, po.RefCreditorId, po.OrderDate, po.DeliveryDate, po.RefPurchaseTypeId, po.Remarks, po.IsClosed, " +
                    "pt.PurchaseTypeId, pt.Name, pt.Description, " +
                    "b.BillId, b.CreditorInvoiceNumber, b.BillDate, b.BillDueDate, b.Content, b.RefPurchaseOrderId, b.RefBillTypeId, " +
                    "grn.RefPurchaseOrderId, grn.Content, " +
                    "c.CreditorId, c.RefCompanyId, c.RefCostAccountId, " +
                    "pop.PurchaseOrderPositionId, pop.RefPurchaseOrderId, pop.RefProductId, pop.RefTaxTypeId, pop.Description, pop.Quantity, pop.Price, pop.DiscountPercentage, pop.IsDelivered, pop.IsCanceled , " +
                    "p.ProductId, p.Name, p.Description, p.Barcode, p.DimensionX, p.DimensionY, p.DimensionZ, p.Weight, p.IsStackable, p.Picture, p.PackageUnit, p.BuyingPrice, p.SalePrice, p.RefProductCategoryId " +
                    $"FROM {TableName} po " +
                    $"LEFT JOIN PurchaseTypes pt ON po.RefPurchaseTypeId = pt.PurchaseTypeId " +
                    $"LEFT JOIN Bills b ON po.PurchaseOrderId = b.RefPurchaseOrderId " +
                    $"LEFT JOIN GoodsReceivedNotes grn ON po.PurchaseOrderId = grn.RefPurchaseOrderId " +
                    $"LEFT JOIN Creditor c ON po.RefCreditorId = c.CreditorId " +
                    $"LEFT JOIN PurchaseOrderPositions pop ON po.PurchaseOrderId = pop.RefPurchaseOrderId " +
                    $"LEFT JOIN Product p ON pop.RefProductID = p.ProductId " +
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
                    $"CREATE PROCEDURE [{TableName}_Insert] @PurchaseInvoiceNumber nvarchar(150), @RefCreditorId int, @OrderDate datetime, @DeliveryDate datetime, @RefPurchaseTypeId int, @Remarks nvarchar(150), @DiscountPercentage money, @IsClosed bit AS BEGIN SET NOCOUNT ON; " +
                    $"INSERT into {TableName} (PurchaseInvoiceNumber, RefCreditorId, OrderDate, DeliveryDate, RefPurchaseTypeId, Remarks, IsClosed ) " +
                    "VALUES (@PurchaseInvoiceNumber, @RefCreditorId, @OrderDate, @DeliveryDate, @RefPurchaseTypeId, @Remarks, @IsClosed ); " +
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
                    $"CREATE PROCEDURE [{TableName}_GetById] @PurchaseOrderId int AS BEGIN SET NOCOUNT ON; " +
                    "SELECT po.PurchaseOrderId, po.PurchaseInvoiceNumber, po.RefCreditorId, po.OrderDate, po.DeliveryDate, po.RefPurchaseTypeId, po.Remarks, po.IsClosed, " +
                    "pt.PurchaseTypeId, pt.Name, pt.Description, " +
                    "b.BillId, b.CreditorInvoiceNumber, b.BillDate, b.BillDueDate, b.Content, b.RefPurchaseOrderId, b.RefBillTypeId, " +
                    "grn.RefPurchaseOrderId, grn.Content, " +
                    "c.CreditorId, c.RefCompanyId, c.RefCostAccountId, " +
                    "pop.PurchaseOrderPositionId, pop.RefPurchaseOrderId, pop.RefProductId, pop.RefTaxTypeId, pop.Description, pop.Quantity, pop.Price, pop.DiscountPercentage, pop.IsDelivered, pop.IsCanceled , " +
                    "p.ProductId, p.Name, p.Description, p.Barcode, p.DimensionX, p.DimensionY, p.DimensionZ, p.Weight, p.IsStackable, p.Picture, p.PackageUnit, p.BuyingPrice, p.SalePrice, p.RefProductCategoryId " +
                    $"FROM {TableName} po " +
                     $"LEFT JOIN PurchaseTypes pt ON po.RefPurchaseTypeId = pt.PurchaseTypeId " +
                    $"LEFT JOIN Bills b ON po.PurchaseOrderId = b.RefPurchaseOrderId " +
                    $"LEFT JOIN GoodsReceivedNotes grn ON po.PurchaseOrderId = grn.RefPurchaseOrderId " +
                    $"LEFT JOIN Creditor c ON po.RefCreditorId = c.CreditorId " +
                    $"LEFT JOIN PurchaseOrderPositions pop ON po.PurchaseOrderId = pop.RefPurchaseOrderId " +
                    $"LEFT JOIN Product p ON pop.RefProductID = p.ProductId " +
                    $"WHERE pt.PurchaseOrderId = @PurchaseOrderId " +
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
                    $"CREATE PROCEDURE [{TableName}_Update] @PurchaseOrderId int, @PurchaseInvoiceNumber nvarchar(150), @RefCreditorId int, @OrderDate datetime, @DeliveryDate datetime, @RefPurchaseTypeId int, @Remarks nvarchar(150), @IsClosed bit " +
                    "AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} " +
                    "SET PurchaseInvoiceNumber = @PurchaseInvoiceNumber, " +
                    "RefCreditorId = @RefCreditorId, " +
                    "OrderDate = @OrderDate, " +
                    "DeliveryDate = @DeliveryDate, " +
                    "RefPurchaseTypeId = @RefPurchaseTypeId, " +
                    "Remarks = @Remarks, " +
                    "IsClosed = @IsClosed " +
                    "WHERE PurchaseOrderId = @PurchaseOrderId END");
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
                    $"CREATE PROCEDURE [{TableName}_Delete] @PurchaseOrderId int AS BEGIN SET NOCOUNT ON; " +
                    $"DELETE FROM {TableName} WHERE PurchaseOrderId = @PurchaseOrderId END");
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