using System;
using System.Collections.Generic;
using System.Linq;

namespace Formulas.DepreciationMethods
{
    /// <summary>
    /// Abschreibungen
    /// </summary>
    public static class Depreciations
    {
        /// <summary>
        /// Berechnung des Degressionsbetrags (Arithmetisch-degressive Abschreibung)
        /// </summary>
        /// <param name="initialValue">Anschaffungs- oder Wiederbeschaffungskosten</param>
        /// <param name="assetValue">Restwerts</param>
        /// <param name="years">Nutzungsjahre</param>
        /// <returns>Degressionsbetrag</returns>
        public static decimal CalculateArithmenticDegressiveYearly(decimal initialValue, decimal assetValue, int years)
        {
            return (initialValue - assetValue) / ((years * (years + 1)) / 2);
        }

        /// <summary>
        /// Berechnet jährlichen Abschreibungsbeträge (Arithmetisch-degressive Abschreibung)
        /// </summary>
        /// <param name="initialValue">Anschaffungs- oder Wiederbeschaffungskosten</param>
        /// <param name="assetValue">Restwerts</param>
        /// <param name="years">Nutzungsjahre</param>
        /// <returns>Jährliche Abschreibungsbeträge</returns>
        public static IEnumerable<DepreciationValue> CalculateArithmenticDegressiveValuesForYears(decimal initialValue, decimal assetValue, int years)
        {
            List<DepreciationValue> depreciationValues = new List<DepreciationValue>
            {
                new DepreciationValue(0, 0, initialValue)
            };

            decimal yearlyAssetValue = CalculateArithmenticDegressiveYearly(initialValue, assetValue, years);
            int year = 1;
            for (int i = years; i > 0; i--)
            {
                initialValue = Math.Max(initialValue - i * yearlyAssetValue, 0);
                depreciationValues.Add(new DepreciationValue(year++, i * yearlyAssetValue, initialValue));
            }
            return depreciationValues;
        }

        /// <summary>
        /// Berechnet die Abschreibungsquote (Geometrisch-degressive Abschreibung)
        /// </summary>
        /// <param name="initialValue">Anschaffungs- oder Wiederbeschaffungskosten</param>
        /// <param name="assetValue">Restwerts</param>
        /// <param name="years">Nutzungsjahre</param>
        /// <returns>Abschreibungsquote</returns>
        public static decimal CalculateGeometryDregressiveValuesRate(decimal initialValue, decimal assetValue, int years)
        {
            return (decimal)(100 * (1 - Math.Pow((double)(assetValue / initialValue), 1.0 / years)));
        }

        /// <summary>
        /// Berechnet die jährlichen Abschreibungsbeträge (Geometrisch-degressive Abschreibung)
        /// </summary>
        /// <param name="initialValue">Anschaffungs- oder Wiederbeschaffungskosten</param>
        /// <param name="assetValue">Restwerts</param>
        /// <param name="years">Nutzungsjahre</param>
        /// <returns>Jährliche Abschreibungsbeträge</returns>
        public static IEnumerable<DepreciationValue> CalculateGeometryDregressiveForYears(decimal initialValue, decimal assetValue, int years)
        {
            List<DepreciationValue> depreciationValues = new List<DepreciationValue>
            {
                new DepreciationValue(0, 0, initialValue)
            };

            decimal yearlyDepreciationRate = CalculateGeometryDregressiveValuesRate(initialValue, assetValue, years);
            int year = 1;
            for (int i = years; i > 0; i--)
            {
                decimal yearlyAssetValue = initialValue * (yearlyDepreciationRate / 100);
                initialValue = Math.Max(initialValue - yearlyAssetValue, 0);
                depreciationValues.Add(new DepreciationValue(year++, yearlyAssetValue, initialValue));
            }

            return depreciationValues;
        }

        /// <summary>
        /// Berechnet ährliche Abschreibung (lineares Verfahren)
        /// </summary>
        /// <param name="initialValue">Anfangswert</param>
        /// <param name="assetValue">Buchwert nach x Jahren</param>
        /// <param name="years">Dauer in Jahren</param>
        /// <returns>Jährliche Abschreibung</returns>
        public static decimal CalculateLinearYearly(decimal initialValue, decimal assetValue, int years)
        {
            return (initialValue - assetValue) / (years);
        }

        /// <summary>
        /// Berechnet die jährlichen Abschreibungsbeträge
        /// </summary>
        /// <param name="initialValue">Anfangswert</param>
        /// <param name="yearlyDepreciation">Jährliche Abschreibung</param>
        /// <param name="years">Dauer in Jahren</param>
        /// <returns>Jährliche Abschreibungsbeträge</returns>
        public static IEnumerable<DepreciationValue> CalculateLinearValueForYears(decimal initialValue, decimal assetValue, int years)
        {
            List<DepreciationValue> depreciationValues = new List<DepreciationValue>
            {
                new DepreciationValue(0, 0, initialValue)
            };

            decimal yearlyAssetValue = CalculateLinearYearly(initialValue, assetValue, years);
            int year = 1;
            for (int i = years; i > 0; i--)
            {
                initialValue = Math.Max(initialValue - yearlyAssetValue, 0);
                depreciationValues.Add(new DepreciationValue(year++, yearlyAssetValue, initialValue));
            }

            return depreciationValues;
        }

        /// <summary>
        /// Berechnet den Abschreibungsbetrag je Leistungseinheit
        /// </summary>
        /// <param name="initialValue">Anschaffungs-/Herstellungskosten</param>
        /// <param name="totalPower">Gesamtleistung</param>
        /// <returns></returns>
        public static decimal CalculatePerfomanceBasedBasicValue(decimal initialValue, decimal totalPower)
        {
            return initialValue / totalPower;
        }

        /// <summary>
        /// Berechnet den Abschreibungsbetrag pro Jahr (Leistungsabhängig)
        /// </summary>
        /// <param name="initialValue">Anschaffungs-/Herstellungskosten</param>
        /// <param name="totalPower">Gesamtleistung</param>
        /// <param name="yearlyPower">Jahresleistung</param>
        /// <returns></returns>
        public static IEnumerable<DepreciationValue> CalculatePerfomanceBased(decimal initialValue, IEnumerable<PerformanceDepreciationItem> yearlyPower)
        {
            var yearlyPowerList = yearlyPower.ToList();

            decimal totalPower = yearlyPowerList.Sum(x => x.Power);

            List<DepreciationValue> depreciationValues = new List<DepreciationValue>
            {
                new DepreciationValue(0, 0, initialValue)
            };

            var baseValue = CalculatePerfomanceBasedBasicValue(initialValue, totalPower);

            for (int i = 0; i < yearlyPowerList.Count; i++)
            {
                decimal yearlyDepreciation = yearlyPowerList[i].Power * baseValue;
                initialValue = Math.Max(initialValue - yearlyDepreciation, 0);
                depreciationValues.Add(new DepreciationValue(i+1, yearlyDepreciation, initialValue));
            }

            return depreciationValues;
        }
    }
}
