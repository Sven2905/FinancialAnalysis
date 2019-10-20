using DevExpress.Mvvm;
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
    public class Booking : BaseClass
    {
        #region Constructor

        public Booking()
        {
        }

        public Booking(DateTime date, string description = "")
        {
            Date = date;
            Description = description;
        }

        #endregion Constructor

        #region Fields

        private FixedCostAllocation fixedCostAllocation = new FixedCostAllocation();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Id
        /// </summary>
        public int BookingId { get; set; }

        /// <summary>
        /// Gesamtbetrag
        /// </summary>
        public decimal Amount
        {
            get
            {
                if (Credits != null)
                    return Credits.Sum(x => x.Amount);
                else
                    return 0;
            }
        }

        public decimal AmountWithoutTax
        {
            get
            {
                if (Credits != null && Debits != null)
                    if (Amount > 0)
                        return Math.Min(Credits.Where(x => x.IsTax == false).Sum(x => x.Amount), Debits.Where(x => x.IsTax == false).Sum(x => x.Amount));
                    else
                        return Math.Max(Credits.Where(x => x.IsTax == false).Sum(x => x.Amount), Debits.Where(x => x.IsTax == false).Sum(x => x.Amount));
                else
                    return 0;
            }
        }

        /// <summary>
        /// Datum der Buchung
        /// </summary>
        public DateTime Date { get; set; } = DateTime.Now;

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
        /// Referenz auf zugeordneten Kostenstellenverteilungsschlüssel
        /// </summary>
        public int RefFixedCostAllocationId { get; set; }

        /// <summary>
        /// Zugeordneter Kostenstellenverteilungsschlüssel
        /// </summary>
        public FixedCostAllocation FixedCostAllocation
        {
            get
            {
                if (fixedCostAllocation == null)
                {
                    fixedCostAllocation = new FixedCostAllocation();
                }
                return fixedCostAllocation;
            }
            set => fixedCostAllocation = value;
        }

        /// <summary>
        /// Gibt an, ob die Buchung bereits storniert wurde
        /// </summary>
        public bool IsCanceled { get; set; }

        /// <summary>
        /// Gibt den ersten Kreditor der Soll-Positionen zurück
        /// </summary>
        [JsonIgnore]
        public CostAccount Creditor => Credits?.LastOrDefault()?.CostAccount;

        /// <summary>
        /// Gibt den ersten Debitor der Haben-Positionen zurück
        /// </summary>
        [JsonIgnore]
        public CostAccount Debitor => Debits?.FirstOrDefault()?.CostAccount;

        #endregion Properties
    }
}