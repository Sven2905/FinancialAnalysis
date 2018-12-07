using System;
using System.Collections.Generic;

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