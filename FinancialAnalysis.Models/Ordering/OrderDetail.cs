using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Ordering
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int RefOrderId { get; set; }
        public int ProductPrototypeId { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerItem { get; set; }
        public int ArrivedQuantity { get; set; }
    }
}
