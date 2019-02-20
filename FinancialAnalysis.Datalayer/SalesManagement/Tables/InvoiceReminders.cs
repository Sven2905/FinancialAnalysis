using Dapper;
using FinancialAnalysis.Models.SalesManagement;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FinancialAnalysis.Datalayer.SalesManagement
{
    public class InvoiceReminders : ITable
    {
        private readonly InvoiceRemindersStoredProcedures sp = new InvoiceRemindersStoredProcedures();

        public InvoiceReminders()
        {
            TableName = "InvoiceReminders";
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
                                 $"CREATE TABLE {TableName}(" +
                                 $"InvoiceReminderId int IDENTITY(1,1) PRIMARY KEY, " +
                                 "RefInvoiceId int, " +
                                 "Date datetime, " +
                                 "Username nvarchar(150), " +
                                 "ReminderType int, " +
                                 "IsLastReminder bit)";

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
        ///     Returns all InvoiceReminder records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<InvoiceReminder> GetAll()
        {
            IEnumerable<InvoiceReminder> output = new List<InvoiceReminder>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<InvoiceReminder>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the InvoiceReminder item
        /// </summary>
        /// <param name="InvoiceReminder"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(InvoiceReminder InvoiceReminder)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result =
                        con.Query<int>(
                            $"dbo.{TableName}_Insert @RefInvoiceId, @Date, @Username, @ReminderType, @IsLastReminder ",
                            InvoiceReminder);
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
        ///     Inserts the list of InvoiceReminder items
        /// </summary>
        /// <param name="InvoiceReminders"></param>
        public void Insert(IEnumerable<InvoiceReminder> InvoiceReminders)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var InvoiceReminder in InvoiceReminders)
                    {
                        Insert(InvoiceReminder);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns InvoiceReminder by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InvoiceReminder GetById(int id)
        {
            var output = new InvoiceReminder();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<InvoiceReminder>($"dbo.{TableName}_GetById @InvoiceReminderId",
                        new { InvoiceReminderId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update InvoiceReminder, if not exist, insert it
        /// </summary>
        /// <param name="InvoiceReminder"></param>
        public void UpdateOrInsert(InvoiceReminder InvoiceReminder)
        {
            if (InvoiceReminder.InvoiceReminderId == 0 || GetById(InvoiceReminder.InvoiceReminderId) is null)
            {
                Insert(InvoiceReminder);
                return;
            }

            Update(InvoiceReminder);
        }

        /// <summary>
        ///     Update InvoiceReminders, if not exist insert them
        /// </summary>
        /// <param name="InvoiceReminders"></param>
        public void UpdateOrInsert(IEnumerable<InvoiceReminder> InvoiceReminders)
        {
            foreach (var InvoiceReminder in InvoiceReminders)
            {
                UpdateOrInsert(InvoiceReminder);
            }
        }

        /// <summary>
        ///     Update InvoiceReminder
        /// </summary>
        /// <param name="InvoiceReminder"></param>
        public void Update(InvoiceReminder InvoiceReminder)
        {
            if (InvoiceReminder.InvoiceReminderId == 0 || GetById(InvoiceReminder.InvoiceReminderId) is null)
            {
                return;
            }

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute(
                        $"dbo.{TableName}_Update @InvoiceReminderId, @RefInvoiceId, @Date, @Username, @ReminderType, @IsLastReminder",
                        InvoiceReminder);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete InvoiceReminder by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @InvoiceReminderId", new { InvoiceReminderId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete InvoiceReminder by Item
        /// </summary>
        /// <param name="id"></param>
        public void Delete(InvoiceReminder InvoiceReminder)
        {
            Delete(InvoiceReminder.InvoiceReminderId);
        }

        public void AddReferences()
        {
            AddPaymentConditionsReference();
        }

        private void AddPaymentConditionsReference()
        {
            string refTable = "Invoices";

            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_{refTable}', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_{refTable} FOREIGN KEY(RefInvoiceId) REFERENCES {refTable}(InvoiceId) ON DELETE CASCADE";

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