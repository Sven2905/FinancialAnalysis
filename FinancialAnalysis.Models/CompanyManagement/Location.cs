using DevExpress.Mvvm;

namespace FinancialAnalysis.Models.CompanyManagement
{
    public class Location : BindableBase
    {
        public int LocationId { get; set; }
        public string Street { get; set; }
        public int Postcode { get; set; }
        public string City { get; set; }
        public FederalState FederalState { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public bool IsActive { get; set; }

        //public virtual Customer Customer { get; set; }
        //public int CustomerId { get; set; }
    }
}