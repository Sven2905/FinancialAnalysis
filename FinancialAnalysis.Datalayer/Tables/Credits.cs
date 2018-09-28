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
    public class Credits : ITable
    {
        public string TableName { get; }
        private CreditsStoredProcedures sp = new CreditsStoredProcedures();

        public Credits()
        {
            TableName = "Credits";
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
                                 $"CreditId int IDENTITY(1,1) PRIMARY KEY," +
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
        /// Returns all Credit records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Credit> GetAll()
        {
            IEnumerable<Credit> output = new List<Credit>();
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<Credit>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }
            return output;
        }

        /// <summary>
        /// Inserts the Credit item
        /// </summary>
        /// <param name="Credit"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(Credit Credit)
        {
            int id = 0;
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Amount, @RefCostAccountId, @RefBookingId", Credit);
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
        /// Inserts the list of Credits items
        /// </summary>
        /// <param name="creditor"></param>
        public void Insert(IEnumerable<Credit> Credits)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var Credit in Credits)
                    {
                        Insert(Credit);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        /// Returns Credit by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Credit GetById(int id)
        {
            Credit output = new Credit();
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<Credit>($"dbo.{TableName}_GetById @CreditId", new { CreditId = id });
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
                var commandStr = $"IF(OBJECT_ID('FK_Credits_CostAccounts', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_Credits_CostAccounts FOREIGN KEY(RefCostAccountId) REFERENCES CostAccounts(CostAccountId)";

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
                var commandStr = $"IF(OBJECT_ID('FK_Credits_Bookings', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_Credits_Booking FOREIGN KEY(RefBookingId) REFERENCES Bookings(BookingId)";

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