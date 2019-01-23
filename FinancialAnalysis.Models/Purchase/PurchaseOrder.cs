using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models
{
    public class PurchaseOrder : BindableBase
    {
        public int PurchaseOrderId { get; set; }
        public string PurchaseInvoiceNumber { get; set; }
        public int CreditorId { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public DateTimeOffset DeliveryDate { get; set; }
        public int PurchaseTypeId { get; set; }
        public string Remarks { get; set; } // Bemerkung
        public double Amount { get; set; }
        public double Discount { get; set; }
        public double Tax { get; set; }
        public double Total { get; set; }
        public List<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = new List<PurchaseOrderDetail>();
    }
}
