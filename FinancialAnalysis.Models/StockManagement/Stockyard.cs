using System.Collections.Generic;

namespace FinancialAnalysis.Models.StockManagement
{
    public class Stockyard
    {
        public int StockyardId { get; set; }
        public string Name { get; set; }
        public int RefWarehouseId { get; set; }
        public List<BookedProduct> BookedProducts { get; set; }
    }
}