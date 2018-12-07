using System.Collections.Generic;

namespace FinancialAnalysis.Models.ProjectManagement
{
    public class Tariff
    {
        public int TariffId { get; set; }
        public string Name { get; set; }

        public virtual List<Employee> Employees { get; set; }
    }
}