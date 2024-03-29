﻿using DevExpress.Mvvm;
using FinancialAnalysis.Models.ProductManagement;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.SalesManagement
{
    /// <summary>
    /// Rechnungsposition
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class InvoicePosition : BaseClass
    {
        public InvoicePosition()
        {
        }

        public InvoicePosition(int RefInvoiceId, int RefSalesOrderPositionId, int Quantity)
        {
            this.RefInvoiceId = RefInvoiceId;
            this.RefSalesOrderPositionId = RefSalesOrderPositionId;
            this.Quantity = Quantity;
        }

        /// <summary>
        /// Id
        /// </summary>
        public int InvoicePositionId { get; set; }

        /// <summary>
        /// Referenz-Id Rechnung
        /// </summary>
        public int RefInvoiceId { get; set; }

        /// <summary>
        /// Referenz-Id Auftragsposition
        /// </summary>
        public int RefSalesOrderPositionId { get; set; }

        /// <summary>
        /// Entsprechende Position des Auftrags, Menge kann abweichen
        /// </summary>
        public SalesOrderPosition SalesOrderPosition { get; set; }

        /// <summary>
        /// Menge
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Produkt
        /// </summary>
        public Product Product { get; set; }
    }
}