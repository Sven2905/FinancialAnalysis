using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public int Postcode { get; set; }
        public string City { get; set; }
        //public Employee CEO { get; set; }
        public int EmployeeId { get; set; }
        public string UStID { get; set; }
        public string TaxNumber { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Mail { get; set; }
        public string Website { get; set; }
        public string IBAN { get; set; }
        public string BIC { get; set; }
        public string BankName { get; set; }
        public EnumFederalState FederalState { get; set; }
    }
}
