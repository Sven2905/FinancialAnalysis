﻿using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Buchung
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class Booking : BindableBase
    {
        public Booking()
        {
        }

        public Booking(decimal amount, DateTime date, string description = "")
        {
            Amount = amount;
            Date = date;
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
        /// Liste der zugeordneten Beträge pro Kostenstelle
        /// </summary>
        public List<BookingCostCenterMapping> BookingCostCenterMappingList { get; set; } = new List<BookingCostCenterMapping>();

        /// <summary>
        /// Liste der Soll-Positionen
        /// </summary>
        public List<Debit> Debits { get; set; } = new List<Debit>();

        /// <summary>
        /// Liste der Haben-Positionen
        /// </summary>
        public List<Credit> Credits { get; set; } = new List<Credit>();

        /// <summary>
        /// Zugeordneter Kostenstellenverteilungsschlüssel
        /// </summary>
        public int RefFixedCostAllocationId { get; set; }

        /// <summary>
        /// Gibt den ersten Kreditor der Soll-Positionen zurück
        /// </summary>
        [JsonIgnore]
        public CostAccount Creditor { get => Credits?.FirstOrDefault().CostAccount; }

        /// <summary>
        /// Gibt den ersten Debitor der Haben-Positionen zurück
        /// </summary>
        [JsonIgnore]
        public CostAccount Debitor { get => Debits?.FirstOrDefault().CostAccount; }

        #endregion Properties
    }
}