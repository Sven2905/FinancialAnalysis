using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models.SalesManagement;
using Serilog;

namespace FinancialAnalysis.Datalayer.SalesManagement
{
    public class InvoicePositions : ITable
    {
        private readonly InvoicePositionsStoredProcedures sp = new InvoicePositionsStoredProcedures();

        public InvoicePositions()
        {
            TableName = "InvoicePositions";
            CheckAndCreateTable();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs\\Tables.txt", rollingInterval: RollingInterval.Month)
                .CreateLogger();
        }

        public string TableName { get; }

        public void CheckAndCreateStoredProcedures()
        {
            sp.CheckAndCreateProcedures();
        }

        public void CheckAndCreateTable()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr = $"If not exists (select name from sysobjects where name = '{TableName}') " +
                                 $"CREATE TABLE {TableName}" +
                                 "(InvoicePositionId int IDENTITY(1,1) PRIMARY KEY, " +
                                 "RefInvoiceId int, " +
                                 "RefSalesOrderPositionId int, " +
                                 "Quantity int)";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Inserts the InvoicePosition item
        /// </summary>
        /// <param name="InvoicePosition"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(InvoicePosition InvoicePosition)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>(
                        $"dbo.{TableName}_Insert @RefInvoiceId, @RefSalesOrderPositionId, @Quantity ",
                        InvoicePosition);
                    return result.Single();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' from table '{TableName}'", e);
            }

            return id;
        }

        /// <summary>
        ///     Inserts the list of InvoicePosition items
        /// </summary>
        /// <param name="InvoicePositions"></param>
        public void Insert(IEnumerable<InvoicePosition> InvoicePositions)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var InvoicePosition in InvoicePositions) Insert(InvoicePosition);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Update InvoicePosition, if not exist, insert it
        /// </summary>
        /// <param name="InvoicePosition"></param>
        public void UpdateOrInsert(InvoicePosition InvoicePosition)
        {
            if (InvoicePosition.InvoicePositionId == 0)
            {
                Insert(InvoicePosition);
                return;
            }

            Update(InvoicePosition);
        }

        /// <summary>
        ///     Update InvoicePositions, if not exist insert them
        /// </summary>
        /// <param name="InvoicePositions"></param>
        public void UpdateOrInsert(IEnumerable<InvoicePosition> InvoicePositions)
        {
            foreach (var InvoicePosition in InvoicePositions) UpdateOrInsert(InvoicePosition);
        }

        /// <summary>
        ///     Update InvoicePosition
        /// </summary>
        /// <param name="InvoicePosition"></param>
        public void Update(InvoicePosition InvoicePosition)
        {
            if (InvoicePosition.InvoicePositionId == 0) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute(
                        $"dbo.{TableName}_Update @InvoicePositionId, @RefInvoiceId, @RefSalesOrderPositionId, @Quantity ",
                        InvoicePosition);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete InvoicePosition by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @InvoicePositionId", new {InvoicePositionId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete InvoicePosition by Item
        /// </summary>
        /// <param name="id"></param>
        public void Delete(InvoicePosition InvoicePosition)
        {
            Delete(InvoicePosition.InvoicePositionId);
        }

        public void AddReferences()
        {
            AddSalesOrderPositionsReference();
            AddInvoicesReference();
        }

        private void AddSalesOrderPositionsReference()
        {
            const string refTable = "SalesOrderPositions";

            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_{refTable}', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_{refTable} FOREIGN KEY(RefSalesOrderPositionId) REFERENCES {refTable}(SalesOrderPositionId) ON DELETE CASCADE";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and {refTable}",
                    e);
            }
        }

        private void AddInvoicesReference()
        {
            const string refTable = "Invoices";

            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_{refTable}', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_{refTable} FOREIGN KEY(RefInvoiceId) REFERENCES {refTable}(InvoiceId) ON DELETE NO ACTION";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and {refTable}",
                    e);
            }
        }
    }
}