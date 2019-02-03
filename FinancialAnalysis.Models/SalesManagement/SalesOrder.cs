﻿using System;
using System.Windows.Media;
using DevExpress.Mvvm;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.General;
using Utilities;

namespace FinancialAnalysis.Models.SalesManagement
{
    public class SalesOrder : BindableBase
    {
        public int SalesOrderId { get; set; }
        public int RefDebitorId { get; set; }
        public Debitor Debitor { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public int RefSalesTypeId { get; set; }
        public SalesType SalesType { get; set; }
        public int RefShipmentTypeId { get; set; }
        public ShipmentType ShipmentType { get; set; }
        public string Remarks { get; set; } // Bemerkung

        public SvenTechCollection<SalesOrderPosition> SalesOrderPositions { get; set; } =
            new SvenTechCollection<SalesOrderPosition>();

        public SvenTechCollection<Shipment> Shipments { get; set; } = new SvenTechCollection<Shipment>();
        public SvenTechCollection<Invoice> Invoices { get; set; } = new SvenTechCollection<Invoice>();
        public Color InvoiceStatusColor { get; set; } = SvenTechColors.Green;
        public Color ShippingStatusColor { get; set; } = SvenTechColors.Red;
        public bool IsClosed { get; set; }
    }
}