﻿using System.Collections.Generic;

namespace FinancialAnalysis.Models.ProjectManagement
{
    /// <summary>
    /// Krankenkasse
    /// </summary>
    public class HealthInsurance
    {
        /// <summary>
        /// Id
        /// </summary>
        public int HealthInsuranceId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Strasse
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// PLZ
        /// </summary>
        public int Postcode { get; set; }

        /// <summary>
        /// Stadt
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Kontaktperson
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// Telefon
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Mailadresse
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// Liste aller Mitarbeiter mit der Krankenkasse
        /// </summary>
        public virtual List<Employee> Employees { get; set; }
    }
}