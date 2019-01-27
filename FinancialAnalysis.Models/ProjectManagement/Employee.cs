using DevExpress.Mvvm;
using System;
using System.Collections.Generic;

namespace FinancialAnalysis.Models.ProjectManagement
{
    /// <summary>
    ///     Employee
    /// </summary>
    public class Employee : BindableBase
    {
        public int EmployeeId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Birthdate { get; set; } = DateTime.Now;
        public string Street { get; set; }
        public string City { get; set; }
        public int Postcode { get; set; }
        public Gender Gender { get; set; }
        public CivilStatus CivilStatus { get; set; }
        public int RefTariffId { get; set; }
        public Tariff Tariff { get; set; }

        // Steuer-Id
        public string TaxId { get; set; }
        public int RefHealthInsuranceId { get; set; }
        public HealthInsurance HealthInsurance { get; set; }
        public bool HasDrivingLicence { get; set; }
        public string Nationality { get; set; }
        public string Confession { get; set; }
        public string BankName { get; set; }
        public string BIC { get; set; }
        public string IBAN { get; set; }
        public decimal Salary { get; set; }
        public float WorkHoursPerWeek { get; set; }

        public int VacationDays { get; set; }

        // Sozialversicherungsnummer
        public string NationalInsuranceNumber { get; set; }
        public byte[] Picture { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Mail { get; set; }

        public virtual List<ProjectEmployeeMapping> ProjectEmployeeMappings { get; set; }
        public virtual List<ProjectWorkingTime> ProjectWorkingTimes { get; set; }

        public string Name => Firstname + " " + Lastname;
        public string Address => Street + ", " + Postcode + " " + City;
    }
}