using System.Collections.Generic;

namespace FinancialAnalysis.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }

        public List<Location> Locations { get; set; }
    }
}