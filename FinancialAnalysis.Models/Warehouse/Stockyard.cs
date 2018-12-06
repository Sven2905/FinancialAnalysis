using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Warehouse
{
    public class Stockyard
    {
        public int StockyardId { get; set; }
        public string Name { get; set; }
        public int RefWarehouseId { get; set; }
        public List<BookedProduct> BookedProducts { get; set; }
    }
}
