using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FinancialAnalysis.Models.ProjectManagement
{
    /// <summary>
    /// Mitarbeiter
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class Employee : BindableBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Vorname
        /// </summary>
        public string Firstname { get; set; }

        /// <summary>
        /// Nachname
        /// </summary>
        public string Lastname { get; set; }

        /// <summary>
        /// Geburtsdatum
        /// </summary>
        public DateTime Birthdate { get; set; } = DateTime.Now;

        /// <summary>
        /// Strasse
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Stadt
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// PLZ
        /// </summary>
        public int Postcode { get; set; }

        /// <summary>
        /// Geschlecht
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Familienstand
        /// </summary>
        public CivilStatus CivilStatus { get; set; }

        /// <summary>
        /// Referenz-Id Tarif
        /// </summary>
        public int RefTariffId { get; set; }

        /// <summary>
        /// Tarif
        /// </summary>
        public Tariff Tariff { get; set; }

        /// <summary>
        /// Steuer-Id
        /// </summary>
        public string TaxId { get; set; }

        /// <summary>
        /// Referenz-Id Krankenkasse
        /// </summary>
        public int RefHealthInsuranceId { get; set; }

        /// <summary>
        /// Krankenkasse
        /// </summary>
        public HealthInsurance HealthInsurance { get; set; }

        /// <summary>
        /// Hat Führerschein
        /// </summary>
        public bool HasDrivingLicence { get; set; }

        /// <summary>
        /// Nationalität
        /// </summary>
        public string Nationality { get; set; }

        /// <summary>
        /// Konfession
        /// </summary>
        public string Confession { get; set; }

        /// <summary>
        /// Name der Bank
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// BIC
        /// </summary>
        public string BIC { get; set; }

        /// <summary>
        /// IBAN
        /// </summary>
        public string IBAN { get; set; }

        /// <summary>
        /// Gehalt
        /// </summary>
        public decimal Salary { get; set; }

        /// <summary>
        /// Arbeitsstunden pro Woche
        /// </summary>
        public float WorkHoursPerWeek { get; set; }

        /// <summary>
        /// Urlaubstage pro Jahr
        /// </summary>
        public int VacationDays { get; set; }

        /// <summary>
        /// Sozialversicherungsnummer
        /// </summary>
        public string NationalInsuranceNumber { get; set; }

        /// <summary>
        /// Daten des Mitarbeiterbilds
        /// </summary>
        public byte[] Picture { get; set; }

        /// <summary>
        /// Telefon
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Mobil
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Mailadresse
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// Liste aller zugeordneten Projekte
        /// </summary>
        public virtual List<ProjectEmployeeMapping> ProjectEmployeeMappings { get; set; }

        /// <summary>
        /// Arbeitszeiten in den Projekte
        /// </summary>
        public virtual List<ProjectWorkingTime> ProjectWorkingTimes { get; set; }

        /// <summary>
        /// Ausgabe: Vorname Nachname
        /// </summary>
        public string Name => Firstname + " " + Lastname;

        /// <summary>
        /// Ausgabe: Strasse, PLZ Stadt
        /// </summary>
        public string Address => Street + ", " + Postcode + " " + City;

        /// <summary>
        /// Überprüfung, ob die Daten valide zum Speichern sind
        /// </summary>
        public bool IsValidForSaving
        {
            get
            {
                if (string.IsNullOrEmpty(Firstname)
                || string.IsNullOrEmpty(Lastname)
                || string.IsNullOrEmpty(Street)
                || string.IsNullOrEmpty(City)
                || Equals(0, Postcode))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}