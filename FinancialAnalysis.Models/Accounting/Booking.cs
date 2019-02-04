using DevExpress.Mvvm;
using System;
using System.Collections.Generic;

namespace FinancialAnalysis.Models.Accounting
{
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

        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public List<ScannedDocument> ScannedDocuments { get; set; } = new List<ScannedDocument>();
        public List<Debit> Debits { get; set; } = new List<Debit>();
        public List<Credit> Credits { get; set; } = new List<Credit>();
        public int RefCostCenterId { get; set; }
        public CostCenter CostCenter { get; set; }

        #endregion Properties
    }
}