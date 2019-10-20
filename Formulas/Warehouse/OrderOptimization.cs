using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas.Warehouse
{
    public static class OrderOptimization
    {
        /// <summary>
        /// Berechnet die optimale Bestellmenge nach Andler
        /// </summary>
        /// <param name="annualConsumption">Jahresverbrauch</param>
        /// <param name="costPerOrder">Kosten je Bestellung</param>
        /// <param name="pricePerItem">Artikelpreis</param>
        /// <param name="interestAndStorageCostsRate">Zins- und Lagerhaltungskostensatz</param>
        /// <returns>Optimale Bestellmenge</returns>
        public static double CalculateEconomicOrderQuantityAndler(int annualConsumption, decimal costPerOrder, decimal pricePerItem, decimal interestAndStorageCostsRate)
        {
            return Math.Sqrt((double)(200 * annualConsumption * costPerOrder) / (double)(pricePerItem * interestAndStorageCostsRate));
        }

        /// <summary>
        /// Berechnet die optimale Bestellmenge nach Kosiol
        /// </summary>
        /// <param name="annualConsumption">Jahresverbrauch</param>
        /// <param name="costPerOrder">Kosten je Bestellung</param>
        /// <param name="pricePerItem">Artikelpreis</param>
        /// <param name="interestRate">Zinssatz</param>
        /// <param name="discount">Rabatt</param>
        /// <param name="holdingCosts">Lagerkostensatz</param>
        /// <returns>Optimale Bestellmenge</returns>
        public static double CalculateEconomicOrderQuantityKosiol(int annualConsumption, decimal costPerOrder, decimal pricePerItem, double interestRate, double discount, double holdingCosts)
        {
            return Math.Ceiling(Math.Sqrt((double)(200 * annualConsumption * costPerOrder) / ((double)pricePerItem * (interestRate * ((1 - discount / 100) / 100) + holdingCosts))));
        }

        /// <summary>
        /// Berechnet die optimale Bestellhäufigkeit
        /// </summary>
        /// <param name="annualConsumption">Jahresverbrauch</param>
        /// <param name="costPerOrder">Kosten je Bestellung</param>
        /// <param name="pricePerItem">Artikelpreis</param>
        /// <param name="interestAndStorageCostsRate">Zins- und Lagerhaltungskostensatz</param>
        /// <returns>Optimale Bestellhäufigkeit</returns>
        public static double CalculateOptimumOrderFrequency(int annualConsumption, decimal costPerOrder, decimal pricePerItem, decimal interestAndStorageCostsRate)
        {
            return Math.Ceiling(Math.Sqrt((double)(annualConsumption * pricePerItem * interestAndStorageCostsRate) / (double)(200 * costPerOrder)));
        }

        /// <summary>
        /// Berechnet den Bestellturnus
        /// </summary>
        /// <param name="annualConsumption">Jahresverbrauch</param>
        /// <param name="costPerOrder">Kosten je Bestellung</param>
        /// <param name="pricePerItem">Artikelpreis</param>
        /// <param name="interestAndStorageCostsRate">Zins- und Lagerhaltungskostensatz</param>
        /// <returns>Turnus in Tage</returns>
        public static double CalculateOrderRotation(int annualConsumption, decimal costPerOrder, decimal pricePerItem, decimal interestAndStorageCostsRate)
        {
            return Math.Round(365 / Math.Sqrt(CalculateOptimumOrderFrequency(annualConsumption, costPerOrder, pricePerItem, interestAndStorageCostsRate)));
        }

        /// <summary>
        /// Berechnet den Bestellturnus
        /// </summary>
        /// <param name="frequency">Bestellhäufigkeit</param>
        /// <returns>Turnus in Tage</returns>
        public static double CalculateOrderRotation(double frequency)
        {
            return Math.Round(365 / frequency);
        }
    }
}
