using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.ClientManagement
{
    public class Client : ViewModelBase
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public int Postcode { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Mail { get; set; }
        public string IBAN { get; set; }
        public string BIC { get; set; }
        public string BankName { get; set; }
        public bool IsCompany { get; set; }
        public FederalState FederalState { get; set; }
        public string Address => $"{Street}, {Postcode} {City}";
        public Company Company { get; set; }
    }
}
