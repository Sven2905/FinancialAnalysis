using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.StoredProcedures
{
    internal class TaxTypesStoredProcedures : IStoredProcedures
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
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAll", DatabaseNames.FinancialAnalysisDB))
            {
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_GetAll] AS BEGIN SET NOCOUNT ON; SELECT * FROM {TableName} END");
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

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_Insert] @Description nvarchar(50), @DescriptionShort nvarchar(50), @AmountOfTax decimal, @TaxCategory int, @RefAccountNumber int, @RefAccountNotPayable int AS BEGIN SET NOCOUNT ON; " +
                                $"INSERT into {TableName} (Description, DescriptionShort, AmountOfTax, TaxCategory, RefAccountNumber, RefAccountNotPayable) " +
                                $"VALUES (@Description, @DescriptionShort, @AmountOfTax, @TaxCategory, @RefAccountNumber, @RefAccountNotPayable); " +
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
                    $"CREATE PROCEDURE [{TableName}_GetById] @TaxTypeId int AS BEGIN SET NOCOUNT ON; SELECT TaxTypeId, Description, DescriptionShort, AmountOfTax, TaxCategory, RefAccountNumber, RefAccountNotPayable " +
                    $"FROM {TableName} " +
                    $"WHERE TaxTypeId = @TaxTypeId END");
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
                    $"CREATE PROCEDURE [{TableName}_Update] @TaxTypeId int,  @Description nvarchar(50), @DescriptionShort nvarchar(50), @AmountOfTax decimal, @TaxCategory int, @RefAccountNumber int, @RefAccountNotPayable int " +
                    $"AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} " +
                    $"SET Description = @Description, @DescriptionShort = DescriptionShort, AmountOfTax = @AmountOfTax, @TaxCategory = TaxCategory, @RefAccountNumber = RefAccountNumber, @RefAccountNotPayable = RefAccountNotPayable " +
                    $"WHERE TaxTypeId = @TaxTypeId END");
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
                    $"CREATE PROCEDURE [{TableName}_Delete] @TaxTypeId int AS BEGIN SET NOCOUNT ON; DELETE FROM {TableName} WHERE TaxTypeId = @TaxTypeId END");
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
