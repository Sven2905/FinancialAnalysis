using DevExpress.Mvvm;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FinancialAnalysis.Models
{
    public class Company : BindableBase
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public int Postcode { get; set; }
        public string City { get; set; }
        public string ContactPerson { get; set; }
        public string UStID { get; set; }
        public string TaxNumber { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string eMail { get; set; }
        public string Website { get; set; }
        public string IBAN { get; set; }
        public string BIC { get; set; }
        public string BankName { get; set; }
        public FederalState FederalState { get; set; }
        public string Address { get { return $"{Street}, {Postcode} {City}"; } }
    }
}
