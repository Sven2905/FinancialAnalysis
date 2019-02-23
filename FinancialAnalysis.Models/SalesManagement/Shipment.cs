using DevExpress.Mvvm;
using System;
using Utilities;

namespace FinancialAnalysis.Models.SalesManagement
{
    /// <summary>
    /// Warenlieferung
    /// </summary>
    public class Shipment : BindableBase
    {
        public Shipment()
        {
            ShippedProducts = new SvenTechCollection<ShippedProduct>();
        }

        /// <summary>
        /// Id
        /// </summary>
        public int ShipmentId { get; set; }

        /// <summary>
        /// Liefernummer
        /// </summary>
        public string ShipmentNumber { get; set; }

        /// <summary>
        /// Versanddatum
        /// </summary>
        public DateTime ShipmentDate { get; set; }

        /// <summary>
        /// Positionen
        /// </summary>
        public SvenTechCollection<ShippedProduct> ShippedProducts { get; set; }
    }
}