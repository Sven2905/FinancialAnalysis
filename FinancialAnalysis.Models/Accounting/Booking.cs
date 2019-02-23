using DevExpress.Mvvm;
using System;
using System.Collections.Generic;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Buchung
    /// </summary>
    public class Booking : BindableBase
    {
        public Booking()
        {
        }

        public Booking(decimal amount, DateTime date, int refCostCenterId, string description = "")
        {
            Amount = amount;
            Date = date;
            RefCostCenterId = refCostCenterId;
            Description = description;
        }

        #region Properties

        /// <summary>
        /// Id
        /// </summary>
        public int BookingId { get; set; }

        /// <summary>
        /// Gesamtbetrag
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Datum der Buchung
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Beschreibungstext
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Liste der eingescannten Dokumente, z.B. Rechnungen
        /// </summary>
        public List<ScannedDocument> ScannedDocuments { get; set; } = new List<ScannedDocument>();

        /// <summary>
        /// Liste der Soll-Positionen
        /// </summary>
        public List<Debit> Debits { get; set; } = new List<Debit>();

        /// <summary>
        /// Liste der Haben-Positionen
        /// </summary>
        public List<Credit> Credits { get; set; } = new List<Credit>();

        /// <summary>
        /// Referenz-Id der zugeordneten Kostenstelle
        /// </summary>
        public int RefCostCenterId { get; set; }

        /// <summary>
        /// Zugeordnete Kostenstelle
        /// </summary>
        public CostCenter CostCenter { get; set; }

        #endregion Properties
    }
}