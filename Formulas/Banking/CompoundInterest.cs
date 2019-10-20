using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas.Banking
{
    public static class CompoundInterest
    {
        /// <summary>
        /// Berechnet das Endkapital
        /// </summary>
        /// <returns>Endkapital</returns>
        public static decimal CalculateFinalCapital(CompoundInterestItem compoundInterestItem)
        {
            return compoundInterestItem.SeedCapital * (decimal)(Math.Pow((1 + compoundInterestItem.Rate / (100d* (int)compoundInterestItem.CompoundInterestIntervall)), compoundInterestItem.Years * (int)compoundInterestItem.CompoundInterestIntervall));
        }

        /// <summary>
        /// Berechnet den Zinssatz
        /// </summary>
        /// <returns></returns>
        public static double CalculateRate(CompoundInterestItem compoundInterestItem)
        {
            return ((Math.Pow((double)(compoundInterestItem.FinalCapital / compoundInterestItem.SeedCapital), (1d / (compoundInterestItem.Years * (int)compoundInterestItem.CompoundInterestIntervall))) - 1) *100 * (int)compoundInterestItem.CompoundInterestIntervall) / 100;
        }

        /// <summary>
        /// Berechnet die Dauer (Jahre)
        /// </summary>
        /// <returns></returns>
        public static double CalculateYears(CompoundInterestItem compoundInterestItem)
        {
            return Math.Log((double)compoundInterestItem.FinalCapital / (double)compoundInterestItem.SeedCapital) / Math.Log(Math.Pow(1 + compoundInterestItem.Rate / (100 * (int)compoundInterestItem.CompoundInterestIntervall), (int)compoundInterestItem.CompoundInterestIntervall));
        }
    }
}
