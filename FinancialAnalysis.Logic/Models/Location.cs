using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.Models
{
    public class Location
    {
        public int LocationId { get; set; }
        public string Street { get; set; }
        public int Postcode { get; set; }
        public string City { get; set; }
        public EnumFederalState FederalState { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public bool IsActive { get; set; }

        public virtual Customer Customer { get; set; }
        public int CustomerId { get; set; }
    }
}
