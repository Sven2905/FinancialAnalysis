using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.WarehouseManagement
{
    public class WarehouseStockingHistoriesStoredProcedures : IStoredProcedures
    {
        public WarehouseStockingHistoriesStoredProcedures()
        {
            TableName = "WarehouseStockingHistories";
        }

        public string TableName { get; }

        /// <summary>
        ///     Check if all Stored Procedures are created, otherwise create them
        /// </summary>
        public void CheckAndCreateProcedures()
        {
            InsertData();
            GetAllData();
            GetLast10();
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
                                "SELECT w.*, p.*, s.* " +
                                $"FROM {TableName} w " +
                                "LEFT JOIN Products p ON w.RefProductId = p.ProductId " +
                                "LEFT JOIN Stockyards s on w.RefStockyardId = s.StockyardId " +
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

        private void GetLast10()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetLast10", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_GetLast10] @RefProductId int, @RefStockyardId int AS BEGIN SET NOCOUNT ON; " +
                                "SELECT TOP 10 w.*, p.*, s.* " +
                                $"FROM {TableName} w " +
                                "LEFT JOIN Products p ON w.RefProductId = p.ProductId " +
                                "LEFT JOIN Stockyards s on w.RefStockyardId = s.StockyardId " +
                                "WHERE w.RefProductId = @RefProductId " +
                                "AND w.RefStockyardId = @RefStockyardId " +
                                "ORDER BY w.WarehouseStockingHistoryId DESC " +
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
                    $"CREATE PROCEDURE [{TableName}_Insert] @RefProductId int, @ProductName nvarchar(150), @RefStockyardId int, @StockyardName nvarchar(150), @Quantity int, @RefUserId int, @UserName nvarchar(150), @Date datetime AS BEGIN SET NOCOUNT ON; " +
                    $"INSERT into {TableName} (RefProductId, ProductName, RefStockyardId, StockyardName, Quantity, RefUserId, UserName, Date) " +
                    "VALUES (@RefProductId, @ProductName, @RefStockyardId, @StockyardName, @Quantity, @RefUserId, @UserName, @Date); " +
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
                    $"CREATE PROCEDURE [{TableName}_GetById] @WarehouseStockingHistoryId int AS BEGIN SET NOCOUNT ON; " +
                    "SELECT w.*, p.*, s.* " +
                    $"FROM {TableName} w " +
                    "LEFT JOIN Products p ON w.RefProductId = p.ProductId " +
                    "LEFT JOIN Stockyards s on w.RefStockyardId = s.StockyardId " +
                    "WHERE WarehouseStockingHistoryId = @WarehouseStockingHistoryId END");
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
                    $"CREATE PROCEDURE [{TableName}_Update] @WarehouseStockingHistoryId int, @RefProductId int, @ProductName nvarchar(150), @RefStockyardId int, @StockyardName nvarchar(150), @Quantity int, @RefUserId int, @UserName nvarchar(150), @Date datetime " +
                    "AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} " +
                    "SET RefProductId = @RefProductId, " +
                    "ProductName = @ProductName, " +
                    "RefStockyardId = @RefStockyardId, " +
                    "StockyardName = @StockyardName, " +
                    "Quantity = @Quantity, " +
                    "RefUserId = @RefUserId, " +
                    "UserName = @UserName, " +
                    "Date = @Date " +
                    "WHERE WarehouseStockingHistoryId = @WarehouseStockingHistoryId END");
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
                    $"CREATE PROCEDURE [{TableName}_Delete] @WarehouseStockingHistoryId int AS BEGIN SET NOCOUNT ON; " +
                    $"DELETE FROM {TableName} " +
                    $"WHERE WarehouseStockingHistoryId = @WarehouseStockingHistoryId END");
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