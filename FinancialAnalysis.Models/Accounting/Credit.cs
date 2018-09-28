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
        public int RefCostAccountId { get; set; }
        public CostAccount CostAccount { get; set; }
        public int BookingId { get; set; }
    }
}
