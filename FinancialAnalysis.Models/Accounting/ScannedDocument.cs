using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Accounting
{
    public class ScannedDocument
    {
        public int ScannedDocumentId { get; set; }
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public DateTime Date { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}
