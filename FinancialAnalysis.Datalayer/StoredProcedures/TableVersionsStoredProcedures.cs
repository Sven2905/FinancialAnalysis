using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.StoredProcedures
{
    internal class TableVersionsStoredProcedures : IStoredProcedures
    {
        public string TableName { get; }

        public TableVersionsStoredProcedures()
        {
            TableName = "TableVersions";
        }

        public void CheckAndCreateProcedures()
        {
            GetAllData();
            InsertData();
        }

        private void GetAllData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAll", DatabaseNames.FinancialAnalysisDB))
            {
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_GetAll] AS BEGIN SET NOCOUNT ON; SELECT TableVersionId, Name, Version, LastModified FROM {TableName} END");
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

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_Insert] @Name nvarchar(50), @Version int, @LastModified datetime AS BEGIN SET NOCOUNT ON; INSERT into {TableName} (Name, Version, LastModified) VALUES (@Name,@Version,@LastModified) END");
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
    }
}
