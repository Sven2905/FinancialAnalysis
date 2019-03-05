using DevExpress.Mvvm;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.ClientManagement;
using System;
using Utilities;

namespace FinancialAnalysis.Models.SalesManagement
{
    public class Invoice : BindableBase
    {
        public Invoice()
        {
            InvoicePositions = new SvenTechCollection<InvoicePosition>();
            RefPaymentConditionId = 1;
        }

        /// <summary>
        /// Id
        /// </summary>
        public int InvoiceId { get; set; }

        /// <summary>
        /// Rechnungsempfänger
        /// </summary>
        public Client Client{ get; set; }

        /// <summary>
        /// Referenz auf die Auftrags-Id
        /// </summary>
        public int RefSalesOrderId { get; set; }

        /// <summary>
        /// Rechnungsdatum
        /// </summary>
        public DateTime InvoiceDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Fälligkeitsdatum
        /// </summary>
        public DateTime InvoiceDueDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Referenz-Id des Rechnungstyps
        /// </summary>
        public int RefInvoiceTypeId { get; set; }

        /// <summary>
        /// Rechnungstyp
        /// </summary>
        public InvoiceType InvoiceType { get; set; }

        /// <summary>
        /// Referenz-Id der Zahlungskondition
        /// </summary>
        public int RefPaymentConditionId { get; set; }

        /// <summary>
        /// Zahlungskondition
        /// </summary>
        public PaymentCondition PaymentCondition { get; set; }

        /// <summary>
        /// Ist komplett bezahlt
        /// </summary>
        public bool IsPaid { get; set; }

        /// <summary>
        /// Bezahlter Betrag
        /// </summary>
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// Mahnungen
        /// </summary>
        public SvenTechCollection<InvoiceReminder> InvoiceReminders { get; set; } = new SvenTechCollection<InvoiceReminder>();

        /// <summary>
        /// Rechnungspositionen
        /// </summary>
        public SvenTechCollection<InvoicePosition> InvoicePositions { get; set; } = new SvenTechCollection<InvoicePosition>();
    }
}