using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.Accounting
{
    internal class FixedCostAllocationsStoredProcedures : IStoredProcedures
    {
        public FixedCostAllocationsStoredProcedures()
        {
            TableName = "FixedCostAllocations";
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
                    $"SELECT f.*, c.*, cc.* " +
                    $"FROM {TableName} f " +
                    $"JOIN CostCenters c ON f.RefCostCenterId = c.CostCenterId " +
                    $"JOIN CostCenterCategories cc ON c.RefCostCenterCategoryId = cc.CostCenterCategoryId " +
                    $"END");
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
                    $"CREATE PROCEDURE [{TableName}_Insert] @Shares decimal, @RefCostCenterId int AS BEGIN SET NOCOUNT ON; " +
                    $"INSERT into {TableName} (Shares, RefCostCenterId) " +
                    "VALUES (@Shares, @RefCostCenterId); " +
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
                    $"CREATE PROCEDURE [{TableName}_GetById] @FixedCostAllocationId int AS BEGIN SET NOCOUNT ON; " +
                    $"SELECT FixedCostAllocationId, Shares, RefCostCenterId " +
                    $"FROM {TableName} " +
                    "WHERE FixedCostAllocationId = @FixedCostAllocationId END");
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
                    $"CREATE PROCEDURE [{TableName}_Update] @FixedCostAllocationId int, @Shares decimal, @RefCostCenterId int " +
                    "AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} " +
                    "SET Shares = @Shares, " +
                    "@RefCostCenterId = RefCostCenterId " +
                    "WHERE FixedCostAllocationId = @FixedCostAllocationId END");
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
                    $"CREATE PROCEDURE [{TableName}_Delete] @FixedCostAllocationId int AS BEGIN SET NOCOUNT ON; " +
                    $"DELETE FROM {TableName} " +
                    $"WHERE FixedCostAllocationId = @FixedCostAllocationId END");
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