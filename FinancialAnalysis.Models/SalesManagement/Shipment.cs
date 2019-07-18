using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;
using Utilities;

namespace FinancialAnalysis.Models.SalesManagement
{
    /// <summary>
    /// Warenlieferung
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
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

        /// <summary>
        /// Referenz-Id der Warenlieferung
        /// </summary>
        public int RefShipmentTypeId { get; set; } = 1;

        /// <summary>
        /// Versandtyp
        /// </summary>
        public ShipmentType ShipmentType { get; set; }

        /// <summary>
        /// Referenz auf die Bestellung
        /// </summary>
        public int RefSalesOrderId { get; set; }
    }
}