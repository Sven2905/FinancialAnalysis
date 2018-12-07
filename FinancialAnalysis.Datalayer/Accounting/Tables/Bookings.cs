using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models.Accounting;
using Serilog;
using Slapper;
using Utilities;

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
            IEnumerable<Booking> output = new SvenTechCollection<Booking>();
            try
            {
                using (var conn = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    conn.Open();
                    dynamic dynamicItem = conn.Query<dynamic>($"dbo.{TableName}_GetAll");
                    AutoMapper.Configuration.AddIdentifiers(typeof(Booking), new List<string> {"BookingId"});
                    AutoMapper.Configuration.AddIdentifiers(typeof(ScannedDocument),
                        new List<string> {"ScannedDocuments_ScannedDocumentId"});
                    AutoMapper.Configuration.AddIdentifiers(typeof(Credit), new List<string> {"Credits_CreditId"});
                    AutoMapper.Configuration.AddIdentifiers(typeof(Debit), new List<string> {"Debits_DebitId"});
                    conn.Close();

                    output = (AutoMapper.MapDynamic<Booking>(dynamicItem) as IEnumerable<Booking>).ToList();
                }

                return output;
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
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
        ///     Returns CostAccount by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Booking GetById(int id)
        {
            var output = new Booking();
            try
            {
                using (var conn = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    conn.Open();
                    dynamic dynamicItem =
                        conn.Query<dynamic>($"dbo.{TableName}_GetById @BookingId", new {BookingId = id});
                    AutoMapper.Configuration.AddIdentifiers(typeof(Booking), new List<string> {"BookingId"});
                    AutoMapper.Configuration.AddIdentifiers(typeof(ScannedDocument),
                        new List<string> {"ScannedDocuments_ScannedDocumentId"});
                    AutoMapper.Configuration.AddIdentifiers(typeof(Credit), new List<string> {"Credits_CreditId"});
                    AutoMapper.Configuration.AddIdentifiers(typeof(Debit), new List<string> {"Debits_DebitId"});
                    conn.Close();

                    var outputs = (AutoMapper.MapDynamic<Booking>(dynamicItem) as IEnumerable<Booking>).ToList();
                    if (outputs.Count == 1) output = outputs[0];
                }

                return output;
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
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
            IEnumerable<Booking> output = new SvenTechCollection<Booking>();
            try
            {
                using (var conn = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    conn.Open();
                    dynamic dynamicItem =
                        conn.Query<dynamic>(
                            $"dbo.{TableName}_GetByConditions @StartDate, @EndDate, @CreditId, @DebitId",
                            new {StartDate = startDate, EndDate = endDate, CreditId = creditId, DebitId = debitId});
                    AutoMapper.Configuration.AddIdentifiers(typeof(Booking), new List<string> {"BookingId"});
                    AutoMapper.Configuration.AddIdentifiers(typeof(ScannedDocument),
                        new List<string> {"ScannedDocuments_ScannedDocumentId"});
                    AutoMapper.Configuration.AddIdentifiers(typeof(Credit), new List<string> {"Credits_CreditId"});
                    AutoMapper.Configuration.AddIdentifiers(typeof(Debit), new List<string> {"Debits_DebitId"});
                    conn.Close();

                    output = (AutoMapper.MapDynamic<Booking>(dynamicItem) as IEnumerable<Booking>).ToList();
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