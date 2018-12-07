using System.Collections.Generic;

namespace FinancialAnalysis.Models.ProjectManagement
{
    public class HealthInsurance
    {
        public int HealthInsuranceId { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public int Postcode { get; set; }
        public string City { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }

        public virtual List<Employee> Employees { get; set; }
    }
}