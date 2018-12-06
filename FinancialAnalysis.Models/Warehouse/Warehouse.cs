using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
