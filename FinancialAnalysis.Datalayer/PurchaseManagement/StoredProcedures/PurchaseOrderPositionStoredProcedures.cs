using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.PurchaseManagement
{
    public class PurchaseOrderPositionsStoredProcedures : IStoredProcedures
    {
        public PurchaseOrderPositionsStoredProcedures()
        {
            TableName = "PurchaseOrderPositions";
        }

        public string TableName { get; }

        /// <summary>
        ///     Check if all Stored Procedures are created, otherwise create them
        /// </summary>
        public void CheckAndCreateProcedures()
        {
            InsertData();
            GetAllData();
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
                    $"SELECT PurchaseOrderPositionId, RefPurchaseOrderId, RefProductId, RefTaxTypeId, Description, Quantity, Price, DiscountPercentage, IsDelivered, IsCanceled " +
                    $"FROM {TableName} END");
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
                    $"CREATE PROCEDURE [{TableName}_Insert] @RefPurchaseOrderId int, @RefProductId int, @RefTaxTypeId int, @Description nvarchar(150), @Quantity int, @Price money, @DiscountPercentage money, @IsDelivered bit, @IsCanceled bit AS BEGIN SET NOCOUNT ON; " +
                    $"INSERT into {TableName} (RefPurchaseOrderId, RefProductId, RefTaxTypeId, Description, Quantity, Price, DiscountPercentage, IsDelivered, IsCanceled ) " +
                    "VALUES (@RefPurchaseOrderId, @RefProductId, @RefTaxTypeId, @Description, @Quantity, @Price, @DiscountPercentage, @IsDelivered, @IsCanceled ); " +
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
                    $"CREATE PROCEDURE [{TableName}_GetById] @PurchaseOrderPositionId int AS BEGIN SET NOCOUNT ON; SELECT PurchaseOrderPositionId, RefPurchaseOrderId, RefProductId, RefTaxTypeId, Description, Quantity, Price, DiscountPercentage, IsDelivered, IsCanceled " +
                    $"FROM {TableName} " +
                    "WHERE PurchaseOrderPositionId = @PurchaseOrderPositionId END");
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
                    $"CREATE PROCEDURE [{TableName}_Update] @PurchaseOrderPositionId int, @RefPurchaseOrderId int, @RefProductId int, @RefTaxTypeId int, @Description nvarchar(150), @Quantity int, @Price money, @DiscountPercentage money, @IsDelivered bit, @IsCanceled bit " +
                    "AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} " +
                    "SET RefProductId = @RefProductId, " +
                    "RefTaxTypeId = @RefTaxTypeId, " +
                    "Description = @Description, " +
                    "Quantity = @Quantity, " +
                    "Price = @Price, " +
                    "DiscountPercentage = @DiscountPercentage, " +
                    "IsDelivered = @IsDelivered, " +
                    "IsCanceled = @IsCanceled " +
                    "WHERE PurchaseOrderPositionId = @PurchaseOrderPositionId END");
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
                    $"CREATE PROCEDURE [{TableName}_Delete] @PurchaseOrderPositionId int AS BEGIN SET NOCOUNT ON; " +
                    $"DELETE FROM {TableName} WHERE PurchaseOrderPositionId = @PurchaseOrderPositionId END");
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