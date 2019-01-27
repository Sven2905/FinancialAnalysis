using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.CompanyManagement;
using FinancialAnalysis.Models.ProductManagement;
using FinancialAnalysis.Models.PurchaseManagement;
using Serilog;
using Utilities;

namespace FinancialAnalysis.Datalayer.PurchaseManagement
{
    public class PurchaseOrders : ITable
    {
        private readonly PurchaseOrdersStoredProcedures sp = new PurchaseOrdersStoredProcedures();

        public PurchaseOrders()
        {
            TableName = "PurchaseOrders";
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
                                 "(PurchaseOrderId int IDENTITY(1,1) PRIMARY KEY, " +
                                 "PurchaseInvoiceNumber nvarchar(150), " +
                                 "RefCreditorId int, " +
                                 "OrderDate datetime, " +
                                 "DeliveryDate datetime, " +
                                 "RefPurchaseTypeId int, " +
                                 "Remarks nvarchar(150), " +
                                 "IsClosed bit)";

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
        ///     Returns all PurchaseOrder records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PurchaseOrder> GetAll()
        {
            var PurchaseOrderDictionary = new Dictionary<int, PurchaseOrder>();
            using (var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                var query = con.Query<PurchaseOrder, PurchaseType, Bill, GoodsReceivedNote, Creditor, PurchaseOrderPosition, Product, PurchaseOrder>
                    ($"dbo.{TableName}_GetAll",
                        (po, pt, b, grn, c, pop, p) =>
                        {
                            PurchaseOrder PurchaseOrderEntry;

                            if (!PurchaseOrderDictionary.TryGetValue(po.PurchaseOrderId, out PurchaseOrderEntry))
                            {
                                PurchaseOrderEntry = po;
                                PurchaseOrderEntry.PurchaseOrderPositions = new SvenTechCollection<PurchaseOrderPosition>();
                                PurchaseOrderEntry.Bills = new SvenTechCollection<Bill>();
                                PurchaseOrderEntry.GoodsReceivedNotes = new SvenTechCollection<GoodsReceivedNote>();
                                PurchaseOrderEntry.PurchaseType = pt;
                                PurchaseOrderEntry.Creditor = c;
                                PurchaseOrderDictionary.Add(PurchaseOrderEntry.PurchaseOrderId, PurchaseOrderEntry);
                            }

                            PurchaseOrderEntry.Bills.Add(b);
                            PurchaseOrderEntry.GoodsReceivedNotes.Add(grn);
                            p.TaxType = DataLayer.Instance.TaxTypes.GetById(p.RefTaxTypeId);
                            pop.Product = p;
                            PurchaseOrderEntry.PurchaseOrderPositions.Add(pop);

                            return po;
                        }, splitOn: "PurchaseOrderId, PurchaseTypeId, BillId, GoodsReceivedNoteId, CreditorId, PurchaseOrderPositionId, ProductId")
                    .AsQueryable();
                return PurchaseOrderDictionary.Values.ToList();
            }
        }

        /// <summary>
        ///     Inserts the PurchaseOrder item
        /// </summary>
        /// <param name="PurchaseOrder"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(PurchaseOrder PurchaseOrder)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @PurchaseInvoiceNumber, @RefCreditorId, @OrderDate, @DeliveryDate, @RefPurchaseTypeId, @Remarks, @IsClosed ",
                        PurchaseOrder);
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
        ///     Inserts the list of PurchaseOrder items
        /// </summary>
        /// <param name="PurchaseOrders"></param>
        public void Insert(IEnumerable<PurchaseOrder> PurchaseOrders)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var PurchaseOrder in PurchaseOrders) Insert(PurchaseOrder);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns PurchaseOrder by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PurchaseOrder GetById(int id)
        {
            var PurchaseOrderDictionary = new Dictionary<int, PurchaseOrder>();
            using (var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                var query = con.Query<PurchaseOrder, PurchaseType, Bill, GoodsReceivedNote, Creditor, PurchaseOrderPosition, Product, PurchaseOrder>
                    ($"dbo.{TableName}_GetById @PurchaseOrderId",
                        (po, pt, b, grn, c, pop, p) =>
                        {
                            PurchaseOrder PurchaseOrderEntry;

                            if (!PurchaseOrderDictionary.TryGetValue(po.PurchaseOrderId, out PurchaseOrderEntry))
                            {
                                PurchaseOrderEntry = po;
                                PurchaseOrderEntry.PurchaseOrderPositions = new SvenTechCollection<PurchaseOrderPosition>();
                                PurchaseOrderEntry.Bills = new SvenTechCollection<Bill>();
                                PurchaseOrderEntry.GoodsReceivedNotes = new SvenTechCollection<GoodsReceivedNote>();
                                PurchaseOrderEntry.PurchaseType = pt;
                                PurchaseOrderEntry.Creditor = c;
                                PurchaseOrderDictionary.Add(PurchaseOrderEntry.PurchaseOrderId, PurchaseOrderEntry);
                            }

                            PurchaseOrderEntry.Bills.Add(b);
                            PurchaseOrderEntry.GoodsReceivedNotes.Add(grn);
                            p.TaxType = DataLayer.Instance.TaxTypes.GetById(p.RefTaxTypeId);
                            pop.Product = p;
                            PurchaseOrderEntry.PurchaseOrderPositions.Add(pop);

                            return po;
                        }, new { PurchaseOrderId = id }, splitOn: "PurchaseOrderId, PurchaseTypeId, BillId, GoodsReceivedNoteId, CreditorId, PurchaseOrderPositionId, ProductId")
                    .AsQueryable();
                return PurchaseOrderDictionary.Values.FirstOrDefault();
            }
        }

