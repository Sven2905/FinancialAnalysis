using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.PurchaseManagement
{
    public class GoodsReceivedNote : BindableBase
    {
        public int GoodsReceivedNoteId { get; set; }
        public int RefPurchaseOrderId { get; set; }
        public byte[] Content { get; set; }
    }
}
