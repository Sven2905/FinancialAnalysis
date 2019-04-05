using System;

namespace Formulas
{
    /// <summary>
    /// Formeln zur einfachen Zinsrechnung
    /// </summary>
    public class CalculationOfInterest
    {
        /// <summary>
        /// Berechnung der Zinstage.
        /// Der Zinsmonat umfasst immer 30 Tage, das Zinsjahr umfasst immer 360 Tage. In Monaten mit 31 Tagen werden der 30. und 31. als insgesamt ein Tag gezählt
        /// </summary>
        /// <param name="startDate">Startdatum</param>
        /// <param name="endDate">Enddatum</param>
        /// <returns>Anzahl der Tage</returns>
        public static int CalculateInterestDays_30_360(DateTime startDate, DateTime endDate)
        {
            int factor = 0;
            if (startDate.Day >= 30)
            {
                factor = 1;
            }
            return (360 * (endDate.Year - startDate.Year)) + (30 * (endDate.Month - startDate.Month)) + endDate.Day - Math.Min(startDate.Day, 30) - Math.Max(endDate.Day - 30, 0) * factor;
        }

        /// <summary>
        /// Errechnet den Zinsbetrag
        /// </summary>
        /// <param name="capital">Kapital</param>
        /// <param name="rate">Zinssatz</param>
        /// <returns>Zinsbetrag</returns>
        public static decimal CalculateInterestAmount(decimal capital, decimal rate)
        {
            return (capital * rate) / 100;
        }

        /// <summary>
        /// Zinsbetrag auf Tagesbasis
        /// </summary>
        /// <param name="capital">Kapital</param>
        /// <param name="rate">Zinssatz</param>
        /// <param name="days">Anzahl der Zinstage</param>
        /// <returns>Zinsbetrag auf Tagesbasis</returns>
        public static decimal CalculateInterestAmountOnDailyBasis(decimal capital, decimal rate, int days)
        {
            return (capital * rate * days) / (100 * 360);
        }

        /// <summary>
        /// Berechnet das Kapital
        /// </summary>
        /// <param name="rate">Zinssatz</param>
        /// <param name="interestAmount">Zinsen</param>
        /// <param name="days">Dauer in Tage</param>
        /// <returns>Kapital</returns>
        public static decimal CalculateCapital(decimal interestAmount, decimal rate, int days)
        {
            return (interestAmount * 100 * 360) / (days * rate);
        }

        /// <summary>
        /// Berechnet den Zinssatz
        /// </summary>
        /// <param name="capital">Kapital</param>
        /// <param name="interestAmount">Zinsen</param>
        /// <param name="days">Dauer in Tage</param>
        /// <param name="inPercent">Ausgabe in Prozent</param>
        /// <returns>Zinssatz</returns>
        public static decimal CalculateRate(decimal capital, decimal interestAmount, int days, bool inPercent = false)
        {
            if (inPercent)
            {
                return (interestAmount * 100) / (capital * days);
            }
            else
            {
                return (interestAmount) / (capital * days);
            }
        }

        /// <summary>
        /// Berechnet die Laufzeit
        /// </summary>
        /// <param name="capital">Kapital</param>
        /// <param name="interestAmount">Zinsen</param>
        /// <param name="rate">Zinssatz</param>
        /// <returns>Laufzeit in Tagen</returns>
        public static decimal CalculateDuration(decimal capital, decimal interestAmount, decimal rate)
        {
            return (100 * interestAmount) / (capital * rate);
        }

        /// <summary>
        /// Berechnet Zeitwert, Kapital zum bestimmten Zeitpunkt
        /// </summary>
        /// <param name="startCapital">Startkapital</param>
        /// <param name="rate">Zinssatz</param>
        /// <param name="days">Zeitpunkt in Tagen</param>
        /// <returns>Kapital</returns>
        public static decimal CalculateCapitalForTimepoint(decimal startCapital, decimal rate, int days)
        {
            return startCapital * (1 + rate / 100 * days);
        }

        /// <summary>
        /// Berechnet Barwert
        /// </summary>
        /// <param name="CapitalAtDay">Kapital zum bestimmten Zeitpunkt</param>
        /// <param name="rate">Zinssatz</param>
        /// <param name="days">Zeitpunkt in Tagen</param>
        /// <returns></returns>
        public static decimal CalculateCashValue(decimal CapitalAtDay, decimal rate, int days)
        {
            return CapitalAtDay / (1 + (rate / 100) * days);
        }
    }
}