        /// <summary>
        ///     Update PurchaseOrder, if not exist, insert it
        /// </summary>
        /// <param name="PurchaseOrder"></param>
        public void UpdateOrInsert(PurchaseOrder PurchaseOrder)
        {
            if (PurchaseOrder.PurchaseOrderId == 0 ||
                GetById(PurchaseOrder.PurchaseOrderId) is null)
            {
                Insert(PurchaseOrder);
                return;
            }

            Update(PurchaseOrder);
        }

        /// <summary>
        ///     Update PurchaseOrders, if not exist insert them
        /// </summary>
        /// <param name="PurchaseOrders"></param>
        public void UpdateOrInsert(IEnumerable<PurchaseOrder> PurchaseOrders)
        {
            foreach (var PurchaseOrder in PurchaseOrders) UpdateOrInsert(PurchaseOrder);
        }

        /// <summary>
        ///     Update PurchaseOrder
        /// </summary>
        /// <param name="PurchaseOrder"></param>
        public void Update(PurchaseOrder PurchaseOrder)
        {
            if (PurchaseOrder.PurchaseOrderId == 0 ||
                GetById(PurchaseOrder.PurchaseOrderId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @PurchaseOrderId, @PurchaseInvoiceNumber, @RefCreditorId, @OrderDate, @DeliveryDate, @RefPurchaseTypeId, @Remarks, @IsClosed ",
                        PurchaseOrder);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete PurchaseOrder by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @PurchaseOrderId", new {PurchaseOrderId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete PurchaseOrder by Item
        /// </summary>
        /// <param name="id"></param>
        public void Delete(PurchaseOrder PurchaseOrder)
        {
            Delete(PurchaseOrder.PurchaseOrderId);
        }

        public void AddReferences()
        {
            AddPurchaseTypesReference();
            AddCreditorsReference();
        }

        private void AddPurchaseTypesReference()
        {
            string refTable = "PurchaseTypes";

            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_{refTable}', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_{refTable} FOREIGN KEY(RefPurchaseTypeId) REFERENCES {refTable}(PurchaseTypeId) ON DELETE CASCADE";

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

        private void AddCreditorsReference()
        {
            string refTable = "Creditors";

            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_{refTable}', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_{refTable} FOREIGN KEY(RefCreditorId) REFERENCES {refTable}(CreditorId) ON DELETE CASCADE";

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