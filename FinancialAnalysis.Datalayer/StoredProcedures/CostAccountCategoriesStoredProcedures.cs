using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.StoredProcedures
{
    public class CostAccountCategoriesStoredProcedures : IStoredProcedures
    {
        public string TableName { get; }

        public CostAccountCategoriesStoredProcedures()
        {
            TableName = "CostAccountCategories";
        }

        /// <summary>
        /// Check if all Stored Procedures are created, otherwise create them
        /// </summary>
        public void CheckAndCreateProcedures()
        {
            InsertData();
            GetAllData();
            GetById();
            UpdateData();
            DeleteData();
            GetCreditorId();
            GetDebitorId();
        }

        private void GetAllData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAll", DatabaseNames.FinancialAnalysisDB))
            {
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_GetAll] AS BEGIN SET NOCOUNT ON; SELECT CostAccountCategoryId, Description, ParentCategoryId FROM {TableName} END");
                using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (SqlCommand cmd = new SqlCommand(sbSP.ToString(), connection))
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
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_Insert] @Description nvarchar(50), @ParentCategoryId int AS BEGIN SET NOCOUNT ON; " +
                                $"INSERT into {TableName} (Description, ParentCategoryId) " +
                                $"VALUES (@Description, @ParentCategoryId); " +
                                $"SELECT CAST(SCOPE_IDENTITY() as int) END");
                using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (SqlCommand cmd = new SqlCommand(sbSP.ToString(), connection))
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
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_GetById] @CostAccountCategoryId int AS BEGIN SET NOCOUNT ON; SELECT CostAccountCategoryId, Description, ParentCategoryId " +
                    $"FROM {TableName} " +
                    $"WHERE CostAccountCategoryId = @CostAccountCategoryId END");
                using (SqlConnection connection =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (SqlCommand cmd = new SqlCommand(sbSP.ToString(), connection))
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
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_Update] @CostAccountCategoryId int, @Description nvarchar(50), @ParentCategoryId int " +
                    $"AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} " +
                    $"SET Description = @Description, ParentCategoryId = @ParentCategoryId " +
                    $"WHERE CostAccountCategoryId = @CostAccountCategoryId END");
                using (SqlConnection connection =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (SqlCommand cmd = new SqlCommand(sbSP.ToString(), connection))
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
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_Delete] @CostAccountCategoryId int AS BEGIN SET NOCOUNT ON; DELETE FROM {TableName} WHERE CostAccountCategoryId = @CostAccountCategoryId OR ParentCategoryId= @CostAccountCategoryId END");
                using (SqlConnection connection =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (SqlCommand cmd = new SqlCommand(sbSP.ToString(), connection))
                    {
                        connection.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
        }

        private void GetCreditorId()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetCreditorId", DatabaseNames.FinancialAnalysisDB))
            {
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_GetCreditorId] @Description nvarchar(150) AS BEGIN SET NOCOUNT ON; SELECT CostAccountCategoryId " +
                    $"FROM {TableName} " +
                    $"WHERE Description = @Description END");
                using (SqlConnection connection =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (SqlCommand cmd = new SqlCommand(sbSP.ToString(), connection))
                    {
                        connection.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
        }

        private void GetDebitorId()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetDebitorId", DatabaseNames.FinancialAnalysisDB))
            {
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_GetDebitorId] @Description nvarchar(150) AS BEGIN SET NOCOUNT ON; SELECT CostAccountCategoryId " +
                    $"FROM {TableName} " +
                    $"WHERE Description = @Description END");
                using (SqlConnection connection =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (SqlCommand cmd = new SqlCommand(sbSP.ToString(), connection))
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
