using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Ordering
{
    public class Order
    {
        public int OrderId { get; set; }
        public int RefDebitorId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CompleteDate { get; set; }
        public decimal Deposit { get; set; }
        public bool IsCompleted { get; set; }
        public List<OrderDetail> OrderDetailItems { get; set; }
    }
}
