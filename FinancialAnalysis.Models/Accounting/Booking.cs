﻿using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace FinancialAnalysis.Models.Accounting
{
    public class Booking : BindableBase
    {
        public Booking()
        {

        }

        public Booking(decimal Amount, DateTime Date, string Description = "")
        {
            this.Amount = Amount;
            this.Date = Date;
            this.Description = Description;
        }

        #region Properties

        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public List<ScannedDocument> ScannedDocuments { get; set; } = new List<ScannedDocument>();
        public List<Debit> Debits { get; set; } = new List<Debit>();
        public List<Credit> Credits { get; set; } = new List<Credit>();

        #endregion

        #region Methods

        #endregion

    }
}
