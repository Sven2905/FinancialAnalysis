using Dapper;
using FinancialAnalysis.Models.Accounting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Datalayer.Accounting
{
    public class ScannedDocuments : ITable
    {
        public string TableName { get; }
        private ScannedDocumentsStoredProcedures sp = new ScannedDocumentsStoredProcedures();

        public ScannedDocuments()
        {
            TableName = "ScannedDocuments";
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
                    $"CREATE TABLE {TableName}(ScannedDocumentId int IDENTITY(1,1) PRIMARY KEY, Content varbinary(MAX), FileName nvarchar(150) NOT NULL, Date datetime NOT NULL, RefBookingId int NOT NULL)";

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
        /// Returns all ScannedDocument records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ScannedDocument> GetAll()
        {
            IEnumerable<ScannedDocument> output = new List<ScannedDocument>();
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<ScannedDocument>($"dbo.{TableName}_GetAll");
                }

            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }
            return output;
        }

        /// <summary>
        /// Inserts the ScannedDocument item
        /// </summary>
        /// <param name="scannedDocument"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(ScannedDocument scannedDocument)
        {
            int id = 0;
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Content, @FileName, @Date, @RefBookingId", scannedDocument);
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
        /// Inserts the list of CostAccountCategory items
        /// </summary>
        /// <param name="scannedDocuments"></param>
        public void Insert(IEnumerable<ScannedDocument> scannedDocuments)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var scannedDocument in scannedDocuments)
                    {
                        Insert(scannedDocument);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }
        }

        /// <summary>
        /// Returns ScannedDocument by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ScannedDocument GetById(int id)
        {
            ScannedDocument output = new ScannedDocument();
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<ScannedDocument>($"dbo.{TableName}_GetById @ScannedDocumentId", new { ScannedDocumentId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }
            return output;
        }

        /// <summary>
        /// Update ScannedDocument, if not exist, insert it
        /// </summary>
        /// <param name="scannedDocument"></param>
        public void UpdateOrInsert(ScannedDocument scannedDocument)
        {
            if (scannedDocument.ScannedDocumentId == 0 || GetById(scannedDocument.ScannedDocumentId) is null)
            {
                Insert(scannedDocument);
                return;
            }

            Update(scannedDocument);
        }

        /// <summary>
        /// Update ScannedDocuments, if not exist insert them
        /// </summary>
        /// <param name="scannedDocuments"></param>
        public void UpdateOrInsert(IEnumerable<ScannedDocument> scannedDocuments)
        {
            foreach (var scannedDocument in scannedDocuments)
            {
                UpdateOrInsert(scannedDocument);
            }
        }

        /// <summary>
        /// Update CostAccountCategory
        /// </summary>
        /// <param name="scannedDocument"></param>
        public void Update(ScannedDocument scannedDocument)
        {
            if (scannedDocument.ScannedDocumentId == 0 || GetById(scannedDocument.ScannedDocumentId) is null)
            {
                return;
            }

            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @ScannedDocumentId, @Content, @FileName, @Date, @RefBookingId", scannedDocument);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        /// Delete CostAccountCategory by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @ScannedDocumentId", new { ScannedDocumentId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        /// <summary>
        /// Delete CostAccountCategory by Item
        /// </summary>
        /// <param name="id"></param>
        public void Delete(CostAccountCategory costAccountCategory)
        {
            Delete(costAccountCategory.CostAccountCategoryId);
        }

        public void AddReferences()
        {
            AddBookingsReference();
        }

        private void AddBookingsReference()
        {
            try
            {
                SqlConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr = $"IF(OBJECT_ID('FK_ScannedDocuments_Bookings', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_ScannedDocuments_Booking FOREIGN KEY(RefBookingId) REFERENCES Bookings(BookingId)";

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
