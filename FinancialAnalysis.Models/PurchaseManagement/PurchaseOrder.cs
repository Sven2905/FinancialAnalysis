using DevExpress.Mvvm;
using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace FinancialAnalysis.Models.PurchaseManagement
{
    public class PurchaseOrder : BindableBase
    {
        public int PurchaseOrderId { get; set; }
        public string PurchaseInvoiceNumber { get; set; }
        public int RefCreditorId { get; set; }
        public Creditor Creditor { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int RefPurchaseTypeId { get; set; }
        public PurchaseType PurchaseType { get; set; }
        public string Remarks { get; set; } // Bemerkung
        public bool IsClosed { get; set; }

        public SvenTechCollection<PurchaseOrderPosition> PurchaseOrderPositions { get; set; } = new SvenTechCollection<PurchaseOrderPosition>();
        public SvenTechCollection<Bill> Bills { get; set; } = new SvenTechCollection<Bill>();
        public SvenTechCollection<GoodsReceivedNote> GoodsReceivedNotes { get; set; } = new SvenTechCollection<GoodsReceivedNote>();
    }
}
