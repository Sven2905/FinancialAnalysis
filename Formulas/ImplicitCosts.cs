namespace Formulas
{
    /// <summary>
    /// Kalkulatorische Kosten
    /// </summary>
    public class ImplicitCosts
    {
        /// <summary>
        /// Berechnet die Kalkulatorische Zinsen
        /// </summary>
        /// <param name="capital">Anschaffungswert</param>
        /// <param name="rate">Kalkulatorische Zinssatz</param>
        /// <param name="assetValue">Restwert</param>
        /// <returns>Kalkulatorische Zinsen</returns>
        public static decimal CalculatedInterestRate(decimal capital, decimal assetValue, decimal rate)
        {
            return 1 / 2 * (capital + assetValue) * rate;
        }
    }
}
