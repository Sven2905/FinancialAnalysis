using FinancialAnalysis.Logic.Models.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.Models
{
    /// <summary>
    /// Employee of the company
    /// </summary>
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Birthdate { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public int Postcode { get; set; }
        public Gender Gender { get; set; }
        public CivilStatus CivilStatus { get; set; }
        public int TariffId { get; set; }
        public virtual Tariff Tariff { get; set; }
        // Steuer-Id
        public string TaxId { get; set; }
        public int HealthInsuranceId { get; set; }
        public HealthInsurance HealthInsurance { get; set; }
        public bool DrivingLicence { get; set; }
        public string Nationality { get; set; }
        public string Confession { get; set; }
        public string BankName { get; set; }
        public string BIC { get; set; }
        public string IBAN { get; set; }
        public decimal Salary { get; set; }
        public float HoursPerWeek { get; set; }
        public int VacationDays { get; set; }
        // Sozialversicherungsnummer
        public string NationalInsuranceNumber { get; set; }

        public int ProjectEmployeeMappingId { get; set; }
        public virtual List<ProjectEmployeeMapping> ProjectEmployeeMappings { get; set; }
        public virtual List<ProjectWorkingTime> ProjectWorkingTimes { get; set; }
    }
}
