using Dapper;
using FinancialAnalysis.Datalayer.StoredProcedures;
using FinancialAnalysis.Models.Accounting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FinancialAnalysis.Datalayer.Tables
{
    public class Debits : ITable
    {
        public string TableName { get; }
        private DebitsStoredProcedures sp = new DebitsStoredProcedures();

        public Debits()
        {
            TableName = "Debits";
            CheckAndCreateTable();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs\\Tables.txt", rollingInterval: RollingInterval.Month)
                .CreateLogger();
        }

        public void CheckAndCreateTable()
        {
            try
            {
                SqlConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr = $"If not exists (select name from sysobjects where name = '{TableName}') CREATE TABLE {TableName}(" +
                                 $"DebitId int IDENTITY(1,1) PRIMARY KEY," +
                                 $"Amount money NOT NULL," +
                                 $"RefBookingId int NOT NULL," +
                                 $"RefCostAccountId int NOT NULL)";

                using (SqlCommand command = new SqlCommand(commandStr, con))
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

        public void CheckAndCreateStoredProcedures()
        {
            sp.CheckAndCreateProcedures();
        }

        /// <summary>
        /// Returns all Debit records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Debit> GetAll()
        {
            IEnumerable<Debit> output = new List<Debit>();
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<Debit>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }
            return output;
        }

        /// <summary>
        /// Inserts the Debit item
        /// </summary>
        /// <param name="debit"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(Debit debit)
        {
            int id = 0;
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Amount, @RefCostAccountId, @RefBookingId", debit);
                    id = result.Single();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }
            return id;
        }

        /// <summary>
        /// Inserts the list of Debits items
        /// </summary>
        /// <param name="creditor"></param>
        public void Insert(IEnumerable<Debit> debits)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var debit in debits)
                    {
                        Insert(debit);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        /// Returns Debit by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Debit GetById(int id)
        {
            Debit output = new Debit();
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<Debit>($"dbo.{TableName}_GetById @DebitId", new { DebitId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }
            return output;
        }

        public void AddReferences()
        {
            AddCostAccountsReference();
            AddBookingsReference();
        }

        private void AddCostAccountsReference()
        {
            try
            {
                SqlConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr = $"IF(OBJECT_ID('FK_Debits_CostAccounts', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_Debits_CostAccounts FOREIGN KEY(RefCostAccountId) REFERENCES CostAccounts(CostAccountId)";

                using (SqlCommand command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and CostAccounts", e);
            }
        }

        private void AddBookingsReference()
        {
            try
            {
                SqlConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr = $"IF(OBJECT_ID('FK_Debits_Bookings', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_Debits_Booking FOREIGN KEY(RefBookingId) REFERENCES Bookings(BookingId)";

                using (SqlCommand command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and Bookings", e);
            }
        }
    }
}


