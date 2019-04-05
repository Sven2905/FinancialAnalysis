using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
    /// <summary>
    /// Klasse zur Berechnung der Kostenstellen
    /// </summary>
    public class CostCenterCalculations
    {
        /// <summary>
        /// Berechnung der Kostenverteilung
        /// </summary>
        /// <param name="value">Summe, welche aufgeteilt werden soll</param>
        /// <param name="shares">Anteile</param>
        /// <returns></returns>
        public static decimal[] CalculateCostAllocation(decimal value, double[] shares)
        {
            decimal[] results = new decimal[shares.Length];

            double sumShares = shares.Sum();

            for (int i = 0; i < shares.Length; i++)
            {
                results[i] = value / (decimal)sumShares * (decimal)shares[i];
            }

            return results;
        }
    }
}
