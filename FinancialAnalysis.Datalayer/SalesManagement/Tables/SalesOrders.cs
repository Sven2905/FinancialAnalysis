using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.CompanyManagement;
using FinancialAnalysis.Models.ProductManagement;
using FinancialAnalysis.Models.SalesManagement;
using Serilog;
using Utilities;

namespace FinancialAnalysis.Datalayer.SalesManagement
{
    public class SalesOrders : ITable
    {
        private readonly SalesOrdersStoredProcedures sp = new SalesOrdersStoredProcedures();

        public SalesOrders()
        {
            TableName = "SalesOrders";
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
                                 "(SalesOrderId int IDENTITY(1,1) PRIMARY KEY, " +
                                 "RefDebitorId int, " +
                                 "OrderDate datetime, " +
                                 "RefSalesTypeId int, " +
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
        ///     Returns all SalesOrder records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SalesOrder> GetAll()
        {
            var SalesOrderDictionary = new Dictionary<int, SalesOrder>();
            using (var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                var query = con.Query<SalesOrder, SalesType, Invoice, Shipment, Debitor, SalesOrderPosition, Product, SalesOrder>
                    ($"dbo.{TableName}_GetAll",
                        (so, st, i, s, d, sop, p) =>
                        {
                            SalesOrder SalesOrderEntry;

                            if (!SalesOrderDictionary.TryGetValue(so.SalesOrderId, out SalesOrderEntry))
                            {
                                SalesOrderEntry = so;
                                SalesOrderEntry.SalesOrderPositions = new SvenTechCollection<SalesOrderPosition>();
                                SalesOrderEntry.Invoices = new SvenTechCollection<Invoice>();
                                SalesOrderEntry.Shipments = new SvenTechCollection<Shipment>();
                                SalesOrderEntry.SalesType = st;
                                SalesOrderEntry.Debitor = d;
                                SalesOrderDictionary.Add(SalesOrderEntry.SalesOrderId, SalesOrderEntry);
                            }

                            SalesOrderEntry.Invoices.Add(i);
                            SalesOrderEntry.Shipments.Add(s);
                            p.TaxType = DataLayer.Instance.TaxTypes.GetById(p.RefTaxTypeId);
                            sop.Product = p;
                            SalesOrderEntry.SalesOrderPositions.Add(sop);

                            return so;
                        }, splitOn: "SalesOrderId, SalesTypeId, InvoiceId, ShipmentId, DebitorId, SalesOrderPositionId, ProductId")
                    .AsQueryable();
                return SalesOrderDictionary.Values.ToList();
            }
        }

        /// <summary>
        ///     Inserts the SalesOrder item
        /// </summary>
        /// <param name="SalesOrder"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(SalesOrder SalesOrder)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @RefDebitorId, @OrderDate, @RefSalesTypeId, @Remarks, @IsClosed ",
                        SalesOrder);
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
        ///     Inserts the list of SalesOrder items
        /// </summary>
        /// <param name="SalesOrders"></param>
        public void Insert(IEnumerable<SalesOrder> SalesOrders)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var SalesOrder in SalesOrders) Insert(SalesOrder);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns SalesOrder by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SalesOrder GetById(int id)
        {
            var SalesOrderDictionary = new Dictionary<int, SalesOrder>();
            using (var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                var query = con.Query<SalesOrder, SalesType, Invoice, Shipment, Debitor, SalesOrderPosition, Product, SalesOrder>
                    ($"dbo.{TableName}_GetById @SalesOrderId",
                        (so, st, i, s, d, sop, p) =>
                        {
                            SalesOrder SalesOrderEntry;

                            if (!SalesOrderDictionary.TryGetValue(so.SalesOrderId, out SalesOrderEntry))
                            {
                                SalesOrderEntry = so;
                                SalesOrderEntry.SalesOrderPositions = new SvenTechCollection<SalesOrderPosition>();
                                SalesOrderEntry.Invoices = new SvenTechCollection<Invoice>();
                                SalesOrderEntry.Shipments = new SvenTechCollection<Shipment>();
                                SalesOrderEntry.SalesType = st;
                                SalesOrderEntry.Debitor = d;
                                SalesOrderDictionary.Add(SalesOrderEntry.SalesOrderId, SalesOrderEntry);
                            }

                            SalesOrderEntry.Invoices.Add(i);
                            SalesOrderEntry.Shipments.Add(s);
                            p.TaxType = DataLayer.Instance.TaxTypes.GetById(p.RefTaxTypeId);
                            sop.Product = p;
                            SalesOrderEntry.SalesOrderPositions.Add(sop);

                            return so;
                        }, new { SalesOrderId = id }, splitOn: "SalesOrderId, SalesTypeId, InvoiceId, ShipmentId, DebitorId, SalesOrderPositionId, ProductId")
                    .AsQueryable();
                return SalesOrderDictionary.Values.FirstOrDefault();
            }
        }

        /// <summary>
        ///     Update SalesOrder, if not exist, insert it
        /// </summary>
        /// <param name="SalesOrder"></param>
        public void UpdateOrInsert(SalesOrder SalesOrder)
        {
            if (SalesOrder.SalesOrderId == 0 ||
                GetById(SalesOrder.SalesOrderId) is null)
            {
                Insert(SalesOrder);
                return;
            }

            Update(SalesOrder);
        }

        /// <summary>
        ///     Update SalesOrders, if not exist insert them
        /// </summary>
        /// <param name="SalesOrders"></param>
        public void UpdateOrInsert(IEnumerable<SalesOrder> SalesOrders)
        {
            foreach (var SalesOrder in SalesOrders) UpdateOrInsert(SalesOrder);
        }

        /// <summary>
        ///     Update SalesOrder
        /// </summary>
        /// <param name="SalesOrder"></param>
        public void Update(SalesOrder SalesOrder)
        {
            if (SalesOrder.SalesOrderId == 0 ||
                GetById(SalesOrder.SalesOrderId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @SalesOrderId, @RefDebitorId, @OrderDate, @RefSalesTypeId, @Remarks, @IsClosed ",
                        SalesOrder);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete SalesOrder by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @SalesOrderId", new {SalesOrderId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete SalesOrder by Item
        /// </summary>
        /// <param name="id"></param>
        public void Delete(SalesOrder SalesOrder)
        {
            Delete(SalesOrder.SalesOrderId);
        }

        public void AddReferences()
        {
            AddSalesTypesReference();
            AddDebitorsReference();
        }

        private void AddSalesTypesReference()
        {
            string refTable = "SalesTypes";

            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_{refTable}', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_{refTable} FOREIGN KEY(RefSalesTypeId) REFERENCES {refTable}(SalesTypeId) ON DELETE CASCADE";

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

        private void AddDebitorsReference()
        {
            string refTable = "Debitors";

            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_{refTable}', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_{refTable} FOREIGN KEY(RefDebitorId) REFERENCES {refTable}(DebitorId) ON DELETE CASCADE";

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