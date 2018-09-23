using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Accounting
{
    public class Credit : BindableBase
    { 
        public int CreditId { get; set; }
        public decimal Amount { get; set; }
        public int KontenrahmenId { get; set; }
        public Kontenrahmen Kontenrahmen { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}
