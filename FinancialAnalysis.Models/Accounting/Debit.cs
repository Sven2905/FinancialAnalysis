using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Accounting
{
    public class Debit : BindableBase
    {
        public Debit()
        {

        }

        public Debit(decimal Amount, int RefCostAccountId, int RefBookingId)
        {
            this.Amount = Amount;
            this.RefCostAccountId = RefCostAccountId;
            this.RefBookingId = RefBookingId;
        }

        public int DebitId { get; set; }
        public decimal Amount { get; set; }
        public int RefCostAccountId { get; set; }                                                                                                                                                                                                                 
        public CostAccount CostAccount { get; set; }
        public int RefBookingId { get; set; }
    }
}
