using System.Collections.Generic;

namespace FinancialAnalysis.Models.Warehouse
{
    public class Warehouse
    {
        public int WarehouseId { get; set; }
        public string Name { get; set; }
        public int Description { get; set; }
        public string Street { get; set; }
        public int Postcode { get; set; }
        public string City { get; set; }
        public int MyProperty { get; set; }
        public List<Stockyard> Stockyards { get; set; }
    }
}