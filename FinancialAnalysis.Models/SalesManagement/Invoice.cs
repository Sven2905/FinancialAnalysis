using System;
using DevExpress.Mvvm;
using FinancialAnalysis.Models.Accounting;
using Utilities;

namespace FinancialAnalysis.Models.SalesManagement
{
    public class Invoice : BindableBase
    {
        public Invoice()
        {
            InvoicePositions = new SvenTechCollection<InvoicePosition>();
        }

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

        public SvenTechCollection<InvoiceReminder> InvoiceReminders { get; set; } = new SvenTechCollection<InvoiceReminder>();
        public SvenTechCollection<InvoicePosition> InvoicePositions { get; set; } = new SvenTechCollection<InvoicePosition>();
    }
}