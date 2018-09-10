using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Accounting
{
    public class Debit
    {
        public int DebitId { get; set; }
        public decimal Amount { get; set; }
        public int KontenrahmenId { get; set; }
        public Kontenrahmen Kontenrahmen { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}
