using DevExpress.Mvvm;
using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.SalesManagement
{
    public class Invoice : BindableBase
    {
        public int InvoiceId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoiceDueDate { get; set; }
        public int RefInvoiceTypeId { get; set; }
        public InvoiceType InvoiceType { get; set; }
        public int RefPaymentConditionId { get; set; }
        public PaymentCondition PaymentCondition { get; set; }
        public int RefInvoicePositionId { get; set; }
        public bool IsPaid { get; set; }
        public decimal PaidAmount { get; set; }
    }
}
