﻿using DevExpress.Mvvm;
using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace FinancialAnalysis.Models.SalesManagement
{
    public class SalesOrder : BindableBase
    {
        public int SalesOrderId { get; set; }
        public int RefDebitorId { get; set; }
        public Debitor Debitor { get; set; }
        public DateTime OrderDate { get; set; }
        public int RefSalesTypeId { get; set; }
        public SalesType SalesType { get; set; }
        public string Remarks { get; set; } // Bemerkung
        public SvenTechCollection<SalesOrderPosition> SalesOrderPositions { get; set; } = new SvenTechCollection<SalesOrderPosition>();
        public SvenTechCollection<Shipment> Shipments { get; set; } = new SvenTechCollection<Shipment>();
        public SvenTechCollection<Invoice> Invoices { get; set; } = new SvenTechCollection<Invoice>();
        public bool IsClosed { get; set; }
    }
}