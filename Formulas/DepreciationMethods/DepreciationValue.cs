namespace Formulas.DepreciationMethods
{
    /// <summary>
    /// Abschreibungsbetrag anhand der arithmetisch degressiven Abschreibungsmethode
    /// </summary>
    public class DepreciationValue
    {
        public DepreciationValue(int Year, decimal YearlyDepreciation, decimal AssetValue)
        {
            this.Year = Year;
            this.AssetValue = AssetValue;
            this.YearlyDepreciation = YearlyDepreciation;
        }

        /// <summary>
        /// Nutzungsjahr 
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// kalk. Restbuchwert
        /// </summary>
        public decimal AssetValue { get; set; }

        /// <summary>
        /// Jährliche Abschreibung
        /// </summary>
        public decimal YearlyDepreciation { get; set; }
    }
}
