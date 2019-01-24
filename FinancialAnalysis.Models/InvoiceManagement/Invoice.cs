using DevExpress.Mvvm;
using FinancialAnalysis.Models.ShipmentManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.InvoiceManagement
{
    public class Invoice : BindableBase
    {
        public int InvoiceId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoiceDueDate { get; set; }
        public int RefShipmentId { get; set; }
        public Shipment Shipment { get; set; }
        public int RefInvoiceTypeId { get; set; }
        public InvoiceType InvoiceType { get; set; }
    }
}
