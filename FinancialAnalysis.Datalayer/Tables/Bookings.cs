using Dapper;
using FinancialAnalysis.Datalayer.StoredProcedures;
using FinancialAnalysis.Models.Accounting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Utilities;

namespace FinancialAnalysis.Datalayer.Tables
{
    public class Bookings : ITable
    {
        public string TableName { get; }
        private BookingsStoredProcedures sp = new BookingsStoredProcedures();

        public Bookings()
        {
            TableName = "Bookings";
            CheckAndCreateTable();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs\\Tables.txt", rollingInterval: RollingInterval.Month)
                .CreateLogger();
        }

        public void CheckAndCreateStoredProcedures()
        {
            sp.CheckAndCreateProcedures();
        }

        public void CheckAndCreateTable()
        {
            try
            {
                SqlConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr = $"If not exists (select name from sysobjects where name = '{TableName}') " +
                    $"CREATE TABLE {TableName}(BookingId int IDENTITY(1,1) PRIMARY KEY, Description nvarchar(150) NOT NULL, Amount money, Date datetime )";

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

        /// <summary>
        /// Returns all Booking records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Booking> GetAll()
        {
            IEnumerable<Booking> output = new List<Booking>();
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<Booking, SvenTechCollection<Credit>, SvenTechCollection<Debit>, SvenTechCollection<ScannedDocument>, Booking>($"dbo.{TableName}_GetAll",
                    (booking, credits, debits, documents) => { booking.Credits = credits; booking.Debits = debits; booking.ScannedDocuments = documents; return booking; }, splitOn: "CreditId, DebitId, ScannedDocumentId",
                    commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }
            return output;
        }

        /// <summary>
        /// Inserts the CostAccount item
        /// </summary>
        /// <param name="Booking"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(Booking Booking)
        {
            int id = 0;
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Description, @Amount, @Date ", Booking);
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
        /// Inserts the list of CostAccount items
        /// </summary>
        /// <param name="Bookings"></param>
        public void Insert(IEnumerable<Booking> Bookings)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var Booking in Bookings)
                    {
                        Insert(Booking);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }
        }

        /// <summary>
        /// Returns CostAccount by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Booking GetById(int id)
        {
            Booking output = new Booking();
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<Booking>($"dbo.{TableName}_GetById @BookingId", new { BookingId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }
            return output;
        }
    }
}
