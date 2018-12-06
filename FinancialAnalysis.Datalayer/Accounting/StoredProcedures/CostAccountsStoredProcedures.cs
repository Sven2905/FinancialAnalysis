using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Datalayer.Accounting
{
    class CostAccountsStoredProcedures : IStoredProcedures
    {
        public string TableName { get; }

        public CostAccountsStoredProcedures()
        {
            TableName = "CostAccounts";
        }

        /// <summary>
        /// Check if all Stored Procedures are created, otherwise create them
        /// </summary>
        public void CheckAndCreateProcedures()
        {
            GetAllData();
            GetAllVisibleData();
            InsertData();
            GetById();
            UpdateData();
            DeleteData();
            GetNextCreditorNumber();
            GetNextDebitorNumber();
        }

        private void GetAllData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAll", DatabaseNames.FinancialAnalysisDB))
            {
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_GetAll] AS BEGIN SET NOCOUNT ON; " +
                    $"SELECT a.CostAccountId, a.Description, a.AccountNumber, a.RefTaxTypeId, a.RefCostAccountCategoryId, a.IsVisible, a.IsEditable, " +
                    $"c.CostAccountCategoryId, c.Description, c.ParentCategoryId FROM {TableName} a " +
                    $"LEFT JOIN CostAccountCategories c ON a.RefCostAccountCategoryId = c.CostAccountCategoryId " +
                    $"END");
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

        private void GetAllVisibleData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAllVisible", DatabaseNames.FinancialAnalysisDB))
            {
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_GetAllVisible] AS BEGIN SET NOCOUNT ON; " +
                    $"SELECT a.CostAccountId, a.Description, a.AccountNumber, a.RefTaxTypeId, a.RefCostAccountCategoryId, a.IsVisible, a.IsEditable, " +
                    $"c.CostAccountCategoryId, c.Description, c.ParentCategoryId FROM {TableName} a " +
                    $"LEFT JOIN CostAccountCategories c ON a.RefCostAccountCategoryId = c.CostAccountCategoryId " +
                    $"WHERE IsVisible = 1 END");
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

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_Insert] @Description nvarchar(150), @AccountNumber int, @RefTaxTypeId int, @RefCostAccountCategoryId int, @IsVisible bit, @IsEditable bit AS BEGIN SET NOCOUNT ON; " +
                                $"INSERT into {TableName} (Description, AccountNumber, RefTaxTypeId, RefCostAccountCategoryId, IsVisible, IsEditable) " +
                                $"VALUES (@Description, @AccountNumber, @RefTaxTypeId, @RefCostAccountCategoryId, @IsVisible, @IsEditable); " +
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
                    $"CREATE PROCEDURE [{TableName}_GetById] @CostAccountId int AS BEGIN SET NOCOUNT ON; SELECT CostAccountId, Description, AccountNumber, RefTaxTypeId, RefCostAccountCategoryId, IsVisible, IsEditable " +
                    $"FROM {TableName} " +
                    $"WHERE CostAccountId = @CostAccountId END");
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
                    $"CREATE PROCEDURE [{TableName}_Update] @CostAccountId int, @Description nvarchar(150), @AccountNumber int, @RefTaxTypeId int, @RefCostAccountCategoryId int, @IsVisible bit, @IsEditable bit " +
                    $"AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} " +
                    $"SET Description = @Description, AccountNumber = @AccountNumber, RefTaxTypeId = @RefTaxTypeId, RefCostAccountCategoryId = @RefCostAccountCategoryId, IsVisible = @IsVisible, IsEditable = @IsEditable " +
                    $"WHERE CostAccountId = @CostAccountId END");
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
                    $"CREATE PROCEDURE [{TableName}_Delete] @CostAccountId int AS BEGIN SET NOCOUNT ON; DELETE FROM {TableName} WHERE CostAccountId = @CostAccountId END");
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

        private void GetNextCreditorNumber()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetNextCreditorNumber", DatabaseNames.FinancialAnalysisDB))
            {
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_GetNextCreditorNumber] AS BEGIN SET NOCOUNT ON; SELECT MAX(AccountNumber) " +
                    $"FROM {TableName} " +
                    $"WHERE AccountNumber >= 70000 END");
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

        private void GetNextDebitorNumber()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetNextDebitorNumber", DatabaseNames.FinancialAnalysisDB))
            {
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_GetNextDebitorNumber] AS BEGIN SET NOCOUNT ON; SELECT MAX(AccountNumber) " +
                    $"FROM {TableName} " +
                    $"WHERE AccountNumber >= 10000 AND AccountNumber < 70000 END");
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
