using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.PaymentManagement
{
    public class PaymentVoucher : BindableBase
    {
        public int PaymentvoucherId { get; set; }
        [Display(Name = "Payment Voucher Number")]
        public string PaymentNumber { get; set; }
        public int BillId { get; set; }
        public DateTime PaymentDate { get; set; }
        public int PaymentTypeId { get; set; }
        public double PaymentAmount { get; set; }
        public int CashBankId { get; set; }
        public bool IsFullPaid { get; set; } = true;
    }
}
