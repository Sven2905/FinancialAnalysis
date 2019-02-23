using DevExpress.Mvvm;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Kontenrahmen
    /// </summary>
    public class CostAccount : BindableBase
    {
        public CostAccount()
        {
            IsEditable = true;
        }

        /// <summary>
        /// Id
        /// </summary>
        public int CostAccountId { get; set; }

        /// <summary>
        /// Kontenrahmen-Nummer
        /// </summary>
        public int AccountNumber { get; set; }

        /// <summary>
        /// Beschreibung
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Referenz-Id des Steuertyps
        /// </summary>
        public int RefTaxTypeId { get; set; } = 1;

        /// <summary>
        /// Steuertyp
        /// </summary>
        public TaxType TaxType { get; set; }

        /// <summary>
        /// Kontenrahmenkategorie
        /// </summary>
        public CostAccountCategory CostAccountCategory { get; set; }

        /// <summary>
        /// Referenz-Id der Kontenrahmenkategorie
        /// </summary>
        public int RefCostAccountCategoryId { get; set; }

        /// <summary>
        /// Wird angezeigt
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Ist editierbar
        /// </summary>
        public bool IsEditable { get; set; }

        /// <summary>
        /// Ausgabe: Nummer - Beschreibung
        /// </summary>
        public string DisplayName => $"{AccountNumber} - {Description}";

        /// <summary>
        /// Zuordnung Aktivkonto der Bilanzrechnung
        /// </summary>
        public int RefActiveBalanceAccountId { get; set; }

        /// <summary>
        /// Zuordnung Passivkonto der Bilanzrechnung
        /// </summary>
        public int RefPassiveBalanceAccountId { get; set; }

        /// <summary>
        /// Zuordnung Gewinn und Verlustrechnung
        /// </summary>
        public int RefGainAndLossAccountId { get; set; }
    }
}