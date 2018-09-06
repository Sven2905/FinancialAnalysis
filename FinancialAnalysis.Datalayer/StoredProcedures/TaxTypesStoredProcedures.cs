using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.StoredProcedures
{
    internal class TaxTypesStoredProcedures : IStoredProcedure
    {
        public string TableName { get; }

        public TaxTypesStoredProcedures()
        {
            TableName = "TaxTypes";
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
        }

        private void GetAllData()
        {
            if (!Helper.StoredProcedureExists("dbo.TaxTypes_GetAll", DatabaseNames.FinancialAnalysisDB))
            {
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [TaxTypes_GetAll] AS BEGIN SET NOCOUNT ON; SELECT Id, Name, AmountOfTax FROM {TableName} END");
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
            if (!Helper.StoredProcedureExists("dbo.TaxTypes_Insert", DatabaseNames.FinancialAnalysisDB))
            {
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [TaxTypes_Insert] @Name nvarchar(50), @AmountOfTax decimal AS BEGIN SET NOCOUNT ON; INSERT into {TableName} (Name,AmountOfTax) VALUES (@Name,@AmountOfTax); SELECT CAST(SCOPE_IDENTITY() as int) END");
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
            if (!Helper.StoredProcedureExists("dbo.TaxTypes_GetById", DatabaseNames.FinancialAnalysisDB))
            {
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [TaxTypes_GetById] @Id int AS BEGIN SET NOCOUNT ON; SELECT Id, Name, AmountOfTax FROM {TableName} WHERE Id = @Id END");
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
            if (!Helper.StoredProcedureExists("dbo.TaxTypes_Update", DatabaseNames.FinancialAnalysisDB))
            {
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [TaxTypes_Update] @Id int, @Name nvarchar(50), @AmountOfTax decimal AS BEGIN SET NOCOUNT ON; UPDATE {TableName} SET Name = @Name, AmountOfTax = @AmountOfTax WHERE Id = @Id END");
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
            if (!Helper.StoredProcedureExists("dbo.TaxTypes_Delete", DatabaseNames.FinancialAnalysisDB))
            {
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [TaxTypes_Delete] @Id int AS BEGIN SET NOCOUNT ON; DELETE FROM {TableName} WHERE Id = @Id END");
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
