using DevExpress.Mvvm;

namespace FinancialAnalysis.Models.ClientManagement
{
    /// <summary>
    /// Klient
    /// </summary>
    public class Client : ViewModelBase
    {
        public Client()
        {
            Company = new Company();
        }

        /// <summary>
        /// Id des Klienten
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Name des Klienten
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
        /// Telefonnummer
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Fax-Nummer
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// Mailadresse
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// IBAN
        /// </summary>
        public string IBAN { get; set; }

        /// <summary>
        /// BIC
        /// </summary>
        public string BIC { get; set; }

        /// <summary>
        /// Name der Bank
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// Gibt an, ob es eine Firma ist
        /// </summary>
        public bool IsCompany { get; set; }

        /// <summary>
        /// Bundesland
        /// </summary>
        public FederalState FederalState { get; set; }

        /// <summary>
        /// Ausgabe: Strasse, PLZ Stadt
        /// </summary>
        public string Address => $"{Street}, {Postcode} {City}";

        /// <summary>
        /// Ausgabe: PLZ Stadt
        /// </summary>
        public string PostcodeCity => $"{Postcode} {City}";

        /// <summary>
        /// Firma
        /// </summary>
        public Company Company { get; set; }
    }
}