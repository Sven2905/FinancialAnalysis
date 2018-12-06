using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Warehouse
{
    public class BookedProduct
    {
        public int BookedItemId { get; set; }
        public int RefProductId { get; set; }
        public int RefStockyardId { get; set; }
        public int Quantity { get; set; }
    }
}
