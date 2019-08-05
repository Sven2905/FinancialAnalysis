using DevExpress.Mvvm;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.ProjectManagement;
using Newtonsoft.Json;
using System;
using Utilities;

namespace FinancialAnalysis.Models.SalesManagement
{
    /// <summary>
    /// Rechnung
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class Invoice : BindableBase
    {
        private decimal _PaidAmount;

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
        public Debitor Debitor { get; set; }

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
        /// Zahlungsdatum
        /// </summary>
        public DateTime PaidDate { get; set; } = DateTime.Now;

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
        /// Ist bezahlt
        /// </summary>
        public bool IsPaid { get; set; }

        /// <summary>
        /// Bezahlter Betrag
        /// </summary>
        public decimal PaidAmount
        {
            get => _PaidAmount;
            set
            {
                _PaidAmount = value;
                if (PaidAmount == TotalAmountWithPaymentCondition)
                {
                    IsPaid = true;
                }
            }
        }

        /// <summary>
        /// Endbetrag unter Berücksichtigung der Zahlungsbedingungen (Skonto)
        /// </summary>
        public decimal TotalAmountWithPaymentCondition => CheckForPaymentConditions();

        /// <summary>
        /// Referenz-Id des Mitarbeiters
        /// </summary>
        public int RefEmployeeId { get; set; }

        /// <summary>
        /// Zuständiger Mitarbeiter
        /// </summary>
        public Employee Employee { get; set; }

        /// <summary>
        /// Rechnungsbetrag
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Ausstehender Betrag
        /// </summary>
        public decimal OutstandingAmount => TotalAmount - PaidAmount;

        /// <summary>
        /// Mahnungen
        /// </summary>
        public SvenTechCollection<InvoiceReminder> InvoiceReminders { get; set; } = new SvenTechCollection<InvoiceReminder>();

        /// <summary>
        /// Rechnungspositionen
        /// </summary>
        public SvenTechCollection<InvoicePosition> InvoicePositions { get; set; } = new SvenTechCollection<InvoicePosition>();

        /// <summary>
        /// Wenn die Zahlungsbedingungen eingehalten wurden, wird der automatisch der Prozentsatz abgezogen
        /// </summary>
        /// <returns>Der letztendlich zu bezahlende Betrag</returns>
        private decimal CheckForPaymentConditions()
        {
            decimal result = TotalAmount;

            if (PaymentCondition?.CheckIfAdhered(InvoiceDueDate, PaidDate) == true)
            {
                return result * (100 - PaymentCondition.Percentage);
            }

            return result;
        }
    }
}