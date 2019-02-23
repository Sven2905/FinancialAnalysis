using DevExpress.Mvvm;

namespace FinancialAnalysis.Models.ClientManagement
{
    /// <summary>
    /// Firma
    /// </summary>
    public class Company : BindableBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Kontaktperson
        /// </summary>
        public string ContactPerson { get; set; }

        /// <summary>
        /// Umsatzsteuer-Id
        /// </summary>
        public string UStID { get; set; }

        /// <summary>
        /// Steuernummer
        /// </summary>
        public string TaxNumber { get; set; }

        /// <summary>
        /// Adresse der Website
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Geschäftsführer
        /// </summary>
        public string CEO { get; set; }

        /// <summary>
        /// Daten des Logos
        /// </summary>
        public byte[] Logo { get; set; }

        /// <summary>
        /// Referenz-Id des zugeordneten Klienten
        /// </summary>
        public int RefClientId { get; set; }
    }
}