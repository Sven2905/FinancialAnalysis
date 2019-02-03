using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using FinancialAnalysis.Models;

namespace FinancialAnalysis.Datalayer
{
    public static class Helper
    {
        public static string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        /// <summary>
        ///     Checks if the StoredProcedure exist
        /// </summary>
        /// <param name="sp">Name of stored procedure</param>
        /// <param name="connectionString">Name of connection string</param>
        /// <returns></returns>
        public static bool StoredProcedureExists(string sp, string dbName)
        {
            using (var con = new SqlConnection(GetConnectionString(dbName)))
            {
                con.Open();
                using (var cmd = new SqlCommand($"select object_id('{sp}')", con))
                {
                    return !string.IsNullOrWhiteSpace(cmd.ExecuteScalar().ToString());
                }
            }
        }

        public static bool CheckIfTableExists(string tableName, string dbName)
        {
            var connection = new SqlConnection(GetConnectionString(dbName));
            var cmd = new SqlCommand(@"IF EXISTS(
                SELECT 1 FROM INFORMATION_SCHEMA.TABLES 
                WHERE TABLE_NAME = @table) 
                SELECT 1 ELSE SELECT 0", connection);

            cmd.Parameters.Add("@table", SqlDbType.NVarChar).Value = tableName;
            var exists = (int) cmd.ExecuteScalar();
            if (exists == 1)
                return true;
            return false;
        }

        public static TableVersion GetTableVersion(string tableName)
        {
                var tableVersion = DataContext.Instance.TableVersions.GetAll().SingleOrDefault(x => x.Name == tableName);
                return tableVersion;
        }
    }
}