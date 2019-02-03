using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models.Accounting;
using Serilog;

namespace FinancialAnalysis.Datalayer.Accounting
{
    public class Bookings : ITable
    {
        private readonly BookingsStoredProcedures sp = new BookingsStoredProcedures();

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
                                 "(BookingId int IDENTITY(1,1) PRIMARY KEY, " +
                                 "Description nvarchar(150) NOT NULL, " +
                                 "Amount money, " +
                                 "RefCostCenterId int, " +
                                 "Date datetime )";

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
        ///     Returns all Booking records.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Booking> GetAll()
        {
            var bookingDictionary = new Dictionary<int, Booking>();
            using (var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                var query = con.Query<Booking, ScannedDocument, Credit, Debit, Booking>
                    ($"dbo.{TableName}_GetAll",
                        (b, s, c, d) =>
                        {
                            Booking bookingEntry;

                            if (!bookingDictionary.TryGetValue(b.BookingId, out bookingEntry))
                            {
                                bookingEntry = b;
                                bookingEntry.Credits = new List<Credit>();
                                bookingEntry.Debits = new List<Debit>();
                                bookingEntry.ScannedDocuments = new List<ScannedDocument>();
                                bookingDictionary.Add(bookingEntry.BookingId, bookingEntry);
                            }

                            bookingEntry.Credits.Add(c);
                            bookingEntry.Debits.Add(d);
                            bookingEntry.ScannedDocuments.Add(s);

                            return b;
                        }, splitOn: "BookingId, ScannedDocumentId, CreditId, DebitId")
                    .AsQueryable();
                return query.ToList();
            }
        }

        /// <summary>
        ///     Inserts the CostAccount item.
        /// </summary>
        /// <param name="Booking"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(Booking Booking)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result =
                        con.Query<int>($"dbo.{TableName}_Insert @Description, @Amount, @RefCostCenterId, @Date",
                            Booking);
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
        ///     Inserts the list of CostAccount items.
        /// </summary>
        /// <param name="Bookings"></param>
        public void Insert(IEnumerable<Booking> Bookings)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var Booking in Bookings) Insert(Booking);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns Booking by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Booking GetById(int id)
        {
            var bookingDictionary = new Dictionary<int, Booking>();
            using (var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                var query = con.Query<Booking, ScannedDocument, Credit, Debit, Booking>
                    ($"dbo.{TableName}_GetById @BookingId",
                        (b, s, c, d) =>
                        {
                            Booking bookingEntry;

                            if (!bookingDictionary.TryGetValue(b.BookingId, out bookingEntry))
                            {
                                bookingEntry = b;
                                bookingEntry.Credits = new List<Credit>();
                                bookingEntry.Debits = new List<Debit>();
                                bookingEntry.ScannedDocuments = new List<ScannedDocument>();
                                bookingDictionary.Add(bookingEntry.BookingId, bookingEntry);
                            }

                            bookingEntry.Credits.Add(c);
                            bookingEntry.Debits.Add(d);
                            bookingEntry.ScannedDocuments.Add(s);

                            return b;
                        }, new {BookingId = id}, splitOn: "BookingId, ScannedDocumentId, CreditId, DebitId")
                    .AsQueryable();
                return query.FirstOrDefault();
            }
        }

        /// <summary>
        ///     Returns all Booking records with conditions.
        /// </summary>
        /// <param name="startDate">[incl.]</param>
        /// <param name="endDate">[incl.]</param>
        /// <param name="creditId"></param>
        /// <param name="debitId"></param>
        /// <returns></returns>
        public IEnumerable<Booking> GetByConditions(DateTime startDate, DateTime endDate, int? creditId = null,
            int? debitId = null)
        {
            var bookingDictionary = new Dictionary<int, Booking>();
            using (var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                var query = con.Query<Booking, ScannedDocument, Credit, Debit, Booking>
                    ($"dbo.{TableName}_GetByConditions @StartDate, @EndDate, @CreditId, @DebitId",
                        (b, s, c, d) =>
                        {
                            Booking bookingEntry;

                            if (!bookingDictionary.TryGetValue(b.BookingId, out bookingEntry))
                            {
                                bookingEntry = b;
                                bookingEntry.Credits = new List<Credit>();
                                bookingEntry.Debits = new List<Debit>();
                                bookingEntry.ScannedDocuments = new List<ScannedDocument>();
                                bookingDictionary.Add(bookingEntry.BookingId, bookingEntry);
                            }

                            bookingEntry.Credits.Add(c);
                            bookingEntry.Debits.Add(d);
                            bookingEntry.ScannedDocuments.Add(s);

                            return b;
                        }, new {StartDate = startDate, EndDate = endDate, CreditId = creditId, DebitId = debitId},
                        splitOn: "BookingId, ScannedDocumentId, CreditId, DebitId")
                    .AsQueryable();
                return query.ToList();
            }
        }

        public void AddReferences()
        {
            AddCostCentersReference();
        }

        private void AddCostCentersReference()
        {
            var refTable = "CostCenters";

            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_{refTable}', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_{refTable} FOREIGN KEY(RefCostCenterId) REFERENCES {refTable}(CostCenterId) ON DELETE CASCADE";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and {TableName}",
                    e);
            }
        }
    }
}