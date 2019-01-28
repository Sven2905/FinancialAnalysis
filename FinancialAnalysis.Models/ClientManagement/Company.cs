using DevExpress.Mvvm;

namespace FinancialAnalysis.Models.ClientManagement
{
    public class Company : BindableBase
    {
        public int CompanyId { get; set; }
        public string ContactPerson { get; set; }
        public string UStID { get; set; }
        public string TaxNumber { get; set; }
        public string Website { get; set; }
        public string CEO { get; set; }
        public byte[] Logo { get; set; }
        public int RefClientId { get; set; }
    }
}