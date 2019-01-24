using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models.PurchaseManagement;
using Serilog;

namespace FinancialAnalysis.Datalayer.PurchaseManagement
{
    public class GoodsReceivedNotes : ITable
    {
        private readonly GoodsReceivedNotesStoredProcedures sp = new GoodsReceivedNotesStoredProcedures();

        public GoodsReceivedNotes()
        {
            TableName = "GoodsReceivedNotes";
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
                                 "(GoodsReceivedNoteId int IDENTITY(1,1) PRIMARY KEY, " +
                                 "RefPurchaseOrderId int NOT NULL, " +
                                 "Content varbinary(MAX) NOT NULL)";

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
        ///     Returns all GoodsReceivedNote records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GoodsReceivedNote> GetAll()
        {
            IEnumerable<GoodsReceivedNote> output = new List<GoodsReceivedNote>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<GoodsReceivedNote>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the GoodsReceivedNote item
        /// </summary>
        /// <param name="GoodsReceivedNote"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(GoodsReceivedNote GoodsReceivedNote)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @RefPurchaseOrderId, @Content ",
                        GoodsReceivedNote);
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
        ///     Inserts the list of GoodsReceivedNote items
        /// </summary>
        /// <param name="GoodsReceivedNotes"></param>
        public void Insert(IEnumerable<GoodsReceivedNote> GoodsReceivedNotes)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var GoodsReceivedNote in GoodsReceivedNotes) Insert(GoodsReceivedNote);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns GoodsReceivedNote by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GoodsReceivedNote GetById(int id)
        {
            var output = new GoodsReceivedNote();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<GoodsReceivedNote>(
                        $"dbo.{TableName}_GetById @GoodsReceivedNoteId", new {GoodsReceivedNoteId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update GoodsReceivedNote, if not exist, insert it
        /// </summary>
        /// <param name="GoodsReceivedNote"></param>
        public void UpdateOrInsert(GoodsReceivedNote GoodsReceivedNote)
        {
            if (GoodsReceivedNote.GoodsReceivedNoteId == 0 ||
                GetById(GoodsReceivedNote.GoodsReceivedNoteId) is null)
            {
                Insert(GoodsReceivedNote);
                return;
            }

            Update(GoodsReceivedNote);
        }

        /// <summary>
        ///     Update GoodsReceivedNotes, if not exist insert them
        /// </summary>
        /// <param name="GoodsReceivedNotes"></param>
        public void UpdateOrInsert(IEnumerable<GoodsReceivedNote> GoodsReceivedNotes)
        {
            foreach (var GoodsReceivedNote in GoodsReceivedNotes) UpdateOrInsert(GoodsReceivedNote);
        }

        /// <summary>
        ///     Update GoodsReceivedNote
        /// </summary>
        /// <param name="GoodsReceivedNote"></param>
        public void Update(GoodsReceivedNote GoodsReceivedNote)
        {
            if (GoodsReceivedNote.GoodsReceivedNoteId == 0 ||
                GetById(GoodsReceivedNote.GoodsReceivedNoteId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @GoodsReceivedNoteId, @RefPurchaseOrderId, @Content ",
                        GoodsReceivedNote);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete GoodsReceivedNote by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @GoodsReceivedNoteId", new {GoodsReceivedNoteId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete GoodsReceivedNote by Item
        /// </summary>
        /// <param name="id"></param>
        public void Delete(GoodsReceivedNote GoodsReceivedNote)
        {
            Delete(GoodsReceivedNote.GoodsReceivedNoteId);
        }

        public void AddReferences()
        {
            AddPurchaseTypesReference();
        }

        private void AddPurchaseTypesReference()
        {
            string refTable = "PurchaseOrders";

            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_{refTable}', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_{refTable} FOREIGN KEY(RefPurchaseOrderId) REFERENCES {refTable}(PurchaseOrderId) ON DELETE CASCADE";

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