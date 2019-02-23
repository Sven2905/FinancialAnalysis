using System.Collections.Generic;

namespace FinancialAnalysis.Models.ProjectManagement
{
    /// <summary>
    /// Tarif
    /// </summary>
    public class Tariff
    {
        /// <summary>
        /// Id
        /// </summary>
        public int TariffId { get; set; }

        /// <summary>
        /// Name des Tarifs
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Mitarbeiter in diesem Tarif
        /// </summary>
        public virtual List<Employee> Employees { get; set; }
    }
}