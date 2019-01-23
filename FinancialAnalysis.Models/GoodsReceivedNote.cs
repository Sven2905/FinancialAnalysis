using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models
{
    public class GoodsReceivedNote : BindableBase
    {
        public int GoodsReceivedNoteId { get; set; }
        public string GoodsReceivedNoteNumber { get; set; }
        public int PurchaseOrderId { get; set; }
        public DateTime GRNDate { get; set; }
        public string VendorInvoiceNumber { get; set; }
        public int StockyardId { get; set; }
        public bool IsFullReceive { get; set; } = true;
    }
}
