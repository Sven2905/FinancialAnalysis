using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.BillManagement
{
    public class Bill : BindableBase
    {
        public int BillId { get; set; }
        public int GoodsReceivedNoteId { get; set; }
        public string VendorInvoiceNumber { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime BillDueDate { get; set; }
        public int BillTypeId { get; set; }
    }
}
