using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.StoredProcedures
{
    internal class TableVersionsStoredProcedures : IStoredProcedure
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
            if (!Helper.StoredProcedureExists("dbo.TableVersions_GetAll", DatabaseNames.FinancialAnalysisDB))
            {
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [TableVersions_GetAll] AS BEGIN SET NOCOUNT ON; SELECT Id, Name, Version, LastModified FROM {TableName} END");
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

                sbSP.AppendLine($"CREATE PROCEDURE [TableVersions_Insert] @Name char(50), @Version int, @LastModified datetime AS BEGIN SET NOCOUNT ON; INSERT into {TableName} (Name, Version, LastModified) VALUES (@Name,@Version,@LastModified) END");
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
