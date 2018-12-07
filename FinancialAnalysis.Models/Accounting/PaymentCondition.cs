using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Accounting
{
    public class PaymentCondition
    {
        public int PaymentConditionId { get; set; }
        public string Name { get; set; }
        public decimal DiscountPercent { get; set; }
        public int RefCashbackId { get; set; }
        public Cashback Cashback { get; set; }
    }
}
