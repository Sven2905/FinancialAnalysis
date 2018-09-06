using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Datalayer.StoredProcedures;
using FinancialAnalysis.Models.Models;

namespace FinancialAnalysis.Datalayer.Tables
{
    public class TableVersions : ITable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        TableVersionsStoredProcedures sp = new TableVersionsStoredProcedures();

        public string TableName { get; }

        public TableVersions()
        {
            TableName = "TableVersions";
            CheckAndCreateTable();
        }

        public void CheckAndCreateStoredProcedures()
        {
            sp.CheckAndCreateProcedures();
        }

        /// <summary>
        /// Checks if table exists otherwise creates it
        /// </summary>
        public void CheckAndCreateTable()
        {
            try
            {
                SqlConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr = $"If not exists (select name from sysobjects where name = '{TableName}') CREATE TABLE {TableName}(Id int IDENTITY(1,1) PRIMARY KEY,Name char(50) NOT NULL,Version int NOT NULL,LastModified datetime NOT NULL)";

                using (SqlCommand command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }

                log.Info($"Table '{TableName}' created successfully...");
            }
            catch (Exception e)
            {
                log.Error($"Exception occured while creating table '{TableName}'",e);
            }
        }

        /// <summary>
        /// Returns all TableVersion records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TableVersion> GetAllTaxTypes()
        {
            using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                var output = con.Query<TableVersion>("dbo.TableVersions_GetAll");
                return output;
            }
        }

        /// <summary>
        /// Inserts the list of TableVersion items
        /// </summary>
        /// <param name="tableVersions"></param>
        public void Insert(IEnumerable<TableVersion> tableVersions)
        {
            using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                con.Execute("dbo.TableVersions_Insert @Name, @Version, @LastModified", tableVersions);
            }
        }
    }
}
