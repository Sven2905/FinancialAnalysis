using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.Product
{
    public class ProductPrototypesProcedures : IStoredProcedures
    {
        public string TableName { get; }

        public ProductPrototypesProcedures()
        {
            TableName = "ProductPrototypes";
        }

        /// <summary>
        /// Check if all Stored Procedures are created, otherwise create them
        /// </summary>
        public void CheckAndCreateProcedures()
        {
            InsertData();
            GetAllData();
            GetById();
        }

        private void GetAllData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAll", DatabaseNames.FinancialAnalysisDB))
            {
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_GetAll] AS BEGIN SET NOCOUNT ON; " +
                    $"SELECT ProductPrototypeId, " +
                    $"Name, " +
                    $"Description, " +
                    $"DimensionX " +
                    $"DimensionY " +
                    $"DimensionZ " +
                    $"Weight " +
                    $"IsStackable " +
                    $"RefProductCategory " +
                    $"FROM {TableName} " +
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

        private void InsertData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_Insert", DatabaseNames.FinancialAnalysisDB))
            {
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_Insert] @Name nvarchar(150), @Description nvarchar(150), @DimensionX decimal, @DimensionY decimal, @DimensionZ decimal, @Weight decimal, @IsStackable bit, @RefProductCategory int AS BEGIN SET NOCOUNT ON; " +
                                $"INSERT into {TableName} (Name, Description, DimensionX, DimensionY, DimensionZ, Weight, IsStackable, RefProductCategory) " +
                                $"VALUES (@Name, @Description, @DimensionX, @DimensionY, @DimensionZ, @Weight, @IsStackable, @RefProductCategory); " +
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
                    $"CREATE PROCEDURE [{TableName}_GetById] @ProductPrototypeId int AS BEGIN SET NOCOUNT ON; SELECT Name, Description, DimensionX, DimensionY, DimensionZ, Weight, IsStackable, RefProductCategory " +
                    $"FROM {TableName} " +
                    $"WHERE ProductPrototypeId = @ProductPrototypeId END");
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
