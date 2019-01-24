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
        public string CreditorInvoiceNumber { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime BillDueDate { get; set; }
        public int BillTypeId { get; set; }
        public byte[] Content { get; set; }
        public BillType RefBillType { get; set; }
    }
}
