using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Accounting
{
    public class Booking
    {
        #region Properties

        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public List<ScannedDocument> ScannedDocuments { get; set; }
        public List<Debit> Debits { get; set; }
        public List<Credit> Credits { get; set; }

        #endregion

        #region Methods

        #endregion

    }
}
